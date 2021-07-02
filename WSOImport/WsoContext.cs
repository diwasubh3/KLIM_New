namespace WSOImport
{
	using System.Data.Entity;

	//public partial class WsoContext : DbContext
	//{
	//	public WsoContext()
	//		: base("name=WsoContext")
	//	{
	//		//Configuration.ProxyCreationEnabled = false;
	//	}
	//	public virtual DbSet<WsoDatasetBatchJob> WsoDatasetBatchJobs { get; set; }
	//	public virtual DbSet<WsoDatasetQuery> WsoDatasetQueries { get; set; }
	//	public virtual DbSet<Dataset> WsoDatasets { get; set; }
	//	public virtual DbSet<WsoExtractTestResult> WsoExtractTestResults { get; set; }

	//	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	//	{
	//		//modelBuilder.Entity<WsoDatasetBatchJob>()
	//		//	.Property(e => e.BatchRequestor)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<WsoDatasetBatchJob>()
	//		//	.Property(e => e.WhoCreated)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<WsoDatasetBatchJob>()
	//		//	.Property(e => e.WhereCreated)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<WsoDatasetBatchJob>()
	//		//	.Property(e => e.WhoModified)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<WsoDatasetBatchJob>()
	//		//	.Property(e => e.WhereModified)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<WsoDatasetBatchJob>()
	//		//	.HasMany(e => e.WsoDatasets)
	//		//	.WithRequired(e => e.WsoDatasetBatchJob)
	//		//	.WillCascadeOnDelete(false);

	//		//modelBuilder.Entity<Dataset>()
	//		//	.Property(e => e.Title)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<Dataset>()
	//		//	.Property(e => e.Description)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<Dataset>()
	//		//	.Property(e => e.StatusOwner)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<Dataset>()
	//		//	.Property(e => e.WhoCreated)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<Dataset>()
	//		//	.Property(e => e.WhereCreated)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<Dataset>()
	//		//	.Property(e => e.WhoModified)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<Dataset>()
	//		//	.Property(e => e.WhereModified)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<WsoExtractTestResult>()
	//		//	.Property(e => e.RecordSource)
	//		//	.IsUnicode(false);

	//		//modelBuilder.Entity<WsoExtractTestResult>()
	//		//	.Property(e => e.ReportTotal)
	//		//	.IsUnicode(false);
	//	}
	//}
}
