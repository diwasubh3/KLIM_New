namespace YCM.CLO.DTO
{
	public interface IWatchProcessedDto
    {
        int IssuerId { get; set; }
        int SecurityId { get; set; }
		int? WatchId { get; set; }
		bool? IsOnWatch { get; set; }
		short? WatchObjectTypeId { get; set; }
		int? WatchObjectId { get; set; }
        string WatchComments { get; set; }
        string WatchLastUpdatedOn { get; set; }
        string WatchUser { get; set; }

        string FundCode { get; set; }
    }
}
