namespace WSOImport
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("WsoExtractTestResult")]
    public partial class WsoExtractTestResult
    {
        [Key]
        public int ExtractTestResultKey { get; set; }

        public int DateId { get; set; }

        public int DatasetKey { get; set; }

        public int DatasetId { get; set; }

        [Required]
        [StringLength(255)]
        public string ReportKey { get; set; }

        public int Counter { get; set; }

        public int ScenarioKey { get; set; }

        [StringLength(255)]
        public string PassFailStatus { get; set; }

        [StringLength(255)]
        public string Requirement { get; set; }

        [StringLength(255)]
        public string Outcome { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string MarketValue { get; set; }

        [StringLength(255)]
        public string TotalCap { get; set; }

        [StringLength(255)]
        public string ScenarioName { get; set; }

        public bool IsRefreshed { get; set; }

        public double? RequirementRaw { get; set; }

        public double? OutcomeRaw { get; set; }

        [Column(TypeName = "text")]
        public string RecordSource { get; set; }

        [StringLength(255)]
        public string Operator { get; set; }

        [StringLength(255)]
        public string DealAggregate { get; set; }

        public double? DealAggregateRaw { get; set; }

        [StringLength(255)]
        public string ReportTotal { get; set; }

        public double? ReportTotalRaw { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? BreachDate { get; set; }
    }
}
