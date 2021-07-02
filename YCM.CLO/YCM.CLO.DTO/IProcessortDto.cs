using System.Collections.Generic;

namespace YCM.CLO.DTO
{
	public interface IProcessortDto
    {
        int IssuerId { get; set; }

        bool? IsOnAlert { get; set; }

        string FundCode { get; set; }

        IList<AlertDto> Alerts { get; set; }

        decimal NumExposure { get; set; }

        string PctExposure { get; set; }

        string Bid { get; set; }
        string Offer { get; set; }

        string MaturityDate { get; set; }

		decimal? PctExposureNum { get; set; }
    }
}
