using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CsvProcessor
{
	public static class Utilities
	{
		private readonly static NLog.ILogger _logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

		public static int GetDateId(DateTime date)
			=> date.Year * 10000 + date.Month * 100 + date.Day;

		public static DateTime GetPrevDayDate(string connectionString)
			=> GetDateFromDateId(GetPrevDayDateId(connectionString));

		public static DateTime GetDateFromDateId(int dateId)
			=> DateTime.ParseExact(dateId.ToString()
				, "yyyyMMdd", CultureInfo.InvariantCulture);

		public static int GetPrevDayDateId(string connectionString)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (var cmd = new SqlCommand("SELECT CLO.GetPrevDayDateId()", connection))
				{
					return (int)cmd.ExecuteScalar();
				}
			}
		}

		public static void InsertMap(List<string> headers, string[] data, string connectionString)
		{
			var counter = 0;
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				foreach (var datum in data)
				{
					double dbl;
					DateTime dt;
					var columnName = RemoveSpecialCharacters(headers[counter]);
					var colType = 1;
					if (double.TryParse(datum, out dbl))
						colType = 2;
					else if (DateTime.TryParse(datum, out dt))
						colType = 3;
					_logger.Info($", {columnName} {colType}");
					using (var cmd = new SqlCommand())
					{
						var sql =
							"INSERT INTO CLO.FileImportColumnMap"
							+ "(FileImportId, TableColumnName, FileColumnName, FileColumnIndex, FileImportColumnTypeId)"
							+ $" VALUES(1, '{columnName}', NULL, {counter}, {colType})";
						cmd.CommandText = sql;
						cmd.Connection = connection;
						try
						{
							cmd.ExecuteNonQuery();
						}
						catch (Exception e)
						{
							_logger.Error(e);
						}
					}
					counter++;
				}
			}
		}

		public static string RemoveSpecialCharacters(string input)
		{
			var r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
			return r.Replace(input, String.Empty);
		}

	}
}
