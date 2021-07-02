namespace YCM.CLO.DataAccess.Models
{
	public partial class AlertProcessor
    {
        public int AlertId { get; set; }
        public string AlertProcessorClassName { get; set; }
        public short ParameterTypeId { get; set; }
        public bool? IsActive { get; set; }
        public virtual ParameterType ParameterType { get; set; }
    }
}
