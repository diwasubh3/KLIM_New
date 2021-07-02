using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DTO;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Objects
{
	public class AlertEngine : IAlertEngine, IDisposable
    {
        private IRepository _repository;

        //public AlertEngine() : this(new Repository())
        //{

        //}
        public AlertEngine(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<IProcessortDto> ProcessAlerts(IEnumerable<IProcessortDto> positions,int dateId,string fundCode)
        {
            var alertProcessors = _repository.GetAlertProcessors().Select(a => new
            {
                Processor = Activator.CreateInstance(Type.GetType(a.AlertProcessorClassName))
            }).ToList();

            if (positions.Any())
            {
                alertProcessors.ForEach(alertProcessor =>
                {
                    ((IProcessor) alertProcessor.Processor).Process(positions, _repository,
                        fundCode,dateId);
                });
            }

            return positions;

        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
