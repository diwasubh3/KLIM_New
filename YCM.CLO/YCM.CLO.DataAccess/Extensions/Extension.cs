using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;

namespace YCM.CLO.DataAccess.Extensions
{
    public static class Extension
    {
        public static bool IsDate(this string strDate)
        {
            try
            {
				DateTime dt;

                bool isvalid = DateTime.TryParse(strDate, out dt);

                isvalid = isvalid && dt > DateTime.MinValue && dt < DateTime.MaxValue && dt > (DateTime)SqlDateTime.MinValue && dt < (DateTime)SqlDateTime.MaxValue;

                return isvalid;

            }
            catch
            {
                return false;
            }
        }

        public static bool IsDecimal(this string strDecimal)
        {
            try
            {
                strDecimal = strDecimal.Replace("x", "").Replace("%", "");
                Decimal dc;
                return decimal.TryParse(strDecimal, out dc);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInt(this string strNum)
        {
            try
            {
                int num;
                return int.TryParse(strNum, out num);
            }
            catch
            {
                return false;
            }
        }


        public static bool IsString(this string str)
        {
            try
            {
                return !(string.IsNullOrEmpty(str) || str == "-" || str == "0");
            }
            catch
            {
                return false;
            }
        }


        public static Decimal ToDecimal(this string strDecimal)
        {
            try
            {
                strDecimal = strDecimal.Replace("x", "").Replace("%", "");
                Decimal dc;
                decimal.TryParse(strDecimal, out dc);
                return dc;
            }
            catch
            {
                return 0;
            }
        }

        public static double ToDouble(this string strDouble)
        {
            try
            {
                strDouble = strDouble.Replace("x", "").Replace("%", "");
                double dc;
                double.TryParse(strDouble, out dc);
                return dc;
            }
            catch
            {
                return 0;
            }
        }


        public static Decimal ToInt(this string strNum)
        {
            try
            {
                int num;
                int.TryParse(strNum, out num);
                return num;
            }
            catch
            {
                return 0;
            }
        }

        public static short ToShort(this string strNum)
        {
            try
            {
                short num;
                short.TryParse(strNum, out num);
                return num;
            }
            catch
            {
                return 0;
            }
        }


        public static DateTime ToDate(this string strDate)
        {
            try
            {
                DateTime date;
                DateTime.TryParse(strDate, out date);
                return date;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string ToName(this string strName)
        {
            return strName.Replace("*", "");
        }

        public static bool IsValidSecurity(this string strSecurity)
        {
            return IsString(strSecurity) && strSecurity != "LoanX ID / CUSIP";
        }

        public static void SetPropertyValue(this object obj, string propName, object value)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, value, null);
        }

        public static object GetPropertyValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }


        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static DataTable ToTableValuedParameter<T>(this IEnumerable<T> list)
        {
            var tbl = new DataTable();
            tbl.Columns.Add("Value", typeof(T));

            foreach (var item in list)
            {
                tbl.Rows.Add(new object[] { item });
            }

            return tbl;
        }

    }
}
