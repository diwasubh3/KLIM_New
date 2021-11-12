using System.Data.Entity;
using YCM.CLO.DataAccess.Models.Mapping;

namespace YCM.CLO.DataAccess.Models
{
	public partial class CLOContext : DbContext
    {
        static CLOContext()
        {
            Database.SetInitializer<CLOContext>(null);
        }

        public CLOContext()
            : base("Name=CLOContext")
        { }

	    public DbSet<UserDefaultCustomView> UserDefaultCustomViews { get; set; }
	    public DbSet<CustomView> CustomViews { get; set; }
		public DbSet<CustomViewField> CustomViewFields { get; set; }
		public DbSet<ApplicationRole> ApplicationRoles { get; set; }
	    public DbSet<UserApplicationRole> UserApplicationRoles { get; set; }
        public DbSet<AlertProcessor> AlertProcessors { get; set; }
        public DbSet<AnalystResearch> AnalystResearches { get; set; }
        public DbSet<AnalystResearchHeader> AnalystResearchHeaders { get; set; }
		public DbSet<AnalystResearchDetail> AnalystResearchDetails { get; set; }
		public DbSet<AnalystResearchRowLocation> AnalystResearchRowLocations { get; set; }
		public DbSet<AnalystResearchFile> AnalystResearchFiles { get; set; }
		public DbSet<Calculation> Calculations { get; set; }
        public DbSet<FundCalculation> FundCalculations { get; set; }
        public DbSet<Country> Countries { get; set; }
	    public DbSet<CreditScore> CreditScores { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldGroup> FieldGroups { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundRestriction> FundRestrictions { get; set; }
        public DbSet<FundRestrictionType> FundRestrictionTypes { get; set; }
        public DbSet<GicsToSnpMoodyIndustryMap> GicsToSnpMoodyIndustryMaps { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Issuer> Issuers { get; set; }
        public DbSet<LienType> LienTypes { get; set; }
        public DbSet<MarketData> MarketDatas { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<ParameterType> ParameterTypes { get; set; }
        public DbSet<ParameterValue> ParameterValues { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<RuleField> RuleFields { get; set; }
        public DbSet<RuleSectionType> RuleSectionTypes { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<SecurityOverride> SecurityOverrides { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<TradeAllocation> TradeAllocations { get; set; }
        public DbSet<TradeSwap> TradeSwaps { get; set; }
        public DbSet<TradeSwapSnapshot> TradeSwapSnapshots { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Watch> Watches { get; set; }
        public DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public DbSet<vw_AnalystResearch> vw_AnalystResearch { get; set; }
        public DbSet<vw_Calculation> vw_Calculation { get; set; }
        public DbSet<vw_CLOSummary> vw_CLOSummary { get; set; }
        public DbSet<vw_CurrentActiveSecurityOverrides> vw_CurrentActiveSecurityOverrides { get; set; }
        public DbSet<vw_CurrentAnalystResearch> vw_CurrentAnalystResearch { get; set; }
        
        public DbSet<vw_MarketData> vw_MarketData { get; set; }
        public DbSet<vw_Position> vw_Position { get; set; }
        public DbSet<vw_Price> vw_pricing { get; set; }

        public DbSet<vw_TradeSwap> vw_TradeSwap { get; set; }
        public DbSet<vw_Security> vw_Security { get; set; }
        public DbSet<vw_SecurityMarketCalculation> vw_SecurityMarketCalculation { get; set; }

        public DbSet<vw_SecurityFund> vwSecurityFunds { get; set; }
        public DbSet<vw_AggregatePosition> vw_AggregatePosition { get; set; }
        public DbSet<v_PhoneList> v_PhoneList { get; set; }
        public DbSet<WSOExtractAsset> WSOExtractAssets { get; set; }

        public DbSet<vw_PriceMove> vw_PriceMove { get; set; }
		public DbSet<vw_YorkCoreGenevaAnalyst> vw_YorkCoreGenevaAnalysts { get; set; }
		public DbSet<CacheSetting> CacheSettings { get; set; }

        public DbSet<MatrixData> MatrixDatas { get; set; }

        public DbSet<vw_MatrixData> vw_MatrixDatas { get; set; }

        public DbSet<MatrixPoint> MatrixPoints { get; set; }

        public DbSet<AssetClass> AssetClasses { get; set; }

        public DbSet<FundAssetClass> FundAssetClasses { get; set; }

        public DbSet<DefaultSecurity> DefaultSecurities { get; set; }


        public DbSet<EquityOverride> EquityOverrides { get; set; }


        public DbSet<vw_Mismatch> MismatchCfrsAdj { get; set; }
        public DbSet<vw_ReinvestDetails> vw_ReinvestDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new FundAssetClassMap());
            modelBuilder.Configurations.Add(new AlertProcessorMap());
            modelBuilder.Configurations.Add(new AnalystResearchMap());
			modelBuilder.Configurations.Add(new AnalystResearchFileMap());
			modelBuilder.Configurations.Add(new AnalystResearchHeaderMap());
            modelBuilder.Configurations.Add(new AnalystResearchDetailMap());
			modelBuilder.Configurations.Add(new AnalystResearchHeaderHistoryMap());
			modelBuilder.Configurations.Add(new AnalystResearchDetailHistoryMap());
			modelBuilder.Configurations.Add(new AnalystResearchImportLogMap());
			modelBuilder.Configurations.Add(new AnalystResearchRowLocationMap());
			modelBuilder.Configurations.Add(new CalculationMap());
            modelBuilder.Configurations.Add(new FundCalculationMap());
            modelBuilder.Configurations.Add(new CountryMap());
	        modelBuilder.Configurations.Add(new CreditScoreMap());
            modelBuilder.Configurations.Add(new FacilityMap());
            modelBuilder.Configurations.Add(new FieldMap());
            modelBuilder.Configurations.Add(new FieldGroupMap());
            modelBuilder.Configurations.Add(new FundMap());
            modelBuilder.Configurations.Add(new FundRestrictionMap());
            modelBuilder.Configurations.Add(new FundRestrictionTypeMap());
            modelBuilder.Configurations.Add(new GicsToSnpMoodyIndustryMapMap());
            modelBuilder.Configurations.Add(new IndustryMap());
            modelBuilder.Configurations.Add(new IssuerMap());
            modelBuilder.Configurations.Add(new LienTypeMap());
            modelBuilder.Configurations.Add(new MarketDataMap());
            modelBuilder.Configurations.Add(new OperatorMap());
            modelBuilder.Configurations.Add(new ParameterTypeMap());
            modelBuilder.Configurations.Add(new ParameterValueMap());
            modelBuilder.Configurations.Add(new PositionMap());
            modelBuilder.Configurations.Add(new PricingMap());
            modelBuilder.Configurations.Add(new RatingMap());
            modelBuilder.Configurations.Add(new RuleMap());
            modelBuilder.Configurations.Add(new RuleFieldMap());
            modelBuilder.Configurations.Add(new RuleSectionTypeMap());
            modelBuilder.Configurations.Add(new SecurityMap());
            modelBuilder.Configurations.Add(new SecurityOverrideMap());
            modelBuilder.Configurations.Add(new TradeMap());
            modelBuilder.Configurations.Add(new TradeAllocationMap());
            modelBuilder.Configurations.Add(new TradeSwapMap());
            modelBuilder.Configurations.Add(new TradeSwapSnapshotMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new WatchMap());
            modelBuilder.Configurations.Add(new C__RefactorLogMap());
            modelBuilder.Configurations.Add(new vw_AnalystResearchMap());
            modelBuilder.Configurations.Add(new vw_CalculationMap());
            modelBuilder.Configurations.Add(new vw_CLOSummaryMap());
            modelBuilder.Configurations.Add(new vw_CurrentActiveSecurityOverridesMap());
            modelBuilder.Configurations.Add(new vw_CurrentAnalystResearchMap());
            modelBuilder.Configurations.Add(new vw_MarketDataMap());
            modelBuilder.Configurations.Add(new vw_PositionMap());
            modelBuilder.Configurations.Add(new vw_PriceMap());
            modelBuilder.Configurations.Add(new vw_PriceMoveMap());
            modelBuilder.Configurations.Add(new vw_SecurityMap());
            modelBuilder.Configurations.Add(new vw_SecurityMarketCalculationMap());
            modelBuilder.Configurations.Add(new vw_SecurityFundMap());
            modelBuilder.Configurations.Add(new v_PhoneListMap());
            modelBuilder.Configurations.Add(new vw_TradeSwapMap());
            modelBuilder.Configurations.Add(new vw_AggregatePositionMap());
            modelBuilder.Configurations.Add(new WSOExtractAssetMap());
			modelBuilder.Configurations.Add(new vw_YorkCoreGenevaAnalystMap());
	        modelBuilder.Configurations.Add(new CustomViewFieldMap());
	        modelBuilder.Configurations.Add(new CustomViewMap());
	        modelBuilder.Configurations.Add(new ApplicationRoleMap());
	        modelBuilder.Configurations.Add(new UserApplicationRoleMap());
	        modelBuilder.Configurations.Add(new UserDefaultCustomViewMap());
            modelBuilder.Configurations.Add(new MatrixDataMap());
            modelBuilder.Configurations.Add(new vw_MatrixDataMap());
            modelBuilder.Configurations.Add(new MatrixPointMap());
            modelBuilder.Configurations.Add(new vw_ReinvestDetailsMap());
        }
	}
}
