namespace YCM.CLO.DataAccess.Models
{
    public partial class vw_YorkCoreGenevaAnalyst
    {
        public int AnalystId { get; set; }

        public int AnalystTypeId { get; set; }

        public string AnalystCode { get; set; }

        public string AnalystDesc { get; set; }

        public int? AppUserId { get; set; }

		public override string ToString()
			=> $"{AnalystId}: {AnalystDesc} - {AnalystCode}";
	}
}
