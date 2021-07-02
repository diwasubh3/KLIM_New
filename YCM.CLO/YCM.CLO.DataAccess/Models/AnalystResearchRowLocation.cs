using System.ComponentModel.DataAnnotations.Schema;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.DataAccess
{
	public partial class AnalystResearchRowLocation : Entity
    {
		[NotMapped]
		public override int Id
		{
			get { return AnalystResearchRowLocationId; }
			set { AnalystResearchRowLocationId = value; }
		}

		public int AnalystResearchRowLocationId { get; set; }

        public int RowIndex { get; set; }

        public string ClassName { get; set; }

        public string PropertyName { get; set; }
		public override string ToString()
			=> $"{PropertyName} {RowIndex}";


	}
}
