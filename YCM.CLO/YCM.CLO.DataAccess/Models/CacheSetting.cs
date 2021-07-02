using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	[Table("CLO.CacheSetting")]
	public partial class CacheSetting : Entity
	{
		public int CacheSettingId { get; set; }
		[NotMapped]
		public override int Id { get => CacheSettingId; set => CacheSettingId = value; }
		public string CacheSettingKey { get; set; }
		public int CacheExpirationInSeconds { get; set; }
		[NotMapped]
		public override string CreatedBy { get; set; }
		[NotMapped]
		public override DateTime? CreatedOn { get; set; }
		[NotMapped]
		public override string LastUpdatedBy { get; set; }
		[NotMapped]
		public override DateTime? LastUpdatedOn { get; set; }
		public override string ToString() => $"Id: {Id} Key: {CacheSettingKey} Expiration in seconds: {CacheExpirationInSeconds}";

	}
}
