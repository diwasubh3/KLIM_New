namespace WSOImport
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Dataset
    {
	    public int Id { get; set; }
	    public int DatasetBatchJobId { get; set; }
	    public int DatasetId { get; set; }
	    public int DealId { get; set; }
	    public DateTime AsOfDate { get; set; }
	    public string Title { get; set; }
	    public string Description { get; set; }
	    public int Status { get; set; }
	    public string StatusOwner { get; set; }
	    public int CompanyId { get; set; }
	    public DateTime WhenCreated { get; set; }
	    public string WhoCreated { get; set; }
	    public string WhereCreated { get; set; }
	    public DateTime WhenModified { get; set; }
	    public string WhoModified { get; set; }
	    public string WhereModified { get; set; }
	    public int ParentDatasetId { get; set; }
	    public bool IsManualMode { get; set; }
    }
}
