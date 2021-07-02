using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace YCM.CLO.AnalystLinkage
{
	public static class Constants
	{
		static Constants()
		{
			HeaderFromRow = GetIntValueFromConfig(nameof(HeaderFromRow));
			HeaderToRow = GetIntValueFromConfig(nameof(HeaderToRow));
			HeaderColumn = GetIntValueFromConfig(nameof(HeaderColumn));
			DetailFromRow = GetIntValueFromConfig(nameof(DetailFromRow));
			DetailToRow = GetIntValueFromConfig(nameof(DetailToRow));
			DetailFromColumn = GetIntValueFromConfig(nameof(DetailFromColumn));
			DetailToColumn = GetIntValueFromConfig(nameof(DetailToColumn));
			LabelColumn = GetIntValueFromConfig(nameof(LabelColumn));
			NumberOfRelevantPeriods = GetIntValueFromConfig(nameof(NumberOfRelevantPeriods));
			MultiplierForComparison = GetIntValueFromConfig(nameof(MultiplierForComparison));
			IssuerNameProperty = ConfigurationManager.AppSettings[nameof(IssuerNameProperty)];
			CLOAnalystProperty = ConfigurationManager.AppSettings[nameof(CLOAnalystProperty)];
			HFAnalystProperty = ConfigurationManager.AppSettings[nameof(HFAnalystProperty)];
			QuarterEndedProperty = ConfigurationManager.AppSettings[nameof(QuarterEndedProperty)];
			FilePath = ConfigurationManager.AppSettings[nameof(FilePath)];
			LastUpdatedBy = ConfigurationManager.AppSettings[nameof(LastUpdatedBy)];
			ExcelExtensionNames = GetStringListFromCommaDelimitedSetting(nameof(ExcelExtensionNames))
				.Select(x => $".{x}").ToList();
			SheetExclusionList = GetStringListFromCommaDelimitedSetting(nameof(SheetExclusionList));
			EmailFrom = ConfigurationManager.AppSettings[nameof(EmailFrom)];
			EmailTo = ConfigurationManager.AppSettings[nameof(EmailTo)];
			EmailSubject = ConfigurationManager.AppSettings[nameof(EmailSubject)];
		}

		public const string IssuerName = "IssuerName";
		public const string CLOAnalyst = "CLOAnalyst";
		public const string HFAnalyst = "HFAnalyst";
		public const string QuarterEnded = "QuarterEnded";

		public static string EmailSubject { get; }
		public static string EmailFrom { get; }
		public static string EmailTo { get; }
		public static int MultiplierForComparison { get; }
		public static int NumberOfRelevantPeriods { get; }
		public static int HeaderFromRow { get; }
		public static int HeaderToRow { get; }
		public static int HeaderColumn { get; }
		/// <summary>
		/// Only used for the initial population of the row location table
		/// </summary>
		public static int DetailFromRow { get; }
		/// <summary>
		/// Only used for the initial population of the row location table
		/// </summary>
		public static int DetailToRow { get; }
		public static int DetailFromColumn { get; }
		public static int DetailToColumn { get; }
		public static int LabelColumn { get; }
		public static string IssuerNameProperty { get; }
		public static string CLOAnalystProperty { get; }
		public static string HFAnalystProperty { get; }
		public static string QuarterEndedProperty { get; }
		public static string FilePath { get; }
		public static string LastUpdatedBy { get; }
		public static List<string> ExcelExtensionNames { get; }
		public static List<string> SheetExclusionList { get; }

		public static int GetIntValueFromConfig(string key)
		{
			var setting = ConfigurationManager.AppSettings[key];
			var setValue = 0;
			int.TryParse(setting, out setValue);
			return setValue;
		}
		private static List<string> GetStringListFromCommaDelimitedSetting(string key)
		{
			var setting = ConfigurationManager.AppSettings[key];
			if(string.IsNullOrEmpty(setting))
				return new List<string>();
			var list = setting.Split(",".ToCharArray())
				.Where(x => !string.IsNullOrEmpty(x))
				.Select(x => x.Trim()).ToList();
			return list;
		}

	}
}
