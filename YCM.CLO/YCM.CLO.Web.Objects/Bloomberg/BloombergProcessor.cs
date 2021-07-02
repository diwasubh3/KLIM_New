using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using AutoMapper;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;

namespace YCM.CLO.Web.Objects
{
	public class BloombergProcessor
    {

        IList<string> _securities;
        IList<string> _fields;

        private readonly ConcurrentDictionary<string, SecurityReponse> _responses;

        public BloombergProcessor()
        {
            _fields = new List<string>();
            _fields.Add("INDUSTRY_SECTOR_NUM");
            _fields.Add("PX_ASK");
            _fields.Add("PX_BID");
            _fields.Add("NXT_CALL_DT");
            _fields.Add("MATURITY");
            _fields.Add("ISSUER");
            _fields.Add("LOAN_TYP");
            _fields.Add("LN_TRANCHE_LETTER");
            _fields.Add("FIRST_LIEN_INDICATOR");
            _fields.Add("SECOND_LIEN_INDICATOR");
            _fields.Add("THIRD_LIEN_INDICATOR");
            _fields.Add("LN_SPREAD_CLOSE");
            _fields.Add("INDUSTRY_SECTOR");
            _fields.Add("INDUSTRY_GROUP");
            _fields.Add("INDUSTRY_SUBGROUP");
        }

        public TempSecurityDto Process(string securityCode, IRepository repository,string user)
        {
            TempSecurityDto tempSecurityDto = new TempSecurityDto();
            _securities = new List<string>();

            tempSecurityDto.SecurityCode = securityCode.ToUpper().Contains("Corp") ? securityCode : securityCode + " Corp";

            _securities.Add(tempSecurityDto.SecurityCode);
            var request = new { securities = _securities, fields = _fields,conn="SAPI",app=ConfigurationManager.AppSettings["ApplicationName"] };

            string result = "";
            string json = JsonConvert.SerializeObject(request);
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString(ConfigurationManager.AppSettings["BloombergUrl"], "POST", json);
            }

            SecurityReponse securityReponse = JsonConvert.DeserializeObject <SecurityReponse[]>(result)[0];

            var issuerResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "ISSUER");
            if (issuerResponse != null && issuerResponse.Value != null)
            {
                tempSecurityDto.Issuer =
                    Mapper.Map<Issuer, IssuerDto>(repository.AddIfMissingIssuer(issuerResponse.Value.ToString(),user));
            }

            var loanTypeResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "LOAN_TYP");
            var lnReancheLetterResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "LN_TRANCHE_LETTER");

            if (loanTypeResponse != null && lnReancheLetterResponse != null && loanTypeResponse.Value != null && lnReancheLetterResponse.Value != null)
            {
                tempSecurityDto.Facility =
                    Mapper.Map<Facility, FacilityDto>(
                        repository.AddIfMissingFacility(loanTypeResponse.Value + " " + lnReancheLetterResponse.Value,user));
            }

            var lienIndicatorResponse =
                securityReponse.FieldResponses.FirstOrDefault(l => l.Name.Contains("_LIEN_INDICATOR") && l.Value != null && l.Value.ToString() == "Y");

            if (lienIndicatorResponse != null && lienIndicatorResponse.Value != null)
            {
                string lienType = null;
                if (lienIndicatorResponse.Name.Contains("FIRST"))
                {
                    lienType = "First Lien";
                }
                else if (lienIndicatorResponse.Name.Contains("SECOND"))
                {
                    lienType = "Second Lien";
                }
                else if (lienIndicatorResponse.Name.Contains("THIRD"))
                {
                    lienType = "Third Lien";
                }

                if (!string.IsNullOrEmpty(lienType))
                {
                    tempSecurityDto.LienType = Mapper.Map<LienType, LienTypeDto>(repository.GetLienType(lienType));
                }
            }

            var industrySectorResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "INDUSTRY_SECTOR");
            var industryGroupResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "INDUSTRY_GROUP");
            var industrySubGroupResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "INDUSTRY_SUBGROUP");

            if (industrySectorResponse != null && industryGroupResponse != null && industrySubGroupResponse != null)
            {
                tempSecurityDto.GICSIndustry = industrySectorResponse.Value.ToString() + " | " +
                                               industryGroupResponse.Value.ToString() + " | " +
                                               industrySubGroupResponse.Value.ToString();
            }

            var callDateResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "NXT_CALL_DT");
            if (callDateResponse != null && callDateResponse.Value != null)
            {
                tempSecurityDto.CallDate = DateTime.Parse(callDateResponse.Value.ToString()).ToString("MM/dd/yyyy");
            }

            var maturityDateResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "MATURITY");
            if (maturityDateResponse != null && maturityDateResponse.Value != null)
            {
                tempSecurityDto.MaturityDate = DateTime.Parse(maturityDateResponse.Value.ToString()).ToString("MM/dd/yyyy");
            }


            var lnSpeadCloseResponse = securityReponse.FieldResponses.FirstOrDefault(f => f.Name == "LN_SPREAD_CLOSE");
            if (lnSpeadCloseResponse != null && lnSpeadCloseResponse.Value != null)
            {
                tempSecurityDto.Spread = decimal.Parse(lnSpeadCloseResponse.Value.ToString())/100;
            }

            return tempSecurityDto;
        }
    }
}
