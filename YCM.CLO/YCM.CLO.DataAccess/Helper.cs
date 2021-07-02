using System;
using System.Configuration;
using System.Globalization;
using YCM.CLO.DataAccess.Contracts;
using System.Linq;

namespace YCM.CLO.DataAccess
{
    public static class Helper
    {
        //private static readonly IRepository _repository = new Repository();

        public static int GetPrevDayDateIdFromDB()
        {
            using (IRepository repository = new Repository())
            {
                return repository.GetPrevDayDateId();
            }
        }

        public static int GetPrevDayDateId()
        {
            int daysCount = -1;/* Diwakar Singh*/
            //return GetDateId(DateTime.Now.Date.AddDays(daysCount));
            if (ConfigurationManager.AppSettings.AllKeys.Contains("DatePrevious") && ConfigurationManager.AppSettings["DatePrevious"] != "")
                return Convert.ToInt32(ConfigurationManager.AppSettings["DatePrevious"]);
            else
                return GetDateId(DateTime.Now.Date.AddDays(daysCount));

        }

        public static int GetPrevToPrevDayDateId()
        {
            using (IRepository repository = new Repository())
            {
                return repository.GetPrevToPrevDayDateId();
            }
        }

        public static int GetPrevDayDateIdBasedOnM2F()
        {
            int daysCount = -1;
            return GetDateId(DateTime.Now.Date.AddDays(daysCount));
        }

        public static DateTime GetPrevBusinessDay(DateTime date, int daysCount)
        {
            date = date.AddDays(-1);

            while (daysCount > 0)
            {
                date = date.AddDays(-1);
                daysCount = daysCount - 1;
            }
            return date;
        }

        public static DateTime GetPrevBusinessDay(int daysCount)
        {
            return GetPrevBusinessDay(DateTime.Today, daysCount);
        }

        public static DateTime GetDateFromDateId(int dateId)
        {
            DateTime date;
            DateTime.TryParseExact(dateId.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }

        public static int GetPreviousDateFromDateId(int dateId)
        {
            var date = GetDateFromDateId(dateId);
            var prevDate = date.AddDays(-1);
            var prevDateId = GetDateId(prevDate);
            return prevDateId;
        }

        public static int GetDateId(DateTime date)
        {
            return int.Parse(date.ToString("yyyyMMdd"));
        }

    }
}
