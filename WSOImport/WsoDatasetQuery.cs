namespace WSOImport
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("WsoDatasetQuery")]
    public partial class WsoDatasetQuery
    {
        [Key]
        public int DatasetQueryId { get; set; }

        [StringLength(100)]
        public string Watcher { get; set; }

        [Required]
        [StringLength(100)]
        public string DestinationTable { get; set; }

        [Required]
        [StringLength(100)]
        public string KeyColumn { get; set; }

        [Required]
        public string Sql { get; set; }
    }
}
