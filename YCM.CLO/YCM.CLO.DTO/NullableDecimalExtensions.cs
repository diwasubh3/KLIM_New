namespace YCM.CLO.DTO
{
	public static class NullableDecimalExtensions
	{
		public static decimal ToDecimalOrOne(this decimal? value)
			=> value.GetValueOrDefault() != 0 ? value.Value : 1m;
	}
}
