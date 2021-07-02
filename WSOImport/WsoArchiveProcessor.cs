using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSOImport
{
	public static class WsoArchiveProcessor
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(WsoArchiveProcessor));
		private static readonly IDataMartsWsoRepository _dataMartsWsoRepository = new DataMartsWsoRepository();
		private static readonly string _wsoConnectionString = ConfigurationManager.ConnectionStrings["WsoRepository"].ConnectionString;
		private static readonly IWsoRepository _wsoRepository = new WsoRepository();
		private static SqlConnection _wsoConnection;
		private static int _lastArchivedDatasetId;
		private static int _retry;

		public static void Start()
		{
			Console.WriteLine("Starting copy process.");
			Log.Info($"Creating a new connection using the following connection string: {_wsoConnectionString}");
			_wsoConnection = new SqlConnection(_wsoConnectionString);
			Log.Info($"Opening connection to {_wsoConnection.DataSource} : {_wsoConnection.Database}");
			_wsoConnection.Open();
			Process();
			Log.Info("Done!");
			Console.WriteLine("Done!");
		}

		private static void Process()
		{
			try
			{
				var archivedDatasetIds = new List<int>();
				var lastArchivedDataset = _dataMartsWsoRepository.Datasets.OrderByDescending(d => d.DatasetId).FirstOrDefault(d => d.ParentDatasetId > 0);
				if (lastArchivedDataset != null)
					_lastArchivedDatasetId = lastArchivedDataset.DatasetId;
				//Query Requirements
				//https://msdn.microsoft.com/en-us/library/ms181122.aspx
				using (var archivedDatasetCommand = new SqlCommand(string.Format("SELECT [DatasetID] " +
																				 "FROM [dbo].[tblDatasets]" +
																				 "WHERE ParentDatasetID > 0 " +
																				 "AND DatasetId > '{0}'",
					_lastArchivedDatasetId), _wsoConnection))
				{
					using (var archivedDatasetReader = archivedDatasetCommand.ExecuteReader())
						if (archivedDatasetReader.HasRows)
							while (archivedDatasetReader.Read())
								archivedDatasetIds.Add(int.Parse(archivedDatasetReader[0].ToString()));
				}
				if (archivedDatasetIds.Count > 0)
				{
					var archivedDatasets = _wsoRepository.Datasets.AsNoTracking().Where(d => archivedDatasetIds.Contains(d.DatasetId)).ToList();
					var batchJobs = _dataMartsWsoRepository.DatasetBatchJobs.GroupBy(j => j.DatasetId).Select(s => new { DatasetId = s.Key, LastParentJobId = s.Max(g => g.DatasetBatchJobID) }).ToList();
					//ASSUMING this is correct
					archivedDatasets.ForEach(a => a.DatasetBatchJobId = batchJobs.First(j => j.DatasetId == a.ParentDatasetId).LastParentJobId);

					var datasetQueries = _dataMartsWsoRepository.Queries.Where(q => q.Watcher == "DatasetArchiveWatcher").ToList();


					using (var transaction = _dataMartsWsoRepository.Database.BeginTransaction())
					{
						_dataMartsWsoRepository.Datasets.AddRange(archivedDatasets);
						_dataMartsWsoRepository.SaveChanges();
						foreach (var dataset in archivedDatasets)
						{
							Log.Warn(string.Format("Extracting Dataset Queries for Archive: \n{0}",
								JsonConvert.SerializeObject(dataset, Formatting.Indented)));


							foreach (var datasetQuery in datasetQueries)
							{
								var queryCommand = new SqlCommand(string.Format(datasetQuery.Sql, dataset.DatasetId),
									_wsoConnection);
								using (var queryReader = queryCommand.ExecuteReader())
								{
									var dt = new DataTable();
									dt.Columns.Add(new DataColumn(datasetQuery.KeyColumn, typeof(int)) { AllowDBNull = true });
									dt.Columns.Add(new DataColumn("DatasetKey", typeof(int)) { DefaultValue = dataset.Id });
									dt.Columns.Add(new DataColumn("DateId", typeof(int))
									{
										DefaultValue = dataset.AsOfDate.GetDateId()
									});

									//dt.Columns.Add(new DataColumn("DatasetBatchJobId") {DefaultValue = maxBatchId});
									dt.Load(queryReader);
									using (
										var sqlBulkCopy =
											new SqlBulkCopy(
												(SqlConnection)_dataMartsWsoRepository.Database.Connection,
												SqlBulkCopyOptions.Default,
												(SqlTransaction)transaction.UnderlyingTransaction))
									{
										foreach (DataColumn col in dt.Columns)
											sqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);

										sqlBulkCopy.BulkCopyTimeout = 600;
										sqlBulkCopy.DestinationTableName = datasetQuery.DestinationTable;
										sqlBulkCopy.WriteToServer(dt);
									}
								}
							}
						}
						_dataMartsWsoRepository.SaveChanges();
						transaction.Commit();
					}
				}
			}
			catch (SqlException ex)
			{
				if (++_retry > 3)
				{
					Log.Error(ex.Message, ex);
					throw;
				}
				Thread.Sleep(60000);
				Process();
			}
			_retry = 0;
		}

	}
}
