using System;
using log4net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using YCM.Common.Utilities.Data;
using static YCM.Common.Utilities.Data.ScalarBuilder;

namespace WSOImport
{
	public static class WsoDataProcessor
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(WsoDataProcessor));
		private static readonly DataMartsWsoRepository _dataMartsWsoRepository = new DataMartsWsoRepository();
		private static readonly string _wsoConnectionString = ConfigurationManager.ConnectionStrings["WsoRepository"].ConnectionString;
		private static readonly IWsoRepository _wsoRepository = new WsoRepository();
		private static SqlConnection _wsoConnection;
		private static int _lastJobId;
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
				var batchJobIds = new List<int>();
				var bjs = _dataMartsWsoRepository.DatasetBatchJobs.OrderByDescending(x => x.DatasetBatchJobID).ToList();
				Log.Info($"Batch job count: {bjs.Count}");
				var lastJob = bjs.FirstOrDefault();
				if (lastJob != null)
				{
					_lastJobId = lastJob.DatasetBatchJobID;
					Log.Info($"The last job's id is: {_lastJobId}");
				}
				else
					Log.Info("Unable to find the last job.");
				//Query Requirements
				//https://msdn.microsoft.com/en-us/library/ms181122.aspx
				var sql = "SELECT [DatasetBatchJobID] FROM dbo.tblDatasetBatchJobs "
				          + $"WHERE BatchStatus = 3 AND DataSetBatchJobID > {_lastJobId}";
				Log.Info($"sql: {sql}");
				using (var batchJobCommand = new SqlCommand(sql, _wsoConnection))
				{
					using (var batchJobReader = batchJobCommand.ExecuteReader())
						batchJobIds = batchJobReader.ToList(ToInt);
				}
				Log.Info($"batch job id count: {batchJobIds.Count}");
				if (batchJobIds.Count > 0)
				{
					var batchJobs = _wsoRepository.DatasetBatchJobs.AsNoTracking().Where(j => batchJobIds.Contains(j.DatasetBatchJobID)).ToList();
					var datasetIds = batchJobs.GroupBy(j => j.DatasetId).Select(g => g.Key).ToList();
					var datasets = _wsoRepository.Datasets.AsNoTracking().Where(d => datasetIds.Contains(d.DatasetId)).ToList();

					//ORDER MATTERS TO MAKE SURE EXTRACT HAPPENS THEN ANY PROCS
					var datasetQueries = _dataMartsWsoRepository.Queries.Where(q => q.Watcher == "DatasetBatchJobWatcher")
						.OrderBy(q => q.DatasetQueryId).ToList();
					using (var transaction = _dataMartsWsoRepository.Database.BeginTransaction())
					{
						_dataMartsWsoRepository.DatasetBatchJobs.AddRange(batchJobs);
						_dataMartsWsoRepository.SaveChanges();
						foreach (var dataset in datasets)
						{
							var maxBatchId = batchJobs.Where(j => j.DatasetId == dataset.DatasetId).Max(j => j.DatasetBatchJobID);
							dataset.DatasetBatchJobId = maxBatchId;
						}
						_dataMartsWsoRepository.Datasets.AddRange(datasets);
						_dataMartsWsoRepository.SaveChanges();
						foreach (var dataset in datasets)
						{
							Log.Warn($"Extracting Dataset Queries for:{Environment.NewLine}{JsonConvert.SerializeObject(dataset, Formatting.Indented)}");


							foreach (var datasetQuery in datasetQueries)
							{
								Log.Info($"datasetQuery destination table: {datasetQuery.DestinationTable}");
								//THIS IS FOR BULK COPY, DONT LIKE THE STRING CHECK, SHOULD BE FLAG
								if (datasetQuery.DestinationTable != "NONBULK")
								{
									var queryCommand = new SqlCommand(string.Format(datasetQuery.Sql, dataset.DatasetId), _wsoConnection);
									Log.Info($"query command: {queryCommand}");
									using (var queryReader = queryCommand.ExecuteReader())
									{
										var dt = new DataTable();
										dt.Columns.Add(new DataColumn(datasetQuery.KeyColumn, typeof(int)) { AllowDBNull = true });
										dt.Columns.Add(new DataColumn("DatasetKey", typeof(int)) { DefaultValue = dataset.Id });
										dt.Columns.Add(new DataColumn("DateId", typeof(int))
										{
											DefaultValue = dataset.AsOfDate.GetDateId()
										});

										dt.Load(queryReader);
										var dataMartsConnection = _dataMartsWsoRepository.Database.Connection as SqlConnection;
										var underlyingTransaction = transaction.UnderlyingTransaction as SqlTransaction;
										using (var sqlBulkCopy = new SqlBulkCopy(dataMartsConnection, SqlBulkCopyOptions.Default, underlyingTransaction))
										{
											foreach (DataColumn col in dt.Columns)
												sqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);

											sqlBulkCopy.BulkCopyTimeout = 600;
											sqlBulkCopy.DestinationTableName = datasetQuery.DestinationTable;
											Log.Info($"bulk copy destination table: {sqlBulkCopy.DestinationTableName}");
											sqlBulkCopy.WriteToServer(dt);
										}
									}
								}
								else
								{
									//CALLING A STORED PROC
									var procSql = string.Format(datasetQuery.Sql, dataset.Id, dataset.AsOfDate.GetDateId());
									Log.Info($"proc call: {procSql}");
									var executionResult = _dataMartsWsoRepository.Database.ExecuteSqlCommand(procSql);
									Log.Info($"db stored proc execution result: {executionResult}");
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
				Log.Error(ex);
				Log.Info($"retry count: {_retry}");
				Console.WriteLine(ex);
				if (++_retry > 3)
					throw;
				Thread.Sleep(60 * _retry);
				Process();
			}
			_retry = 0;
		}

	}
}
