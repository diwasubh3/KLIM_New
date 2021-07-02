using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.MatrixGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            int cloId = args.Length > 0 ? int.Parse(args[0]) : 1;
            using (CLOContext context = new CLOContext())
            {
                var majorMatrixDataSpreads = context.MatrixDatas.Where(m => m.FundId == cloId && m.DataPointType == 1).OrderBy(m => m.Diversity).ThenBy(m => m.Spread).Take(10).ToList();
                for (int i = 0; i < majorMatrixDataSpreads.Count(); i++)
                {
                    var current = majorMatrixDataSpreads[i];
                    Console.WriteLine($"Current Row:{i} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity}, WarfModifier:{current.WarfModifier}");

                    if (i < majorMatrixDataSpreads.Count() - 1)
                    {
                        var next = majorMatrixDataSpreads[i + 1];
                        var spreadStep = (next.Spread - current.Spread) / 11;
                        var warfModifierStep = (next.WarfModifier - current.WarfModifier) / 11;
                        var warfStep = (next.Warf - current.Warf) / 11;
                        for (int j = 0; j < 10; j++)
                        {
                            Console.WriteLine($"Spread Child Row:{j} Spread:{current.Spread + (spreadStep*(j+1))}, Warf:{current.Warf + (warfStep * (j + 1))}, Diversity:{current.Diversity}, WarfModifier:{current.WarfModifier + (warfModifierStep * (j + 1))}");
                        }
                    }
                }

                var majorMatrixDataDiversities = context.MatrixDatas.Where(m => m.FundId == cloId && m.DataPointType == 1).OrderBy(m => m.Spread).ThenBy(m => m.Diversity).Take(10).ToList();
                for (int i = 0; i < majorMatrixDataDiversities.Count(); i++)
                {
                    var current = majorMatrixDataDiversities[i];
                    Console.WriteLine($"Row:{i} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity}, WarfModifier:{current.WarfModifier}");

                    if (i < majorMatrixDataDiversities.Count() - 1)
                    {
                        var next = majorMatrixDataDiversities[i + 1];
                        var diveristyStep = (next.Diversity - current.Diversity) / 6;
                        for (int j = 0; j < 5; j++)
                        {
                            Console.WriteLine($"Diversity Child Row:{j} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity + (diveristyStep * (j + 1))}, WarfModifier:{current.WarfModifier}");
                        }
                    }
                }
            }

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
