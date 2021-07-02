using System.Collections.Generic;

namespace YCM.CLO.DTO
{
	public class RuleRestultDto
    {
        public IEnumerable<PositionDto> TopPositions { get; set; }
        public IEnumerable<PositionDto> BottomPositions { get; set; }
    }
}
