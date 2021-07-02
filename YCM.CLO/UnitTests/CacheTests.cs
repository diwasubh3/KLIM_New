using System;
using System.Dynamic;
using NUnit.Framework;
using YCM.CLO.Web.Objects;

namespace UnitTests
{
	[TestFixture]
	public class CacheTests
	{
		[Test]
		public void Testing()
		{
			try
			{
				var crap = CLOCache.GetAllPositions();
				var crappier = CLOCache.GetAllPositions();
				var crappiest = crap;
				var dyn = new ExpandoObject();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Assert.Fail($"{ex.Message}");
			}
		}
	}
}
