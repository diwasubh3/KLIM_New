using System;
using System.Collections.Generic;
using System.Data;

namespace CsvProcessor.ExtensionMethods
{
	public static class IDataReaderExtensionMethods
	{
		public static IEnumerable<T> ToEnumerable<T>(this IDataReader reader, Func<IDataRecord, T> BuildObject)
		{
			try
			{
				while (reader.Read())
				{
					yield return BuildObject(reader);
				}
			}
			finally
			{
				reader.Dispose();
			}
		}
	}
}
