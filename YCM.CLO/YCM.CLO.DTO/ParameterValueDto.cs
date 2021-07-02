namespace YCM.CLO.DTO
{
	public class ParameterValueDto
    {
        public int Id { get; set; }
        public short ParameterTypeId { get; set; }
        public decimal? ParameterValueNumber { get; set; }
        public string ParameterValueText { get; set; }
        public decimal? ParameterMinValueNumber { get; set; }
        public decimal? ParameterMaxValueNumber { get; set; }
        public virtual ParameterTypeDto ParameterType { get; set; }
    }
}
