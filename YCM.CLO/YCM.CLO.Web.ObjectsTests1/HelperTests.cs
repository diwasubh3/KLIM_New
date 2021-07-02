using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YCM.CLO.DataAccess;

namespace YCM.CLO.Web.Objects.Tests
{
	[TestClass()]
    public class HelperTests
    {
        //[TestMethod()]
        //public void GetPrevDayDateIdTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void GetPrevBusinessDayTest()
        {
            DateTime dateTime = Helper.GetPrevBusinessDay(DateTime.Parse("2017-09-25"), 1);
            Assert.AreEqual(dateTime, DateTime.Parse("2017-09-21"));

            DateTime dateTime2 = Helper.GetPrevBusinessDay(DateTime.Parse("2017-09-26"), 1);
            Assert.AreEqual(dateTime2, DateTime.Parse("2017-09-24"));

            DateTime dateTime3 = Helper.GetPrevBusinessDay(DateTime.Parse("2017-09-27"), 1);
            Assert.AreEqual(dateTime3, DateTime.Parse("2017-09-25"));

            DateTime dateTime4 = Helper.GetPrevBusinessDay(DateTime.Parse("2017-09-28"), 1);
            Assert.AreEqual(dateTime4, DateTime.Parse("2017-09-26"));

            DateTime dateTime5 = Helper.GetPrevBusinessDay(DateTime.Parse("2017-09-29"), 1);
            Assert.AreEqual(dateTime5, DateTime.Parse("2017-09-27"));

            DateTime dateTime6 = Helper.GetPrevBusinessDay(DateTime.Parse("2017-10-02"), 1);
            Assert.AreEqual(dateTime6, DateTime.Parse("2017-09-28"));


            DateTime dateTime7 = Helper.GetPrevBusinessDay(DateTime.Parse("2017-10-03"), 1);
            Assert.AreEqual(dateTime7, DateTime.Parse("2017-10-01"));

        }

        //[TestMethod()]
        //public void GetPrevBusinessDayTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetDateIdTest()
        //{
        //    Assert.Fail();
        //}
    }
}