using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DTO;

namespace YCM.CLO.Web.Objects
{
	public class WatchProcessor
    {
        public void Process(IRepository repository, IEnumerable<IWatchProcessedDto> watches,string fundCode)
        {
            var watchProcessedDtos = watches as IWatchProcessedDto[] ?? watches.ToArray();

            var securities = repository.GetSecurityWatch(watchProcessedDtos.Select(w => w.SecurityId).ToArray());

            var positionsDictionary = securities.ToDictionary(s => s.SecurityId, s => s);
            watchProcessedDtos.ToList().ForEach(w =>
            {
                w.IsOnWatch = positionsDictionary[w.SecurityId].IsOnWatch;
                w.WatchId = positionsDictionary[w.SecurityId].WatchId;
                w.WatchObjectTypeId = positionsDictionary[w.SecurityId].WatchObjectTypeId;
                w.WatchObjectId = positionsDictionary[w.SecurityId].WatchObjectId;
                w.WatchComments = positionsDictionary[w.SecurityId].WatchComments;
                w.WatchLastUpdatedOn = positionsDictionary[w.SecurityId].WatchLastUpdatedOn;
                w.WatchUser = positionsDictionary[w.SecurityId].WatchUser;
            });

        }
    }
}
