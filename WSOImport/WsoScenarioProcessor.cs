using System;
using log4net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace WSOImport
{
	public static class WsoScenarioProcessor
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(WsoScenarioProcessor));
		private static readonly DataMartsWsoRepository _dataMartsWsoRepository = new DataMartsWsoRepository();
		private static readonly string _wsoConnectionString = ConfigurationManager.ConnectionStrings["WsoRepository"].ConnectionString;
		private static SqlConnection _wsoConnection;
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
				var updatedScenarios = new List<ScenarioUniqueKey>();

				//Query Requirements
				//https://msdn.microsoft.com/en-us/library/ms181122.aspx
				using (var scenarioCommand = new SqlCommand(string.Format("SELECT " +
																		  "DatasetId, " +
																		  "ScenarioKey " +
																		  "FROM dbo.tblScenarios " +
																		  "WHERE IsRefreshed = 1"), _wsoConnection))
				{

					using (var scenarioReader = scenarioCommand.ExecuteReader())
						if (scenarioReader.HasRows)
							while (scenarioReader.Read())
								updatedScenarios.Add(new ScenarioUniqueKey
								{
									DatasetId = int.Parse(scenarioReader[0].ToString()),
									ScenarioKey = int.Parse(scenarioReader[1].ToString())
								});
				}

				var maxDatasetSubQuery = _dataMartsWsoRepository.Datasets.GroupBy(g => g.DatasetId)
					.Select(r => r.Max(g => g.Id));
				var maxDatasetKeys = _dataMartsWsoRepository.Datasets.Where(d => maxDatasetSubQuery.Contains(d.Id)).ToList();

				var lastProcessedDatasetKeys = _dataMartsWsoRepository.ExtractTestResults.GroupBy(g => new { g.DatasetId, g.ScenarioKey })
					.Select(r => new { r.Key, DatasetKey = r.Max(g => g.DatasetKey) }).ToList();

				var trackedDatasetsIds = maxDatasetKeys.Select(d => d.DatasetId).ToList();
				//Some scenario info doesn't have data sets still
				updatedScenarios.RemoveAll(s => !trackedDatasetsIds.Contains(s.DatasetId));
				var scenarioQuery = _dataMartsWsoRepository.Queries.First(q => q.Watcher == "ScenarioWatcher");

				foreach (var datasetScenario in updatedScenarios)
				{
					var datasetKey = maxDatasetKeys.First(j => j.DatasetId == datasetScenario.DatasetId);
					var lastProcessedDatasetKey = 0;
					if (lastProcessedDatasetKeys.Count > 0)
					{
						var lastProcessedDataset = lastProcessedDatasetKeys.FirstOrDefault(k => k.Key.DatasetId == datasetScenario.DatasetId && k.Key.ScenarioKey == datasetScenario.ScenarioKey);
						lastProcessedDatasetKey = lastProcessedDataset != null ? lastProcessedDataset.DatasetKey : 0;
					}

					//Only process if the scenario hasn't been processed already for that DatasetKey (Id)
					if (lastProcessedDatasetKey != datasetKey.Id)
						using (var transaction = _dataMartsWsoRepository.Database.BeginTransaction())
						{
							_dataMartsWsoRepository.Database.ExecuteSqlCommand(
								string.Format("DELETE FROM {0} WHERE DatasetKey = {1} AND ScenarioKey = {2}",
									ExtractTestResultConfiguration.Table,
									datasetKey.Id,
									datasetScenario.ScenarioKey));
							Log.Warn(string.Format("\nExtracting TestResults Scenario {0} for : \n{1}", datasetScenario.ScenarioKey, JsonConvert.SerializeObject(datasetKey, Formatting.Indented)));
							var queryCommand = new SqlCommand(string.Format(scenarioQuery.Sql, datasetScenario.DatasetId, datasetScenario.ScenarioKey),
								_wsoConnection);
							using (var queryReader = queryCommand.ExecuteReader())
							{
								var dt = new DataTable();
								dt.Columns.Add(new DataColumn(scenarioQuery.KeyColumn, typeof(int)));
								dt.Columns.Add(new DataColumn("DateId", typeof(int)) { DefaultValue = datasetKey.AsOfDate.GetDateId() });
								dt.Columns.Add(new DataColumn("DatasetKey", typeof(int)) { DefaultValue = datasetKey.Id });

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
									sqlBulkCopy.DestinationTableName = scenarioQuery.DestinationTable;

									sqlBulkCopy.WriteToServer(dt);
								}
							}
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

		private struct ScenarioUniqueKey
		{
			public int DatasetId;
			public int ScenarioKey;
		}
	}
}
