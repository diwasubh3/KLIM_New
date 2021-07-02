namespace YCM.CLO.DataAccess.Models
{
	public class AssetExposure
	{
		public int FundId { get; set; }
		public decimal AssetPar { get; set; }
		public decimal PrincipalCash { get; set; }

		public override string ToString() => $"Fund Id:{FundId} Asset Par:{AssetPar:C0} Cash:{PrincipalCash:C0}";
	}
}
