namespace YCM.CLO.DataAccess.Models
{
	public partial class ParameterValue
    {
        public int Id { get; set; }
        public short ParameterTypeId { get; set; }
        public decimal? ParameterValueNumber { get; set; }
        public string ParameterValueText { get; set; }
        public decimal? ParameterMinValueNumber { get; set; }
        public decimal? ParameterMaxValueNumber { get; set; }
        public virtual ParameterType ParameterType { get; set; }
    }
}
