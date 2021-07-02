using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.ExcelProcessor
{
    public class YorkCloPosition
    {
        public string LoanxIdCusip { get; set; }
        public string BbgId { get; set; }
        public string Issuer { get; set; }

        public string Facility { get; set; }

        public string TotalExposure { get; set; }

        public string Clo1Position { get; set; }

        public string PositionClo1 { get; set; }

        public string Clo1PxPrice { get; set; }

        public string Clo2Position { get; set; }

        public string PositionClo2 { get; set; }

        public string Clo2PxPrice { get; set; }

        public string Clo3Position { get; set; }

        public string PositionClo3 { get; set; }

        public string Clo3PxPrice { get; set; }

        public string Clo4Position { get; set; }

        public string PositionClo4 { get; set; }

        public string Clo4PxPrice { get; set; }

        public string CallDate { get; set; }

        public string Bid { get; set; }

        public string Offer { get; set; }

        public string Yield { get; set; }

        public string CappedYield { get; set; }

        public string TargetYield { get; set; }

        public string BetterWorse { get; set; }

        public string Spread { get; set; }

        public string LiborFloor { get; set; }

        public string TotalCoupon { get; set; }

        public string MoodySCfr { get; set; }

        public string MoodySCfrAdjusted { get; set; }

        public string CallSchedule { get; set; }

        public string MoodySFacilityRating { get; set; }

        public string MoodySFacilityRatingAdjusted { get; set; }

        public string Warf { get; set; }

        public string WarfRecoveryClo3 { get; set; }

        public string MoodySRecovery { get; set; }

        public string SPIssuerRating { get; set; }

        public string SPIssuerRatingAdjusted { get; set; }

        public string SPFacilityRating { get; set; }

        public string SPFacilityRatingAdjusted { get; set; }

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

        public string CloAnalyst { get; set; }

        public string HfAnalyst { get; set; }

        public string OilGasExposure { get; set; }

        public string WatchList { get; set; }

        public string Defaulted { get; set; }

        public string NextReportingDate { get; set; }

        public string FiscalYearEnd { get; set; }

        public string AgentBank { get; set; }

        public string BidPrice { get; set; }

        public string OfferPrice { get; set; }

        public string Column1 { get; set; }

        

        public string AsOfDate { get; set; }

        public string OneLLeverage { get; set; }

        public string TotlLeverage { get; set; }

        public string EvMultiple { get; set; }

        public string LtmRevenues { get; set; }

        public string LtmEbitda { get; set; }

        public string Fcf { get; set; }

        public string CreditScore { get; set; }

        public string WasContribution { get; set; }


        public string Comments { get; set; }


        public YorkCloPosition()
        {
            LoanxIdCusip = "";
            BbgId = "";
            Issuer = "";
            Facility = "";
            TotalExposure = "";
            Clo1Position = "";
            PositionClo1 = "";
            Clo1PxPrice = "";
            Clo2Position = "";
            PositionClo2 = "";
            Clo2PxPrice = "";
            Clo3Position = "";
            PositionClo3 = "";
            Clo3PxPrice = "";
            Clo4Position = "";
            PositionClo4 = "";
            Clo4PxPrice = "";
            CallDate = "";
            Bid = "";
            Offer = "";
            Yield = "";
            CappedYield = "";
            TargetYield = "";
            BetterWorse = "";
            Spread = "";
            LiborFloor = "";
            TotalCoupon = "";
            MoodySCfr = "";
            MoodySCfrAdjusted = "";
            CallSchedule = "";
            MoodySFacilityRating = "";
            MoodySFacilityRatingAdjusted = "";
            Warf = "";
            WarfRecoveryClo3 = "";
            MoodySRecovery = "";
            SPIssuerRating = "";
            SPIssuerRatingAdjusted = "";
            SPFacilityRating = "";
            SPFacilityRatingAdjusted = "";
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
            CloAnalyst = "";
            HfAnalyst = "";
            OilGasExposure = "";
            WatchList = "";
            Defaulted = "";
            NextReportingDate = "";
            FiscalYearEnd = "";
            AgentBank = "";
            BidPrice = "";
            OfferPrice = "";
            Column1 = "";
            
            AsOfDate = "";
            OneLLeverage = "";
            TotlLeverage = "";
            EvMultiple = "";
            LtmRevenues = "";
            LtmEbitda = "";
            Fcf = "";
            CreditScore = "";
            WasContribution = "";
            Comments = "";
        }

        public void ReadData(DataRow dataRow)
        {

            int propCounter = 1;
            LoanxIdCusip = dataRow[propCounter++].ToString().ToString();
            BbgId = dataRow[propCounter++].ToString();
            Issuer = dataRow[propCounter++].ToString();
            Facility = dataRow[propCounter++].ToString();
            TotalExposure = dataRow[propCounter++].ToString();
            Clo1Position = dataRow[propCounter++].ToString();
            PositionClo1 = dataRow[propCounter++].ToString();
            Clo1PxPrice = dataRow[propCounter++].ToString();
            Clo2Position = dataRow[propCounter++].ToString();
            PositionClo2 = dataRow[propCounter++].ToString();
            Clo2PxPrice = dataRow[propCounter++].ToString();
            Clo3Position = dataRow[propCounter++].ToString();
            PositionClo3 = dataRow[propCounter++].ToString();
            Clo3PxPrice = dataRow[propCounter++].ToString();
            Clo4Position = dataRow[propCounter++].ToString();
            PositionClo4 = dataRow[propCounter++].ToString();
            Clo4PxPrice = dataRow[propCounter++].ToString();
            CallDate = dataRow[propCounter++].ToString();
            Bid = dataRow[propCounter++].ToString();
            Offer = dataRow[propCounter++].ToString();
            Yield = dataRow[propCounter++].ToString();
            CappedYield = dataRow[propCounter++].ToString();
            TargetYield = dataRow[propCounter++].ToString();
            BetterWorse = dataRow[propCounter++].ToString();
            Spread = dataRow[propCounter++].ToString();
            LiborFloor = dataRow[propCounter++].ToString();
            TotalCoupon = dataRow[propCounter++].ToString();
            MoodySCfr = dataRow[propCounter++].ToString();
            MoodySCfrAdjusted = dataRow[propCounter++].ToString();
            CallSchedule = dataRow[propCounter++].ToString();
            MoodySFacilityRating = dataRow[propCounter++].ToString();
            MoodySFacilityRatingAdjusted = dataRow[propCounter++].ToString();
            Warf = dataRow[propCounter++].ToString();
            WarfRecoveryClo3 = dataRow[propCounter++].ToString();
            MoodySRecovery = dataRow[propCounter++].ToString();
            SPIssuerRating = dataRow[propCounter++].ToString();
            SPIssuerRatingAdjusted = dataRow[propCounter++].ToString();
            SPFacilityRating = dataRow[propCounter++].ToString();
            SPFacilityRatingAdjusted = dataRow[propCounter++].ToString();
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
            CloAnalyst = dataRow[propCounter++].ToString();
            HfAnalyst = dataRow[propCounter++].ToString();
            OilGasExposure = dataRow[propCounter++].ToString();
            WatchList = dataRow[propCounter++].ToString();
            Defaulted = dataRow[propCounter++].ToString();
            NextReportingDate = dataRow[propCounter++].ToString();
            FiscalYearEnd = dataRow[propCounter++].ToString();
            AgentBank = dataRow[propCounter++].ToString();
            BidPrice = dataRow[propCounter++].ToString();
            OfferPrice = dataRow[propCounter++].ToString();
            Column1 = dataRow[propCounter++].ToString();
            propCounter++;
            AsOfDate = dataRow[propCounter++].ToString();
            OneLLeverage = dataRow[propCounter++].ToString();
            TotlLeverage = dataRow[propCounter++].ToString();
            EvMultiple = dataRow[propCounter++].ToString();
            LtmRevenues = dataRow[propCounter++].ToString();
            LtmEbitda = dataRow[propCounter++].ToString();
            Fcf = dataRow[propCounter++].ToString();
            CreditScore = dataRow[propCounter++].ToString();
            WasContribution = dataRow[propCounter++].ToString();
            Comments = dataRow[propCounter].ToString();
        }
    }
}
