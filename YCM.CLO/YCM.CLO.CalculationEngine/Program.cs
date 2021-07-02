using Topshelf;
using YCM.CLO.CalculationEngine.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine
{
	class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(configure =>
            {
                configure.Service<IService>(service =>
                {
                    service.ConstructUsing(s => new CalculationEngineService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("CLOCalculationEngine");
                configure.SetDisplayName("CLO Calculation Engine");
                configure.SetDescription("This is a calculation engine for CLO.");
            });
        }
    }
}
