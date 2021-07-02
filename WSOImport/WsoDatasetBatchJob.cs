namespace WSOImport
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class WsoDatasetBatchJob
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WsoDatasetBatchJob()
        {
            WsoDatasets = new HashSet<Dataset>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DatasetBatchJobID { get; set; }

        public int DatasetId { get; set; }

        public int BatchRequestID { get; set; }

        public int BatchRequestType { get; set; }

        public int BatchStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string BatchRequestor { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? WhenCreated { get; set; }

        [StringLength(50)]
        public string WhoCreated { get; set; }

        [StringLength(50)]
        public string WhereCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? WhenModified { get; set; }

        [StringLength(50)]
        public string WhoModified { get; set; }

        [StringLength(50)]
        public string WhereModified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dataset> WsoDatasets { get; set; }
    }
}
