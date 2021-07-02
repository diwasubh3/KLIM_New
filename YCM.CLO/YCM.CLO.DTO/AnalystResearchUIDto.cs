using System.Collections.Generic;

namespace YCM.CLO.DTO
{
	public class AnalystResearchUIDto
    {
        public AnalystResearchDto IssuerData { get; set; }
        public List<AnalystResearchDto> AnalystResearches { get; set; }
    }
}
