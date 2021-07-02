using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.ExcelProcessor
{
    public class CLO4Position
    {
        public string LoanxIdCusip { get; set; }

        public string BbgId { get; set; }

        public string Issuer { get; set; }

        public string Facility { get; set; }

        public string Warehouse { get; set; }

        public string PurchasePrice { get; set; }

        public string SecondaryPurchase { get; set; }

        public string CallProtection { get; set; }

        public string BidPrice { get; set; }

        public string OfferPrice { get; set; }

        public string OfferWJuice { get; set; }

        public string TotalPar { get; set; }

        public string TotalPurchasePrice { get; set; }

        public string SpreadFloating { get; set; }

        public string LiborFloor { get; set; }

        public string Coupon { get; set; }

        public string AdjCoupon { get; set; }

        public string Yield { get; set; }

        public string CappedYield { get; set; }

        public string SuggestedYield { get; set; }

        public string BetterWorse { get; set; }

        public string MdyCfr { get; set; }

        public string MdyCfrAdj { get; set; }

        public string MoodySFacilityRating { get; set; }

        public string Warf { get; set; }

        public string RecoveryAdjWarf { get; set; }

        public string MoodySRecovery { get; set; }

        public string SPIssuerRating { get; set; }

        public string SPIssuerRatingAdjusted { get; set; }

        public string SPFacilityRating { get; set; }

        public string SPRecoveryRating { get; set; }

        public string Country { get; set; }

        public string Maturity { get; set; }

        public string Life { get; set; }

        public string SPIndustryCode { get; set; }

        public string MoodySIndustryCode { get; set; }

        public string MoodySOutlook { get; set; }

        public string MoodySWatch { get; set; }

        public string SPWatch { get; set; }

        public string CovLiteYN { get; set; }

        public string FloatingFixed { get; set; }

        public string LienType { get; set; }

        public string Analyst { get; set; }

        public string SecondaryAnalyst { get; set; }

        public string OilGasExposure { get; set; }

        public string NextReportingDate { get; set; }

        public string FiscalYearEnd { get; set; }

        public string AgentBank { get; set; }

        public string BidPrice2 { get; set; }

        public string OfferPrice2 { get; set; }

        public string Column1 { get; set; }

        public string Description { get; set; }
        public CLO4Position()
        {
            LoanxIdCusip = "";
            BbgId = "";
            Issuer = "";
            Facility = "";
            Warehouse = "";
            PurchasePrice = "";
            SecondaryPurchase = "";
            CallProtection = "";
            BidPrice = "";
            OfferPrice = "";
            OfferWJuice = "";
            TotalPar = "";
            TotalPurchasePrice = "";
            SpreadFloating = "";
            LiborFloor = "";
            Coupon = "";
            AdjCoupon = "";
            Yield = "";
            CappedYield = "";
            SuggestedYield = "";
            BetterWorse = "";
            MdyCfr = "";
            MdyCfrAdj = "";
            MoodySFacilityRating = "";
            Warf = "";
            RecoveryAdjWarf = "";
            MoodySRecovery = "";
            SPIssuerRating = "";
            SPIssuerRatingAdjusted = "";
            SPFacilityRating = "";
            SPRecoveryRating = "";
            Country = "";
            Maturity = "";
            Life = "";
            SPIndustryCode = "";
            MoodySIndustryCode = "";
            MoodySOutlook = "";
            MoodySWatch = "";
            SPWatch = "";
            CovLiteYN = "";
            FloatingFixed = "";
            LienType = "";
            Analyst = "";
            SecondaryAnalyst = "";
            OilGasExposure = "";
            NextReportingDate = "";
            FiscalYearEnd = "";
            AgentBank = "";
            BidPrice2 = "";
            OfferPrice2 = "";
            Column1 = "";
            Description = "";
        }

        public void ReadData(DataRow dataRow)
        {
            
            int propCounter = 1;
            LoanxIdCusip = dataRow[propCounter++].ToString();
            BbgId = dataRow[propCounter++].ToString();
            Issuer = dataRow[propCounter++].ToString();
            Facility = dataRow[propCounter++].ToString();
            Warehouse = dataRow[propCounter++].ToString();
            PurchasePrice = dataRow[propCounter++].ToString();
            SecondaryPurchase = dataRow[propCounter++].ToString();
            CallProtection = dataRow[propCounter++].ToString();
            BidPrice = dataRow[propCounter++].ToString();
            OfferPrice = dataRow[propCounter++].ToString();
            OfferWJuice = dataRow[propCounter++].ToString();
            TotalPar = dataRow[propCounter++].ToString();
            TotalPurchasePrice = dataRow[propCounter++].ToString();
            SpreadFloating = dataRow[propCounter++].ToString();
            LiborFloor = dataRow[propCounter++].ToString();
            Coupon = dataRow[propCounter++].ToString();
            AdjCoupon = dataRow[propCounter++].ToString();
            Yield = dataRow[propCounter++].ToString();
            CappedYield = dataRow[propCounter++].ToString();
            SuggestedYield = dataRow[propCounter++].ToString();
            BetterWorse = dataRow[propCounter++].ToString();
            MdyCfr = dataRow[propCounter++].ToString();
            MdyCfrAdj = dataRow[propCounter++].ToString();
            MoodySFacilityRating = dataRow[propCounter++].ToString();
            Warf = dataRow[propCounter++].ToString();
            RecoveryAdjWarf = dataRow[propCounter++].ToString();
            MoodySRecovery = dataRow[propCounter++].ToString();
            SPIssuerRating = dataRow[propCounter++].ToString();
            SPIssuerRatingAdjusted = dataRow[propCounter++].ToString();
            SPFacilityRating = dataRow[propCounter++].ToString();
            SPRecoveryRating = dataRow[propCounter++].ToString();
            Country = dataRow[propCounter++].ToString();
            Maturity = dataRow[propCounter++].ToString();
            Life = dataRow[propCounter++].ToString();
            SPIndustryCode = dataRow[propCounter++].ToString();
            MoodySIndustryCode = dataRow[propCounter++].ToString();
            MoodySOutlook = dataRow[propCounter++].ToString();
            MoodySWatch = dataRow[propCounter++].ToString();
            SPWatch = dataRow[propCounter++].ToString();
            CovLiteYN = dataRow[propCounter++].ToString();
            FloatingFixed = dataRow[propCounter++].ToString();
            LienType = dataRow[propCounter++].ToString();
            Analyst = dataRow[propCounter++].ToString();
            SecondaryAnalyst = dataRow[propCounter++].ToString();
            OilGasExposure = dataRow[propCounter++].ToString();
            NextReportingDate = dataRow[propCounter++].ToString();
            FiscalYearEnd = dataRow[propCounter++].ToString();
            AgentBank = dataRow[propCounter++].ToString();
            BidPrice2 = dataRow[propCounter++].ToString();
            OfferPrice2 = dataRow[propCounter++].ToString();
            Column1 = dataRow[propCounter++].ToString();
            Description = dataRow[propCounter++].ToString();
        }
    }
}
