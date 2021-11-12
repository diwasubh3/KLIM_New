namespace YCM.CLO.DTO
{
	public class TotalParDto
    {
        public string Fund { get; set; }
        private decimal? _previous_TotalPar { get; set; }
        private decimal? _current_TotalPar { get; set; }
        private decimal? _totalpar_diff { get; set; }

        public decimal? Previous_TotalPar
        {
            get => _previous_TotalPar.GetValueOrDefault() > 0 ? _previous_TotalPar.Value : 0;
            set => _previous_TotalPar = value;
        }

        public decimal? Current_TotalPar
        {
            get => _current_TotalPar.GetValueOrDefault() > 0 ? _current_TotalPar.Value : 0;
            set => _current_TotalPar = value;
        }

        public decimal? TotalPar_Diff
        {
            get => _totalpar_diff.GetValueOrDefault() > 0 ? _totalpar_diff.Value : 0;
            set => _totalpar_diff = value;
        }
    }
}
