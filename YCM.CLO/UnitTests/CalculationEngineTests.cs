using System;
using Excel.FinancialFunctions;
using NUnit.Framework;
using YCM.CLO.CalculationEngine;
using YCM.CLO.CalculationEngine.Contracts;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;

namespace UnitTests
{
	[TestFixture]
	public class CalculationEngineTests
	{
		private readonly ICalculationEngine _calculationEngine;
		private readonly IRepository _repo;

		public CalculationEngineTests()
		{
			try
			{
				_calculationEngine = new CalculationEngine();
				_repo = new Repository();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		//[Test]
		public void WhenACalcIsRequested()
		{
			try
			{
				var dateId = _repo.GetPrevDayDateId();
				_calculationEngine.Calculate(dateId, "test");
			}
			catch (Exception ex)
			{
				Assert.Fail($"Calc failed. {ex}");
			}
		}

		//[Test]
		//public void CalcMidYield()
		//{

		//	var maturityOffer = new DateTime(2025, 12, 17);
		//	var maturityBid = new DateTime(2025, 12, 17);
		//	var priceDate = DateTime.Today;
		//	var Offer = 98.13m;
		//	var Bid = 97.38m;
		//	var maturityMid = (maturityOffer + maturityBid) / 2;
		//	var Mid = (Offer + Bid) / 2;
		//	Decimal midYield = Math.Round((decimal)Financial.Yield(priceDate, maturityMid, couponPct, Mid, 100.0, Frequency.Annual, DayCountBasis.ActualActual) * 100, 2);

		//}

		[Test]
		public void WhenTradeFileIsProcessed()
		{
			try
			{
				var dateId = Helper.GetPrevDayDateId();
				var fileDateId = Helper.GetPrevDayDateIdBasedOnM2F();
				var engine = new CalculationEngine();
				engine.ProcessTradeFile(3, fileDateId, dateId);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Processing failed. {ex}");
			}
		}

	}
}
