
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCM.CLO.DataAccess.Extensions;
using YCM.CLO.DataAccess.Models;

using YCM.CLO.ExcelProcessor;

namespace YCM.CLO.ExcelProcessor
{
    class Program
    {

        public static DataSet ConvertExcelToDataSet(string FileName, List<string> sheetNames)
        {

            using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
            {
                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                sheetNames.ForEach(sheetName =>
                {
                    cmd.Connection = objConn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "$" + "]";
                    oleda = new OleDbDataAdapter(cmd);
                    oleda.Fill(ds, sheetName);
                });

                objConn.Close();
                return ds;
            }
        }
        static void Main(string[] args)
        {
            string filePath = @"D:\Test\CLO\York CLO Aggregate Exposure.xlsx";
            int dateId = int.Parse(DateTime.Today.ToString("yyyyMMdd"));

            var cloPositionsExcel = ConvertExcelToDataSet(filePath, new List<string>() { "York CLO Positions"});
            List<YorkCloPosition> cloYorkPositions = new List<YorkCloPosition>();


            for (int i = 0; i < cloPositionsExcel.Tables["York CLO Positions"].Rows.Count; i++)
            {
                var cloRowPosition = cloPositionsExcel.Tables["York CLO Positions"].Rows[i];
                var yorkCloPosition = new YorkCloPosition();
                yorkCloPosition.ReadData(cloRowPosition);
                cloYorkPositions.Add(yorkCloPosition);
            }

            

            //Dictionary<string, CLO4Position> clo4Positions = new Dictionary<string, CLO4Position>();
            //for (int i = 0; i < cloPositionsExcel.Tables["CLO-4"].Rows.Count; i++)
            //{
            //    var cloRowPosition = cloPositionsExcel.Tables["CLO-4"].Rows[i];
            //    var clo4Position = new CLO4Position();
            //    clo4Position.ReadData(cloRowPosition);
            //    clo4Positions[clo4Position.LoanxIdCusip] = clo4Position;
            //}

            for (int k = 0; k < cloYorkPositions.Count; k++)
            {
                try
                {
                    Console.WriteLine($"Processing {k} of {cloYorkPositions.Count}");
                    var cloPos = cloYorkPositions[k];
                    if (cloPos.LoanxIdCusip.IsValidSecurity() && cloPos.BbgId.IsString() && cloPos.Issuer.IsString() &&
                        cloPos.Facility.IsString() )
                    {
                        using (var cloContext = new CLOContext())
                        {
                            var funds = cloContext.Funds.ToList();
                            var clo1Fund = funds.First(f => f.FundCode == "CLO-1");
                            var clo2Fund = funds.First(f => f.FundCode == "CLO-2");
                            var clo3Fund = funds.First(f => f.FundCode == "CLO-3");
                            var clo4Fund = funds.First(f => f.FundCode == "CLO-4");
                            //CLO4Position clo4position = null;
                            //if (clo4Positions.ContainsKey(cloPos.LoanxIdCusip))
                            //{
                            //    clo4position = clo4Positions[cloPos.LoanxIdCusip];
                            //}

                            #region Securities 

                            Security security =
                                cloContext.Securities.FirstOrDefault(s => s.SecurityCode == cloPos.LoanxIdCusip);
                            if (security == null)
                            {
                                security = new Security();
                                security.SecurityCode = cloPos.LoanxIdCusip;
                                cloContext.Securities.Add(security);
                            }

                            security.SecurityDesc = cloPos.LoanxIdCusip;
                            security.BBGId = cloPos.BbgId;

                            Issuer issuer = cloContext.Issuers.FirstOrDefault(i => i.IssuerDesc == cloPos.Issuer);
                            if (issuer == null)
                            {
                                issuer = new Issuer();
                                issuer.IssuerDesc = cloPos.Issuer;
                                cloContext.Issuers.Add(issuer);
                            }
                            security.Issuer = issuer;

                            Facility facility =
                                cloContext.Facilities.FirstOrDefault(f => f.FacilityDesc == cloPos.Facility);
                            if (facility == null)
                            {
                                facility = new Facility();
                                facility.FacilityDesc = cloPos.Facility;
                                cloContext.Facilities.Add(facility);
                            }
                            security.Facility = facility;

                            if (cloPos.CallDate.IsDate())
                            {
                                security.CallDate = DateTime.Parse(cloPos.CallDate);
                            }

                           

                            if (cloPos.Maturity.IsDate())
                            {
                                security.MaturityDate = DateTime.Parse(cloPos.Maturity);
                            }

                            Industry snpIndustryCode =
                                cloContext.Industries.FirstOrDefault(
                                    f => f.IndustryDesc == cloPos.SPIndustryCode && f.IsSnP);
                            if (snpIndustryCode == null)
                            {
                                snpIndustryCode = new Industry();
                                snpIndustryCode.IndustryDesc = cloPos.SPIndustryCode;
                                snpIndustryCode.IsSnP = true;
                                cloContext.Industries.Add(snpIndustryCode);
                            }
                            security.Industry1 = snpIndustryCode;

                            Industry moodyIndustryCode =
                                cloContext.Industries.FirstOrDefault(
                                    f => f.IndustryDesc == cloPos.MoodySIndustryCode && f.IsMoody);
                            if (moodyIndustryCode == null)
                            {
                                moodyIndustryCode = new Industry();
                                moodyIndustryCode.IndustryDesc = cloPos.MoodySIndustryCode;
                                moodyIndustryCode.IsMoody = true;
                                cloContext.Industries.Add(moodyIndustryCode);
                            }

                            security.Industry = moodyIndustryCode;

                            

                            security.IsFloating = cloPos.FloatingFixed.ToUpper().Equals("FLOATING");

                            LienType lienType =
                                cloContext.LienTypes.FirstOrDefault(f => f.LienTypeDesc == cloPos.LienType);
                            if (lienType == null)
                            {
                                lienType = new LienType();
                                lienType.LienTypeDesc = cloPos.LienType;
                                cloContext.LienTypes.Add(lienType);
                            }
                            security.LienType = lienType;


                            //if (cloPos.TotalExposure.IsDecimal())
                            //{
                            //security.TotalExposure = cloPos.TotalExposure.ToDecimal();
                            //}


                            #endregion Securities

                            //#region Positions

                            //Country country =
                            //  cloContext.Countries.FirstOrDefault(f => f.CountryDesc == cloPos.Country);
                            //if (country == null)
                            //{
                            //    country = new Country();
                            //    country.CountryDesc = cloPos.Country;
                            //    cloContext.Countries.Add(country);
                            //}

                            //if (cloPos.Clo1Position.IsDecimal() || cloPos.PositionClo1.IsDecimal() ||
                            //    cloPos.Clo1PxPrice.IsDecimal())
                            //{
                            //    var clo1Pos = AddOrUpdateFund(cloContext, clo1Fund, security, cloPos, dateId);
                            //    if (cloPos.Clo1Position.IsDecimal())
                            //    {
                            //        clo1Pos.Exposure = decimal.Parse(cloPos.Clo1Position);
                            //    }
                            //    if (cloPos.PositionClo1.IsDecimal())
                            //    {
                            //        clo1Pos.PctExposure = cloPos.PositionClo1.ToDecimal();
                            //    }
                            //    if (cloPos.Clo1PxPrice.IsDecimal())
                            //    {
                            //        clo1Pos.PxPrice = cloPos.Clo1PxPrice.ToDecimal();
                            //    }


                            //    clo1Pos.Country = country;
                            //    clo1Pos.IsCovLite = cloPos.CovLiteYN == "1";
                            //}

                            //if (cloPos.Clo2Position.IsDecimal() || cloPos.PositionClo2.IsDecimal() ||
                            //    cloPos.Clo2PxPrice.IsDecimal())
                            //{
                            //    var clo2Pos = AddOrUpdateFund(cloContext, clo2Fund, security, cloPos, dateId);
                            //    if (cloPos.Clo2Position.IsDecimal())
                            //    {
                            //        clo2Pos.Exposure = decimal.Parse(cloPos.Clo2Position);
                            //    }
                            //    if (cloPos.PositionClo2.IsDecimal())
                            //    {
                            //        clo2Pos.PctExposure = cloPos.PositionClo2.ToDecimal();
                            //    }
                            //    if (cloPos.Clo2PxPrice.IsDecimal())
                            //    {
                            //        clo2Pos.PxPrice = cloPos.Clo2PxPrice.ToDecimal();
                            //    }


                            //    clo2Pos.Country = country;
                            //    clo2Pos.IsCovLite = cloPos.CovLiteYN == "1";
                            //}

                            //if (cloPos.Clo3Position.IsDecimal() || cloPos.PositionClo3.IsDecimal() ||
                            //    cloPos.Clo3PxPrice.IsDecimal())
                            //{
                            //    var clo3Pos = AddOrUpdateFund(cloContext, clo3Fund, security, cloPos, dateId);
                            //    if (cloPos.Clo3Position.IsDecimal())
                            //    {
                            //        clo3Pos.Exposure = decimal.Parse(cloPos.Clo3Position);
                            //    }
                            //    if (cloPos.PositionClo3.IsDecimal())
                            //    {
                            //        clo3Pos.PctExposure = cloPos.PositionClo3.ToDecimal();
                            //    }
                            //    if (cloPos.Clo3PxPrice.IsDecimal())
                            //    {
                            //        clo3Pos.PxPrice = cloPos.Clo3PxPrice.ToDecimal();
                            //    }

                            //    clo3Pos.Country = country;
                            //    clo3Pos.IsCovLite = cloPos.CovLiteYN == "1";
                            //}


                            //if (cloPos.Clo4Position.IsDecimal() || cloPos.PositionClo4.IsDecimal() ||
                            //    cloPos.Clo4PxPrice.IsDecimal())
                            //{
                            //    var clo4Pos = AddOrUpdateFund(cloContext, clo4Fund, security, cloPos, dateId);
                            //    if (cloPos.Clo4Position.IsDecimal())
                            //    {
                            //        clo4Pos.Exposure = decimal.Parse(cloPos.Clo4Position);
                            //    }
                            //    if (cloPos.PositionClo4.IsDecimal())
                            //    {
                            //        clo4Pos.PctExposure = cloPos.PositionClo4.ToDecimal();
                            //    }
                            //    if (cloPos.Clo4PxPrice.IsDecimal())
                            //    {
                            //        clo4Pos.PxPrice = cloPos.Clo4PxPrice.ToDecimal();
                            //    }

                            //    clo4Pos.Country = country;
                            //    clo4Pos.IsCovLite = cloPos.CovLiteYN == "1";
                            //}


                            //#endregion

                            //#region Market Data


                            //var marketdata_clo_1 = new MarketData() { Security = security, DateId = dateId, Fund = clo1Fund };
                            //cloContext.MarketDatas.Add(marketdata_clo_1);
                            //marketdata_clo_1.Security = security;

                            //if (cloPos.Bid.IsDecimal())
                            //{
                            //    marketdata_clo_1.Bid = cloPos.Bid.ToDecimal();
                            //}
                            //if (cloPos.Offer.IsDecimal())
                            //{
                            //    marketdata_clo_1.Offer = cloPos.Offer.ToDecimal();
                            //}
                            //if (cloPos.Spread.IsDecimal())
                            //{
                            //    marketdata_clo_1.Spread = cloPos.Spread.ToDecimal();
                            //}

                            //if (cloPos.LiborFloor.IsDecimal())
                            //{
                            //    marketdata_clo_1.LiborFloor = cloPos.LiborFloor.ToDecimal();
                            //}

                            //var moodyCfrRating = AddOrUpdateRating(cloContext, cloPos.MoodySCfr);
                            //marketdata_clo_1.Rating = moodyCfrRating;

                            //var moodyCfrRatingAdjusted = AddOrUpdateRating(cloContext, cloPos.MoodySCfrAdjusted);
                            //marketdata_clo_1.Rating1 = moodyCfrRatingAdjusted;

                            //var moodyFacilityRating = AddOrUpdateRating(cloContext, cloPos.MoodySFacilityRating);
                            //marketdata_clo_1.Rating2 = moodyFacilityRating;

                            //var moodyFacilityRatingAdjust = AddOrUpdateRating(cloContext,
                            //    cloPos.MoodySFacilityRatingAdjusted);
                            //marketdata_clo_1.Rating3 = moodyFacilityRatingAdjust;

                            //if (cloPos.MoodySRecovery.IsInt())
                            //{
                            //    marketdata_clo_1.MoodyRecovery = cloPos.MoodySRecovery.ToInt();
                            //}

                            //var snpIssuerRating = AddOrUpdateRating(cloContext, cloPos.SPIssuerRating);
                            //marketdata_clo_1.Rating6 = snpIssuerRating;

                            //var snpIssuerRatingAdjusted = AddOrUpdateRating(cloContext,
                            //    cloPos.SPIssuerRatingAdjusted);
                            //marketdata_clo_1.Rating7 = snpIssuerRatingAdjusted;

                            //var snpFacilityRating = AddOrUpdateRating(cloContext, cloPos.SPFacilityRating);
                            //marketdata_clo_1.Rating4 = snpFacilityRating;

                            //var snpFacilityRatingAdjusted = AddOrUpdateRating(cloContext,
                            //    cloPos.SPFacilityRatingAdjusted);
                            //marketdata_clo_1.Rating5 = snpFacilityRatingAdjusted;

                            //var snpRecoveryRating = AddOrUpdateRating(cloContext, cloPos.SPRecoveryRating);
                            //marketdata_clo_1.Rating8 = snpRecoveryRating;

                            //marketdata_clo_1.MoodyOutlook = cloPos.MoodySOutlook;
                            //marketdata_clo_1.MoodyWatch = cloPos.MoodySWatch;
                            //marketdata_clo_1.SnPWatch = cloPos.SPWatch;
                            //if (cloPos.NextReportingDate.IsDate())
                            //{
                            //    marketdata_clo_1.NextReportingDate = cloPos.NextReportingDate.ToDate();
                            //}

                            //if (cloPos.FiscalYearEnd.IsDate())
                            //{
                            //    marketdata_clo_1.FiscalYearEndDate = cloPos.FiscalYearEnd.ToDate();
                            //}

                            //if (!string.IsNullOrEmpty(cloPos.AgentBank))
                            //{
                            //    marketdata_clo_1.AgentBank = cloPos.AgentBank;
                            //}

                            //var marketdata_clo_2 = new MarketData() { Security = security, DateId = dateId, Fund = clo2Fund };
                            //cloContext.MarketDatas.Add(marketdata_clo_2);
                            //marketdata_clo_2.Security = security;

                            //if (cloPos.Bid.IsDecimal())
                            //{
                            //    marketdata_clo_2.Bid = cloPos.Bid.ToDecimal();
                            //}
                            //if (cloPos.Offer.IsDecimal())
                            //{
                            //    marketdata_clo_2.Offer = cloPos.Offer.ToDecimal();
                            //}
                            //if (cloPos.Spread.IsDecimal())
                            //{
                            //    marketdata_clo_2.Spread = cloPos.Spread.ToDecimal();
                            //}

                            //if (cloPos.LiborFloor.IsDecimal())
                            //{
                            //    marketdata_clo_2.LiborFloor = cloPos.LiborFloor.ToDecimal();
                            //}


                            //marketdata_clo_2.Rating = moodyCfrRating;


                            //marketdata_clo_2.Rating1 = moodyCfrRatingAdjusted;


                            //marketdata_clo_2.Rating2 = moodyFacilityRating;


                            //marketdata_clo_2.Rating3 = moodyFacilityRatingAdjust;

                            //if (cloPos.MoodySRecovery.IsInt())
                            //{
                            //    marketdata_clo_2.MoodyRecovery = cloPos.MoodySRecovery.ToInt();
                            //}


                            //marketdata_clo_2.Rating6 = snpIssuerRating;


                            //marketdata_clo_2.Rating7 = snpIssuerRatingAdjusted;


                            //marketdata_clo_2.Rating4 = snpFacilityRating;


                            //marketdata_clo_2.Rating5 = snpFacilityRatingAdjusted;


                            //marketdata_clo_2.Rating8 = snpRecoveryRating;

                            //marketdata_clo_2.MoodyOutlook = cloPos.MoodySOutlook;
                            //marketdata_clo_2.MoodyWatch = cloPos.MoodySWatch;
                            //marketdata_clo_2.SnPWatch = cloPos.SPWatch;
                            //if (cloPos.NextReportingDate.IsDate())
                            //{
                            //    marketdata_clo_2.NextReportingDate = cloPos.NextReportingDate.ToDate();
                            //}

                            //if (cloPos.FiscalYearEnd.IsDate())
                            //{
                            //    marketdata_clo_2.FiscalYearEndDate = cloPos.FiscalYearEnd.ToDate();
                            //}

                            //if (!string.IsNullOrEmpty(cloPos.AgentBank))
                            //{
                            //    marketdata_clo_2.AgentBank = cloPos.AgentBank;
                            //}

                            //var marketdata_clo_3 = new MarketData() { Security = security, DateId = dateId, Fund = clo3Fund };
                            //cloContext.MarketDatas.Add(marketdata_clo_3);
                            //marketdata_clo_3.Security = security;

                            //if (cloPos.Bid.IsDecimal())
                            //{
                            //    marketdata_clo_3.Bid = cloPos.Bid.ToDecimal();
                            //}
                            //if (cloPos.Offer.IsDecimal())
                            //{
                            //    marketdata_clo_3.Offer = cloPos.Offer.ToDecimal();
                            //}
                            //if (cloPos.Spread.IsDecimal())
                            //{
                            //    marketdata_clo_3.Spread = cloPos.Spread.ToDecimal();
                            //}

                            //if (cloPos.LiborFloor.IsDecimal())
                            //{
                            //    marketdata_clo_3.LiborFloor = cloPos.LiborFloor.ToDecimal();
                            //}


                            //marketdata_clo_3.Rating = moodyCfrRating;


                            //marketdata_clo_3.Rating1 = moodyCfrRatingAdjusted;


                            //marketdata_clo_3.Rating2 = moodyFacilityRating;


                            //marketdata_clo_3.Rating3 = moodyFacilityRatingAdjust;

                            //if (cloPos.MoodySRecovery.IsInt())
                            //{
                            //    marketdata_clo_3.MoodyRecovery = cloPos.MoodySRecovery.ToInt();
                            //}


                            //marketdata_clo_3.Rating6 = snpIssuerRating;


                            //marketdata_clo_3.Rating7 = snpIssuerRatingAdjusted;


                            //marketdata_clo_3.Rating4 = snpFacilityRating;


                            //marketdata_clo_3.Rating5 = snpFacilityRatingAdjusted;


                            //marketdata_clo_3.Rating8 = snpRecoveryRating;

                            //marketdata_clo_3.MoodyOutlook = cloPos.MoodySOutlook;
                            //marketdata_clo_3.MoodyWatch = cloPos.MoodySWatch;
                            //marketdata_clo_3.SnPWatch = cloPos.SPWatch;
                            //if (cloPos.NextReportingDate.IsDate())
                            //{
                            //    marketdata_clo_3.NextReportingDate = cloPos.NextReportingDate.ToDate();
                            //}

                            //if (cloPos.FiscalYearEnd.IsDate())
                            //{
                            //    marketdata_clo_3.FiscalYearEndDate = cloPos.FiscalYearEnd.ToDate();
                            //}

                            //if (!string.IsNullOrEmpty(cloPos.AgentBank))
                            //{
                            //    marketdata_clo_3.AgentBank = cloPos.AgentBank;
                            //}

                            //var marketdata_clo_4 = new MarketData() { Security = security, DateId = dateId, Fund = clo4Fund };
                            //cloContext.MarketDatas.Add(marketdata_clo_4);
                            //marketdata_clo_4.Security = security;

                            //if (cloPos.Bid.IsDecimal())
                            //{
                            //    marketdata_clo_4.Bid = cloPos.Bid.ToDecimal();
                            //}
                            //if (cloPos.Offer.IsDecimal())
                            //{
                            //    marketdata_clo_4.Offer = cloPos.Offer.ToDecimal();
                            //}
                            //if (cloPos.Spread.IsDecimal())
                            //{
                            //    marketdata_clo_4.Spread = cloPos.Spread.ToDecimal();
                            //}

                            //if (cloPos.LiborFloor.IsDecimal())
                            //{
                            //    marketdata_clo_4.LiborFloor = cloPos.LiborFloor.ToDecimal();
                            //}


                            //marketdata_clo_4.Rating = moodyCfrRating;


                            //marketdata_clo_4.Rating1 = moodyCfrRatingAdjusted;


                            //marketdata_clo_4.Rating2 = moodyFacilityRating;


                            //marketdata_clo_4.Rating3 = moodyFacilityRatingAdjust;

                            //if (cloPos.MoodySRecovery.IsInt())
                            //{
                            //    marketdata_clo_4.MoodyRecovery = cloPos.MoodySRecovery.ToInt();
                            //}


                            //marketdata_clo_4.Rating6 = snpIssuerRating;


                            //marketdata_clo_4.Rating7 = snpIssuerRatingAdjusted;


                            //marketdata_clo_4.Rating4 = snpFacilityRating;


                            //marketdata_clo_4.Rating5 = snpFacilityRatingAdjusted;


                            //marketdata_clo_4.Rating8 = snpRecoveryRating;

                            //marketdata_clo_4.MoodyOutlook = cloPos.MoodySOutlook;
                            //marketdata_clo_4.MoodyWatch = cloPos.MoodySWatch;
                            //marketdata_clo_4.SnPWatch = cloPos.SPWatch;
                            //if (cloPos.NextReportingDate.IsDate())
                            //{
                            //    marketdata_clo_4.NextReportingDate = cloPos.NextReportingDate.ToDate();
                            //}

                            //if (cloPos.FiscalYearEnd.IsDate())
                            //{
                            //    marketdata_clo_4.FiscalYearEndDate = cloPos.FiscalYearEnd.ToDate();
                            //}

                            //if (!string.IsNullOrEmpty(cloPos.AgentBank))
                            //{
                            //    marketdata_clo_4.AgentBank = cloPos.AgentBank;
                            //}

                            //#endregion

                            #region Analysts

                            var analyst =
                                cloContext.AnalystResearches.FirstOrDefault(a => a.IssuerId == security.IssuerId);
                            if (analyst == null)
                            {
                                analyst = new AnalystResearch() { Issuer = security.Issuer };
                                cloContext.AnalystResearches.Add(analyst);
                            }

                            if (cloPos.CloAnalyst.IsString())
                            {
                                var cloAnalyst = AddOrUpdateUser(cloContext, cloPos.CloAnalyst);
                                analyst.User = cloAnalyst;
                                cloAnalyst.IsCLOAnalyst = true;
                            }

                            if (cloPos.HfAnalyst.IsString())
                            {
                                var hfAnalyst = AddOrUpdateUser(cloContext, cloPos.HfAnalyst);
                                analyst.User1 = hfAnalyst;
                                hfAnalyst.IsHFAnalyst = true;
                            }

                            if (cloPos.CreditScore.IsDecimal())
                            {
                                analyst.CreditScore = cloPos.CreditScore.ToDecimal();
                            }

                            if (cloPos.AsOfDate.IsDate())
                            {
                                analyst.AsOfDate = cloPos.AsOfDate.ToDate();
                            }

                            analyst.CreditScore = cloPos.CreditScore.IsInt() ? cloPos.CreditScore.ToShort() : (short)0;

                            if (cloPos.OneLLeverage.IsDecimal())
                            {
                                analyst.OneLLeverage = cloPos.OneLLeverage.ToDecimal();
                            }

                            if (cloPos.TotlLeverage.IsDecimal())
                            {
                                analyst.TotalLeverage = cloPos.TotlLeverage.ToDecimal();
                            }

                            if (cloPos.EvMultiple.IsDecimal())
                            {
                                analyst.EVMultiple = cloPos.EvMultiple.ToDecimal();
                            }

                            if (cloPos.LtmRevenues.IsDecimal())
                            {
                                analyst.LTMRevenues = cloPos.LtmRevenues.ToDecimal();
                            }

                            if (cloPos.LtmEbitda.IsDecimal())
                            {
                                analyst.LTMEBITDA = cloPos.LtmEbitda.ToDecimal();
                            }

                            if (cloPos.Fcf.IsDecimal())
                            {
                                analyst.FCF = cloPos.Fcf.ToDecimal();
                            }

                            if (cloPos.Comments.IsString())
                            {
                                analyst.Comments = cloPos.Comments;
                            }
                            if (cloPos.Comments.IsString())
                            {
                                analyst.BusinessDescription = cloPos.Comments;
                            }

                            #endregion


                            cloContext.SaveChanges();
                        }
                    }
                }

                catch
                    (DbEntityValidationException
                        exc)
                {
                    // just to ease debugging
                    foreach (var error in exc.EntityValidationErrors)
                    {
                        foreach (var errorMsg in error.ValidationErrors)
                        {
                            // logging service based on NLog
                            //Logger.Log(LogLevel.Error, $"Error trying to save EF changes - {errorMsg.ErrorMessage}");
                            Console.WriteLine($"Error trying to save EF changes - {errorMsg.ErrorMessage}");
                        }
                    }

                    throw;
                }
                catch
                    (DbUpdateException
                        e)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(
                        $"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                    foreach (var eve in e.Entries)
                    {
                        sb.AppendLine(
                            $"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    }

                    //Logger.Log(LogLevel.Error, e, sb.ToString());
                    Console.WriteLine(sb.ToString());
                    throw;
                }
            }

            Console.ReadLine();
        }


        private static Position AddOrUpdateFund(CLOContext cloContext, Fund fund, Security security, YorkCloPosition cloPos, int dateId)
        {
            
            var clopos =
                cloContext.Positions.FirstOrDefault(p => p.FundId == fund.FundId && p.SecurityId == security.SecurityId && p.DateId == dateId);

            if (clopos == null)
            {
                clopos = new Position() { Security = security, Fund = fund,DateId = dateId};
                cloContext.Positions.Add(clopos);
            }

            return clopos;
        }

        private static Rating AddOrUpdateRating(CLOContext cloContext, string ratingDesc)
        {
            var rating = cloContext.Ratings.FirstOrDefault(r => r.RatingDesc == ratingDesc);
            if (rating == null)
            {
                rating = new Rating();
                rating.RatingDesc = ratingDesc;
                cloContext.Ratings.Add(rating);
            }

            return rating;
        }

        private static User AddOrUpdateUser(CLOContext cloContext, string userFullName)
        {
            userFullName = userFullName.ToName();
            var user = cloContext.Users.FirstOrDefault(r => r.FullName == userFullName);
            if (user == null)
            {
                user = new User() { FullName = userFullName.ToName() };
                cloContext.Users.Add(user);
            }

            return user;
        }

    }
}
