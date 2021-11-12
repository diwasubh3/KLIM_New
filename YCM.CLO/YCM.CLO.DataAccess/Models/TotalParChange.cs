namespace YCM.CLO.DataAccess.Models
{
    public class TotalParChange
    {
        public string Fund { get; set; }
        public decimal? Previous_TotalPar { get; set; }
        public decimal? Current_TotalPar { get; set; }
        public decimal? TotalPar_Diff { get; set; }
    }
}
