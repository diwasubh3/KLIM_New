using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvProcessor.ExtensionMethods
{
    public static class DateTimeExtensionMethods
    {
        public static DateTime CurrentMonthStartDate  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public static DateTime CurrentMonthEndDate  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
        public static DateTime PreviousMonthEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
    }
}
