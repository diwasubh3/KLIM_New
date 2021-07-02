using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
    public partial class AnalystResearchFile : Entity
    {
		[NotMapped]
		public override int Id
		{
			get { return AnalystResearchFileId; }
			set { AnalystResearchFileId = value; }
		}
		public int AnalystResearchFileId { get; set; }

        public string AnalystResearchFileName { get; set; }

        public DateTime LastFileUpdate { get; set; }

    }
}
