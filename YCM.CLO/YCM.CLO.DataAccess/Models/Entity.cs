using System;
using YCM.CLO.DataAccess.Contracts;

namespace YCM.CLO.DataAccess.Models
{
	public abstract class Entity : IEntity
	{
		public abstract int Id { get; set; }
		public virtual DateTime? CreatedOn { get; set; }

		public virtual string CreatedBy { get; set; }

		public virtual DateTime? LastUpdatedOn { get; set; }

		public virtual string LastUpdatedBy { get; set; }

	}
}
