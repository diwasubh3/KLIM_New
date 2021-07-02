using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Extensions;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects.ExcelHelper;
using YCM.CLO.Web.Objects;
using YCM.CLO.DataAccess;

namespace YCM.CLO.Web.Controllers
{
    public class BidOfferDataController : Controller
    {
        // GET: BidOfferUpload
        readonly IRepository _repository;

        //public BidOfferDataController() : this(new Repository())
        //{

        //}
        public BidOfferDataController(IRepository repository)
        {
            _repository = repository;
        }


        public ActionResult DownloadTemplate()
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=BidOfferUpload.xlsx");
            var slExcelData = new SlExcelData();
            slExcelData.Headers.Add("Loan");
            slExcelData.Headers.Add("Bid");
            slExcelData.Headers.Add("Offer");

            _repository.GetSecurityCodesForBidOfferDownload().ForEach(s =>
            {
               slExcelData.DataRows.Add(new List<string>()
               {
                   s,
                   "=" + "Markitrtd(\"" + s + "\",\"Bid\",\"Markit Loans New York\",\"MLXIP\")",
                   "=" + "Markitrtd(\"" + s + "\",\"Ask\",\"Markit Loans New York\",\"MLXIP\")"
               }); 
            });

            var template = new SlExcelWriter().GenerateExcel(new List<SlExcelData>
            {
                slExcelData
            });
            return new FileContentResult(template,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpPost]
        public JsonNetResult UploadFile(HttpPostedFileBase uploadedfile)
        {
            try
            {
                SlExcelData data = new SlExcelReader().ReadExcel(uploadedfile);

                if (data.Headers.Count == 0)
                    throw new Exception("Make sure file is closed before importing.");

                if (data.Headers.Count < 3)
                    throw new Exception("Missing Columns, check your file");

                var securities = Mapper.Map<IEnumerable<Security>, IEnumerable<SecurityDto>>(_repository.GetSecurities(data.DataRows.Select(r => r[0].ToUpper()).ToArray()).ToList()).ToDictionary(s => s.SecurityCode, s => s);
                List<PricingDto> pricingDtos = new List<PricingDto>();
                var uploadedOn = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                var person = _repository.GetPerson(User.Identity.Name);
                var dateId = int.Parse(DateTime.Today.AddDays(-1).ToString("yyyyMMdd"));

                data.DataRows.ForEach(dr =>
                {
                    var loan = dr[0].ToUpper();

                    var hasValidBid = dr.Count > 1 && dr[1].IsDecimal();
                    var hasValidOffer = dr.Count > 2 && dr[2].IsDecimal();

                    var bid = hasValidBid ? Math.Round(dr[1].ToDecimal(),2) : 0;
                    var offer = hasValidOffer ? Math.Round(dr[2].ToDecimal(),2) : 0;

                    if (securities.ContainsKey(loan) && (hasValidBid || hasValidOffer))
                    {
                        var security = securities[loan];
                        var pricingDto = new PricingDto()
                        {
                            CreatedBy = User.Identity.Name,
                            CreatedOn = uploadedOn,
                            CreatedByFullName = person.FullName,
                            SecurityId = security.SecurityId,
                            SecurityCode = security.SecurityCode,
                            Issuer = security.Issuer,
                            Facility = security.Facility,
                            Bid = bid,
                            Offer = offer,
                            DateId = dateId
                        };
                        pricingDtos.Add(pricingDto);
                    }

                    pricingDtos = pricingDtos.GroupBy(p => p.SecurityId).Select(p => p.ToList().First()).ToList();
                });

                return new JsonNetResult
                {
                    Data = new
                    {
                        FileUploaded = true,
                        Data = pricingDtos.ToArray(),
                        Message = person.FullName
                        + ", you have successfully uploaded _COUNT_ loans and their corresponding bids/offers. "
                        + "if the data in the list looks correct, click the \"Save to Database\" action link above."
                    },
                    ContentType = "text/html"
                };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception has occurred during Price upload");
                return new JsonNetResult
                {
                    Data = new { FileUploaded = false, Error = exception.Message },
                    ContentType = "text/html"
                };
            }
        }

        [HttpPost]
        public JsonNetResult SavePrices(Pricing[] prices)
        {
            try
            {


                int dateId = prices[0].DateId;
                if (_repository.ClearExistingPrices(dateId, prices.Select(p => p.SecurityId).ToArray()))
                {
                    
                    var person = _repository.GetPerson(User.Identity.Name);
                    var status = _repository.SavePrices(prices);
                    if (status)
                    {
                        CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
                        return new JsonNetResult()
                        {
                            Data = new
                            {
                                Status = calculationEngineClient.Calculate(dateId, User.Identity.Name),
                                Message = person.FullName
                                              + ", you have successfully saved <b><u>" + prices.Count() +
                                              "</u></b> loans and their corresponding bids/offers to the Database."
                            }
                        };
                    }
                    else
                    {
                        return new JsonNetResult()
                        {
                            Data = new
                            {
                                Status = false,
                                Message = "Some error occured while saving prices, please contact Developers Helpdesk."
                            }
                        };
                    }
                }
                else
                {
                    return new JsonNetResult()
                    {
                        Data = new
                        {
                            Status = false,
                            Message = "Some error occured while clearing existing prices."
                        }
                    };
                }
            }
            catch (Exception exception)
            {
                return new JsonNetResult()
                {
                    Data = new
                    {
                        Status = false,
                        Message = exception.ToString()
                    }
                };
            }
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}