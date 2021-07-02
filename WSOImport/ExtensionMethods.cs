using System;

namespace WSOImport
{
	public static class ExtensionMethods
	{
		public static int GetDateId(this DateTime date)
			=> date.Year * 10000 + date.Month * 100 + date.Day;
	}
}
