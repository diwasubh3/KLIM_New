using System;
using System.Linq;
using NUnit.Framework;
using YCM.CLO.DataAccess;
using static YCM.CLO.DataAccess.Constants;

namespace UnitTests
{
	[TestFixture]
	internal class EntitlementsTest
	{
		[Test]
		public void WhenUserIsASuperUser()
		{
			var crap = Enum.GetValues(typeof(CLOEntitlements)).Cast<int>()
				.Where(x => x <= (int)CLOEntitlements.Watch).ToList();
			var crappier = crap.Sum();
			Assert.AreEqual(EntitlementSuperUser, crappier);
		}
		[Test]
		public void WhenUserIsALegacyUser()
		{
			var crap = Enum.GetValues(typeof(CLOEntitlements)).Cast<int>()
				.Where(x => x < (int)CLOEntitlements.BuySell ).ToList();
			var crappier = crap.Sum();
			Assert.AreEqual(EntitlementLegacyUser, crappier);
		}
		[Test]
		public void WhenUserIsAnAdmin()
		{
			var crap = Enum.GetValues(typeof(CLOEntitlements)).Cast<int>().ToList();
			var crappier = crap.Sum();
			Assert.AreEqual(EntitlementAdmin, crappier);
		}
	}
}
