namespace YCM.CLO.DTO
{
	public class RatingDto
    {
        public short RatingId { get; set; }
        public string RatingDesc { get; set; }
        public short? Rank { get; set; }
        public bool? IsMoody { get; set; }
        public bool? IsSnP { get; set; }
        public bool? IsFitch { get; set; }
    }
}
