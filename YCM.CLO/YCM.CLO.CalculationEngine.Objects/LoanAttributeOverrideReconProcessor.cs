using System;
using System.Data.Entity;
using System.Linq;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Extensions;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class LoanAttributeOverrideReconProcessor : IProcessor
    {
        public bool Process(string user)
        {
            using (CLOContext cloContext = new CLOContext())
            {

                var currentDate = DateTime.Now.Date;
                var securityOverrides = cloContext.SecurityOverrides.Where(s =>
                                                                !(s.IsDeleted.HasValue && s.IsDeleted.Value)
                                                                &&
                                                                (!s.EffectiveFrom.HasValue ||
                                                                 s.EffectiveFrom.Value <= currentDate)
                                                                &&
                                                                (!s.EffectiveTo.HasValue ||
                                                                 s.EffectiveTo.Value >= currentDate)).ToArray();
                var securityIds = securityOverrides.Select(a => a.SecurityId).ToArray();
                var securities = cloContext.vw_Security.Where(s => securityIds.Contains(s.SecurityId)).AsNoTracking().ToDictionary(s=>s.SecurityId,s=>s);
                securityOverrides.ToList().ForEach(so =>
                {
                    
                    var newWsoOriginalValue =
                        securities[so.SecurityId].GetPropertyValue("Orig" +
                                                                   so.Field.JsonPropertyName.FirstLetterToUpper());

                    var overrideValueObject = securities[so.SecurityId].GetPropertyValue(so.Field.JsonPropertyName.FirstLetterToUpper());

                    var overrideValue = overrideValueObject?.ToString() ?? string.Empty;

                    string existingValue = string.IsNullOrEmpty(so.ExistingValue) ? string.Empty : so.ExistingValue;
                    string wsoValue = newWsoOriginalValue?.ToString() ?? string.Empty;
                    
                    so.IsConflict = existingValue != wsoValue;

                    so.LastUpdatedBy = user;
                    so.LastUpdatedOn = DateTime.Now;

                    if (overrideValue == wsoValue)
                    {
                        so.EffectiveTo = currentDate.AddDays(-1);
                        so.IsConflict = false;
                    }
                });

                cloContext.SaveChanges();
            }
            return true;
        }
    }
}
