using System;
using System.Collections.Generic;
using System.Configuration;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.DataAccess
{
	/// <summary>
	/// permissions :
	/// 1   :   Application Permission:                 1                           
	/// 2   :   Position Screen                         10                          
	/// 3   :   Top10Bottom10                           100                           
	/// 4   :   Trade                                   1000
	/// 5   :   Trade Swapping                          10000 
	/// 6   :   Analyst Research                        100000  
	/// 7   :   Bid Offer Upload                        1000000  
	/// 9   :   Fund Triggers                           10000000 
	/// 10  :   Fund Overrides                          100000000
	/// 11  :   Parameters                              1000000000
	/// 12  :   Loan Attribute Override                 10000000000
	/// 13  :   New Loan Recon                          100000000000
	/// 14  :   Loan Attribute Overrides Recon          1000000000000
	/// 15  :   Buy/Sell                                10000000000000
	/// 16  :   Watch                                   100000000000000
	/// 17  :                                           10000000000000000
	/// 17  :                                           100000000000000000
	/// 18  :   Admin                                   1000000000000000000 
	/// </summary>
	[Flags]
	public enum CLOEntitlements
	{
		ApplicationAccess = 1
		, Position = 2
		, Top10Bottom10 = 4
		, Trade = 8
		, TradeSwapping = 16
		, AnalystResearch = 32
		, BidOfferUpload = 64
		, FundTriggers = 128
		, FundOverrides = 256
		, Parameters = 512
		, LoanAttributeOverride = 1024
		, NewLoanRecon = 2048
		, LoanAttributeOverridesRecon = 4096
		, BuySell = 8192
		, Watch = 16384
		, Blah1 = 32768
		, Blah2 = 65536
		, Blah3 = 131072
		, Admin = 262144
	}

	public static class Constants
	{
		static Constants()
		{
			CacheData = GetBoolValueFromConfig(nameof(CacheData), true);
			DefaultCacheExpirationInSeconds = GetIntValueFromConfig(nameof(DefaultCacheExpirationInSeconds), 300);
			EntitlementSuperUser = GetIntValueFromConfig(nameof(EntitlementSuperUser));
		}

		public const string CacheSettingsCacheKey = "CacheSetting";
		public const string PositionsCacheKey = "Positions";
		public const string SummariesCacheKey = "Summaries";

		public const short FilterColumnFieldId = 121;
		public const short SearchTextFieldId = 51;
		public static bool CacheData { get; }
		public static int DefaultCacheExpirationInSeconds { get; }
		public static int EntitlementSuperUser { get; }// = 32767;
		public const int EntitlementLegacyUser = 8191;
		public const int EntitlementAdmin = 524287;
		public const string CurrentVsPreviousAssetParAlert = "CurrentVsPreviousAssetParAlert";
		public static IEnumerable<FundRestrictionType> AssetParFundRestrictionTypes
		{
			get
			{
				//yield return new FundRestrictionType { DisplayColor = string.Empty, FundRestrictionTypeName = CustomFundRestrictions.TODAY.ToString(), FundRestrictionTypeId = (int)CustomFundRestrictions.TODAY, SortOrder = (int)CustomFundRestrictions.TODAY };
				//yield return new FundRestrictionType { DisplayColor = string.Empty, FundRestrictionTypeName = CustomFundRestrictions.YESTERDAY.ToString(), FundRestrictionTypeId = (int)CustomFundRestrictions.YESTERDAY, SortOrder = (int)CustomFundRestrictions.YESTERDAY };
				yield return new FundRestrictionType { DisplayColor = "RED", FundRestrictionToolTip = string.Empty, FundRestrictionTypeName = CurrentVsPreviousAssetParAlert, FundRestrictionTypeId = (int)CustomFundRestrictions.CURRENTVSPREVIOUSASSPAR, SortOrder = (int)CustomFundRestrictions.CURRENTVSPREVIOUSASSPAR };
			}
		}

		static bool GetBoolValueFromConfig(string key, bool defaultValue = false)
		{
			bool val;
			if (!bool.TryParse(ConfigurationManager.AppSettings[key], out val))
				val = defaultValue;
			return val;
		}
		static int GetIntValueFromConfig(string key, int defaultValue = 0)
		{
			int val;
			if (!int.TryParse(ConfigurationManager.AppSettings[key], out val))
				val = defaultValue;
			return val;
		}
	}
	public enum CustomFundRestrictions
	{
		//TODAY = 300, YESTERDAY = 400, DIFFERENCE = 500
		CURRENTVSPREVIOUSASSPAR = 500
	}
}
