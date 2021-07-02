using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class FieldGroup
    {
        public FieldGroup()
        {
            this.Fields = new List<Field>();
        }

        public short FieldGroupId { get; set; }
        public string FieldGroupName { get; set; }
        public short? SortOrder { get; set; }
        public string DisplayIcon { get; set; }
        public bool? ShowOnPositions { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
    }
}
