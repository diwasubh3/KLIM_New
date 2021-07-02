//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using YCM.CLO.DataAccess.Contracts;
//using YCM.CLO.DTO;
//using YCM.CLO.Web.Objects.Contract;

//namespace YCM.CLO.Web.Objects
//{
//    public class PctPositionProcessor : IProcessor
//    {
//        public IEnumerable<IProcessortDto> Process(IEnumerable<IProcessortDto> positions, IRepository repository, string fundCode, int dateId)
//        {
//            var totalExposure = positions.Sum(p => p.NumExposure);

//            positions.ToList().ForEach(p =>
//            {
//                p.PctExposureNum = ((p.NumExposure/totalExposure)*100);
//                p.PctExposure = p.PctExposureNum.Value.ToString("F") + " %";
//            });

//            return positions;
//        }
//    }

//}
