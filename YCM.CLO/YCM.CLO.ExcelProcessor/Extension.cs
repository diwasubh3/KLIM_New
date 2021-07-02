//using System;
//using System.Collections.Generic;
//using System.Data.SqlTypes;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace YCM.CLO.ExcelProcessor
//{
//    public static class Extension
//    {
//        public static bool IsDate(this string strDate)
//        {
//            try
//            {
//                System.DateTime dt;

//                bool isvalid = DateTime.TryParse(strDate,out dt);

//                isvalid = isvalid && (dt > DateTime.MinValue && dt < DateTime.MaxValue) 
//                    && (dt > (DateTime)SqlDateTime.MinValue && dt < (DateTime)SqlDateTime.MaxValue);

//                return isvalid;
              
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public static bool IsDecimal(this string strDecimal)
//        {
//            try
//            {
//                strDecimal = strDecimal.Replace("x", "").Replace("%", "");
//                Decimal dc; 
//                    return decimal.TryParse(strDecimal, out dc);
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public static bool IsInt(this string strNum)
//        {
//            try
//            {
//                int num;
//                return int.TryParse(strNum, out num);
//            }
//            catch
//            {
//                return false;
//            }
//        }


//        public static bool IsString(this string str)
//        {
//            try
//            {
//                return !(string.IsNullOrEmpty(str) || str == "-" || str == "0");
//            }
//            catch
//            {
//                return false;
//            }
//        }


//        public static Decimal ToDecimal(this string strDecimal)
//        {
//            try
//            {
//                strDecimal = strDecimal.Replace("x", "").Replace("%", "");
//                Decimal dc;
//                decimal.TryParse(strDecimal, out dc);
//                return dc;
//            }
//            catch
//            {
//                return 0;
//            }
//        }


//        public static Decimal ToInt(this string strNum)
//        {
//            try
//            {
//                int num;
//                int.TryParse(strNum, out num);
//                return num;
//            }
//            catch
//            {
//                return 0;
//            }
//        }

//        public static short ToShort(this string strNum)
//        {
//            try
//            {
//                short num;
//                short.TryParse(strNum, out num);
//                return num;
//            }
//            catch
//            {
//                return 0;
//            }
//        }


//        public static DateTime ToDate(this string strDate)
//        {
//            try
//            {
//                DateTime date;
//                DateTime.TryParse(strDate, out date);
//                return date;
//            }
//            catch
//            {
//                return DateTime.MinValue;
//            }
//        }

//        public static string ToName(this string strName)
//        {
//            return strName.Replace("*", "");
//        }

//        public static bool IsValidSecurity(this string strSecurity)
//        {
//            return IsString(strSecurity) && strSecurity != "LoanX ID / CUSIP";
//        }

        
//    }
//}
