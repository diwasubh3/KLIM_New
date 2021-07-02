using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class ParameterType
    {
        public ParameterType()
        {
            this.AlertProcessors = new List<AlertProcessor>();
            this.ParameterValues = new List<ParameterValue>();
        }

        public short ParameterTypeId { get; set; }
        public string ParameterTypeName { get; set; }
        public virtual ICollection<AlertProcessor> AlertProcessors { get; set; }
        public virtual ICollection<ParameterValue> ParameterValues { get; set; }
    }
}
