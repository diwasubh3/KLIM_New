using System;
using System.Data;

namespace ExportCsv.ExtensionMethods
{
	public static class IDataRecordExtensionMethods
	{
		public static bool GetBoolValueOrDefault(this IDataRecord record, string columnName)
		{
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentException($"{columnName} cannot be null or empty!");
			var val = default(bool);
			try
			{
				val = bool.Parse(record[columnName].ToString());
			}
			catch
			{
			}
			return val;
		}
		public static int GetIntValueOrDefault(this IDataRecord record, string columnName)
		{
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentException($"{columnName} cannot be null or empty!");
			var val = default(int);
			try
			{
				val = int.Parse(record[columnName].ToString());
			}
			catch
			{
			}
			return val;
		}
	}
}
