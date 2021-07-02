using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace WSOImport
{
	public interface IWsoRepository : IDisposable, IObjectContextAdapter
	{
		DbSet<WsoDatasetBatchJob> DatasetBatchJobs { get; set; }
		DbSet<Dataset> Datasets { get; set; }
		DbContextConfiguration Configuration { get; }
		Database Database { get; }
		int SaveChanges();
		Task<int> SaveChangesAsync();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		IEnumerable<DbEntityValidationResult> GetValidationErrors();
		DbEntityEntry Entry(object entity);
		DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
	}

	internal abstract class BaseWsoRepository : DbContext, IWsoRepository
	{
		internal BaseWsoRepository(string connection)
			: base(connection)
		{
			Configuration.AutoDetectChangesEnabled = false;
			Configuration.ValidateOnSaveEnabled = false;
			Database.SetInitializer<BaseWsoRepository>(null);
			Configuration.ProxyCreationEnabled = false;
		}

		public DbSet<WsoDatasetBatchJob> DatasetBatchJobs { get; set; }
		public DbSet<Dataset> Datasets { get; set; }
	}


	internal class WsoRepository : BaseWsoRepository
	{
		public WsoRepository()
			: base("WsoRepository")
		{
			Database.SetInitializer<WsoRepository>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new DatasetBatchJobConfiguration("dbo", "tbl"));

			var datasetConfig = new DatasetConfiguration("dbo", "tbl");
			datasetConfig.HasKey(m => m.DatasetId);
			datasetConfig.Property(d => d.DatasetId)
				.HasColumnName("DatasetID")
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			datasetConfig.Ignore(d => d.Id);
			datasetConfig.Ignore(d => d.DatasetBatchJobId);
			modelBuilder.Configurations.Add(datasetConfig);
		}
	}


	public interface IDataMartsWsoRepository : IWsoRepository
	{
		DbSet<WsoDatasetQuery> Queries { get; set; }
		DbSet<WsoExtractTestResult> ExtractTestResults { get; set; }
	}


	internal sealed class DatasetBatchJobConfiguration : EntityTypeConfiguration<WsoDatasetBatchJob>
	{
		public DatasetBatchJobConfiguration(string schema, string tablePrefix)
		{
			ToTable(string.Format("{0}.{1}DatasetBatchJobs", schema, tablePrefix));
			HasKey(m => m.DatasetBatchJobID);
			Property(job => job.DatasetBatchJobID)
				.HasColumnName("DatasetBatchJobID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
		}
	}

	internal sealed class QueryConfiguration : EntityTypeConfiguration<WsoDatasetQuery>
	{
		public QueryConfiguration()
		{
			ToTable("dbo.WsoDatasetQuery");
			HasKey(m => m.DatasetQueryId);
			Property(q => q.DatasetQueryId)
				.HasColumnName("DatasetQueryId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}
	}

	internal sealed class ExtractTestResultConfiguration : EntityTypeConfiguration<WsoExtractTestResult>
	{
		public ExtractTestResultConfiguration()
		{
			ToTable(Table);
			HasKey(m => m.ExtractTestResultKey);
			Property(q => q.ExtractTestResultKey)
				.HasColumnName("ExtractTestResultId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}

		internal static string Table
		{
			get { return "dbo.WsoExtractTestResult"; }
		}
	}

	internal class DatasetConfiguration : EntityTypeConfiguration<Dataset>
	{
		public DatasetConfiguration(string schema, string tablePrefix)
		{
			ToTable(string.Format("{0}.{1}Datasets", schema, tablePrefix));
		}
	}
	internal class DataMartsWsoRepository : BaseWsoRepository, IDataMartsWsoRepository
	{
		public DataMartsWsoRepository()
			: base("DataMartsRepository")
		{
			Database.SetInitializer<DataMartsWsoRepository>(null);
		}

		public DbSet<WsoDatasetQuery> Queries { get; set; }
		public DbSet<WsoExtractTestResult> ExtractTestResults { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new DatasetBatchJobConfiguration("dbo", "Wso"));
			var datasetConfig = new DatasetConfiguration("dbo", "Wso");
			datasetConfig.HasKey(m => m.Id);
			datasetConfig.Property(d => d.Id)
				.HasColumnName("DatasetKey").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Configurations.Add(datasetConfig);
			modelBuilder.Configurations.Add(new QueryConfiguration());
			modelBuilder.Configurations.Add(new ExtractTestResultConfiguration());
		}
	}
}
