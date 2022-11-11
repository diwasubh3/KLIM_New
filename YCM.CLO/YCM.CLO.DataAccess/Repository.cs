using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Extensions;
using Rule = YCM.CLO.DataAccess.Models.Rule;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Entity.Infrastructure;

namespace YCM.CLO.DataAccess
{
    public class Repository : IRepository
    {

        private readonly CLOContext _cloContext;
        const int timeout_short = 1000;

        public Repository()
        {
            _cloContext = new CLOContext();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _cloContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Repository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        List<CacheSetting> IRepository.GetCacheSettings() => _cloContext.CacheSettings.ToList();
        IEnumerable<vw_CLOSummary> IRepository.GetSummaries(int dateId)
        {
            using (CLOContext cloContext = new CLOContext())
            {
                /* 
				 * As per user request, Summary section need to be available whole day and change for the same managed at database level
				 * 
				 --Old Code
				 return cloContext.vw_CLOSummary.Where(v=>v.DateId==dateId).OrderBy(c=>c.SortOrder).ToList();
				 * */
                return cloContext.vw_CLOSummary.OrderBy(c => c.SortOrder).ToList();
            }
        }

        IEnumerable<vw_CLOTestResults> IRepository.GetTestResults(int dateId)
        {
            using (CLOContext cloContext = new CLOContext())
            {
                /* 
				 * As per user request, Summary section need to be available whole day and change for the same managed at database level
				 * 
				 --Old Code
				 return cloContext.vw_CLOSummary.Where(v=>v.DateId==dateId).OrderBy(c=>c.SortOrder).ToList();
				 * */
                return cloContext.vw_CLOTestResults.OrderBy(c => c.SortBy).ToList();
            }
        }

        bool IRepository.CalculateSummaries()
        {
            using (CLOContext context = new CLOContext())
            {
                context.Database.CommandTimeout = timeout_short;

                string ConnectionString = context.Database.Connection.ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "CLO.spCalculateSummaryData";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    var affectedRows = cmd.ExecuteNonQuery();

                    context.Funds.Where(f => f.IsActive).ToList().ForEach(fund =>
                    {
                        this.UpdateFundTriggersForMatrixPoint(fund.FundId);
                    });

                    return affectedRows >= 0;
                }
            }
        }


        bool IRepository.GenerateAggregatedPositions()
        {
            using (CLOContext context = new CLOContext())
            {
                context.Database.CommandTimeout = timeout_short;

                string ConnectionString = context.Database.Connection.ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "CLO.spGenerateAggregatedPositions";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    var affectedRows = cmd.ExecuteNonQuery();

                    return affectedRows >= 0;
                }

            }
        }


        AnalystResearchHeader IRepository.GetAnalystResearchHeader(int issuerId)
            => _cloContext.AnalystResearchHeaders.FirstOrDefault(x => x.IssuerId == issuerId);

        IEnumerable<Rule> IRepository.GetRules()
        {
            var rules = _cloContext.Rules.OrderBy(r => r.SortOrder).ToList();
            rules.ForEach(r => r.RuleFields = r.RuleFields.OrderBy(rf => rf.SortOrder).ToList());
            return rules;
        }

        List<CustomView> IRepository.GetCustomViews() => GetCustomViewsInternal();

        private List<CustomView> GetCustomViewsInternal()
        {
            var views = _cloContext.CustomViews.ToList();
            SetCustomViewDefaults(views);
            return views;
        }

        private void SetCustomViewDefaults(List<CustomView> views)
        {
            RemoveAllCustomViewDefaults(views);
            var defaults = _cloContext.UserDefaultCustomViews.ToList();

            foreach (var def in defaults)
            {
                var view = views.FirstOrDefault(x => x.ViewId == def.ViewId && x.UserId == def.UserId);
                if (view != null)
                    view.IsDefault = true;
            }
        }

        private static void RemoveAllCustomViewDefaults(List<CustomView> views)
        {
            foreach (var view in views)
            {
                view.IsDefault = false;
            }
        }

        List<CustomViewField> IRepository.GetCustomViewFields() => _cloContext.CustomViewFields.ToList();
        List<ApplicationRole> IRepository.GetApplicationRoles() => _cloContext.ApplicationRoles.ToList();
        List<UserApplicationRole> IRepository.GetUserApplicationRoles() => _cloContext.UserApplicationRoles.ToList();

        List<AnalystResearchDetail> IRepository.GetAnalystResearchDetails(int headerId)
            => _cloContext.AnalystResearchDetails.AsNoTracking()
                .Where(x => x.AnalystResearchHeaderId == headerId).ToList();

        Rule IRepository.GetRule(short ruleId)
        {
            return _cloContext.Rules.AsNoTracking().First(r => r.RuleId == ruleId);
        }

        IEnumerable<vw_Position> IRepository.GetTop10Positions(string fundCode, int dateId)
        {
            return _cloContext.vw_Position.AsNoTracking().Where(p => p.FundCode == fundCode && p.PositionDateId == dateId).Take(10);
        }

        IEnumerable<vw_Position> IRepository.GetBottom10Positions(string fundCode, int dateId)
        {
            return _cloContext.vw_Position.AsNoTracking().Where(p => p.FundCode == fundCode && p.PositionDateId == dateId).Take(10);
        }

        IEnumerable<vw_Position> IRepository.GetPositions(string fundCode, bool? onlyWithExposures)
        {

            SqlParameter paramSecurityId = new SqlParameter("@paramSecurityId", DBNull.Value);
            SqlParameter paramSecurityCode = new SqlParameter("@paramSecurityCode", DBNull.Value);
            SqlParameter paramFundCode = new SqlParameter("@paramFundCode", string.IsNullOrEmpty(fundCode) ? DBNull.Value : (object)fundCode);
            SqlParameter paramOnlyWithExposures = new SqlParameter("@paramOnlyWithExposures", onlyWithExposures ?? false) { SqlDbType = SqlDbType.Bit };

            return _cloContext.Database.SqlQuery<vw_Position>("CLO.spGetPositions @paramSecurityId, @paramSecurityCode, @paramFundCode,@paramOnlyWithExposures ", paramSecurityId, paramSecurityCode, paramFundCode, paramOnlyWithExposures);
        }

        IEnumerable<vw_Position> IRepository.GetPositions(string securityCode, string fundCode)
        {
            SqlParameter paramSecurityId = new SqlParameter("@paramSecurityId", DBNull.Value);
            SqlParameter paramSecurityCode = new SqlParameter("@paramSecurityCode", securityCode);
            SqlParameter paramFundCode = new SqlParameter("@paramFundCode", fundCode);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<vw_Position>("CLO.spGetPositions @paramSecurityId, @paramSecurityCode, @paramFundCode", paramSecurityId, paramSecurityCode, paramFundCode);
        }

        IEnumerable<vw_Position> IRepository.GetPositions(int securityId)
        {
            SqlParameter paramSecurityId = new SqlParameter("@paramSecurityId", securityId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<vw_Position>("CLO.spGetPositions @paramSecurityId", paramSecurityId).ToList();
        }

        IEnumerable<vw_Position_Exposure> IRepository.GetPositionExposures(int securityId)
        {
            SqlParameter paramSecurityId = new SqlParameter("@paramSecurityId", securityId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<vw_Position_Exposure>("CLO.spGetPositionExposure @paramSecurityId", paramSecurityId).ToList();
        }

        IEnumerable<vw_Position> IRepository.GetPositions(int[] securityIds, string fundCode)
        {
            SqlParameter paramFundCode = new SqlParameter("@paramFundCode", fundCode);
            SqlParameter paramSecurityIds = GetSecurityListParam(securityIds);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<vw_Position>("CLO.spGetPositionForSecurities @paramFundCode, @paramSecurityIds", paramFundCode, paramSecurityIds);
        }

        IEnumerable<vw_Security_Watch> IRepository.GetSecurityWatch(int[] securityIds)
        {

            SqlParameter paramSecurityIds = GetSecurityListParam(securityIds);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<vw_Security_Watch>("CLO.spGetSecurityWatch @paramSecurityIds", paramSecurityIds);
        }

        IEnumerable<vw_Mismatch> IRepository.GetMismatchData(int fieldId)
        {
            SqlParameter paramFieldId = new SqlParameter("@fieldId", fieldId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<vw_Mismatch>("CLO.spGetMismatchData @fieldId", paramFieldId);
        }

        public IEnumerable<RatingChange> GetRatingChanges(int startDateId, int endDateId)
            => _cloContext.Database.SqlQuery<RatingChange>(
                "CLO.spGetRatingsChange @startDateId, @endDateId",
                new SqlParameter("@startDateId", startDateId),
                new SqlParameter("@endDateId", endDateId));


        private static SqlParameter GetSecurityListParam(int[] securityIds)
        {
            SqlParameter paramSecurityIds = new SqlParameter("@paramSecurityIds", securityIds.ToTableValuedParameter())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.IntegerArray"
            };
            return paramSecurityIds;
        }
        IQueryable<FieldGroup> IRepository.GetFieldGroups()
        {
            return _cloContext.FieldGroups.AsNoTracking();
        }

        IEnumerable<Field> IRepository.GetFields(string fieldGroupName)
        {
            SqlParameter paramFieldGroupName = new SqlParameter("@paramFieldGroupName", fieldGroupName);
            return _cloContext.Database.SqlQuery<Field>("CLO.spGetFields @paramFieldGroupName", paramFieldGroupName);
        }

        IEnumerable<Operator> IRepository.GetOperators()
        {
            return _cloContext.Operators.AsNoTracking().ToList();
        }

        IEnumerable<FundRestrictionType> IRepository.GetFundRestrictionTypes()
        {
            var data = _cloContext.Database.SqlQuery<FundRestrictionType>("CLO.spGetFundRestrictionTypes").ToList();
            return data;
        }

        IEnumerable<FundRestriction> IRepository.GetFundRestrictions(int? fundId)
        {
            return _cloContext.FundRestrictions.AsNoTracking().Where(f => !fundId.HasValue || f.FundId == fundId.Value).ToList();
        }

        bool IRepository.SaveFundRestrictions(IEnumerable<FundRestriction> fundRestrictions)
        {
            var existingFundRestrictions = _cloContext.FundRestrictions.ToList();
            fundRestrictions.ToList().ForEach(f =>
            {
                var existingFundRestriction = existingFundRestrictions.FirstOrDefault(ef => ef.Id == f.Id);
                if (existingFundRestriction != null)
                {
                    existingFundRestriction.RestrictionValue = f.RestrictionValue;
                    existingFundRestriction.OperatorId = f.OperatorId;
                }
            });
            _cloContext.SaveChanges();
            return true;
        }

        IEnumerable<vw_Position> IRepository.AddOrUpdateWatch(Watch watch, int dateId)
        {
            try
            {
                var queryWatchObjectType =
                    (WatchObjectTypeEnum)Enum.Parse(typeof(WatchObjectTypeEnum), watch.WatchObjectTypeId.ToString());
                var queryObjectId = watch.WatchObjectId;

                if (watch.WatchId > 0)
                {
                    var watchToUpdate = _cloContext.Watches.Single(w => w.WatchId == watch.WatchId);

                    if (watchToUpdate.WatchObjectTypeId == (int)WatchObjectTypeEnum.Issuer)
                    {
                        queryWatchObjectType = WatchObjectTypeEnum.Issuer;
                        queryObjectId = watchToUpdate.WatchObjectId;
                    }
                    watchToUpdate.WatchObjectTypeId = watch.WatchObjectTypeId;
                    watchToUpdate.WatchObjectId = watch.WatchObjectId;
                    watchToUpdate.WatchComments = watch.WatchComments;
                }
                else
                {
                    _cloContext.Watches.Add(watch);
                }

                _cloContext.SaveChanges();

                if (queryWatchObjectType == WatchObjectTypeEnum.Issuer)
                {
                    return _cloContext.vw_Position.Where(p => p.IssuerId == queryObjectId).ToList();
                }
                else
                {
                    return _cloContext.vw_Position.Where(p => p.SecurityId == queryObjectId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ParameterValue IRepository.AddOrUpdateParameterValue(ParameterValue parameterValue)
        {
            if (parameterValue.Id > 0)
            {
                _cloContext.ParameterValues.Attach(parameterValue);
                _cloContext.Entry(parameterValue).State = EntityState.Modified;
            }
            else
            {
                _cloContext.ParameterValues.Add(parameterValue);
                _cloContext.Entry(parameterValue).State = EntityState.Added;
            }

            _cloContext.SaveChanges();

            return _cloContext.ParameterValues.Single(pv => pv.Id == parameterValue.Id);
        }

        bool IRepository.TransferSecurities(int sourceSecurityId, int destSecurityId, string user)
        {
            var destSecurity = _cloContext.Securities.First(s => s.SecurityId == destSecurityId);

            var securitiesOverrides = _cloContext.SecurityOverrides.Where(so => so.SecurityId == sourceSecurityId).ToList();
            securitiesOverrides.ForEach(so =>
            {
                so.SecurityId = destSecurityId;
                so.Security = destSecurity;
            });

            var trades = _cloContext.Trades.Where(t => t.SecurityId == sourceSecurityId).ToList();
            trades.ForEach(t =>
            {
                t.SecurityId = destSecurityId;
                t.Security = destSecurity;
            });

            _cloContext.SaveChanges();
            return true;
        }

        IEnumerable<string> IRepository.GetSecurityCodesForBidOfferDownload()
        {
            DateTime lastMonth = DateTime.Now.AddDays(-30);

            return
                _cloContext.vw_Position.Where(
                    p =>
                        p.PositionDateId > 0 ||
                        p.SecurityCreatedOn >= lastMonth)
                    .Select(s => s.SecurityCode)
                    .Distinct().OrderBy(s => s)
                    .ToArray();
        }

        IEnumerable<vw_Position> IRepository.DeleteWatch(int watchId, int dateId)
        {
            var watchToDelete = _cloContext.Watches.SingleOrDefault(w => w.WatchId == watchId);
            if (watchToDelete != null)
            {
                var queryWatchObjectType =
                    (WatchObjectTypeEnum)
                        Enum.Parse(typeof(WatchObjectTypeEnum), watchToDelete.WatchObjectTypeId.ToString());
                var queryObjectId = watchToDelete.WatchObjectId;

                _cloContext.Watches.Remove(watchToDelete);
                _cloContext.SaveChanges();

                if (queryWatchObjectType == WatchObjectTypeEnum.Issuer)
                {
                    return _cloContext.vw_Position.Where(p => p.IssuerId == queryObjectId).ToList();
                }
                else
                {
                    return _cloContext.vw_Position.Where(p => p.SecurityId == queryObjectId).ToList();
                }
            }
            else
            {
                return null;
            }
        }

        IEnumerable<ParameterValue> IRepository.GetParameterValues(string parameterTypeName)
        {
            return
                _cloContext.ParameterTypes.Include(p => p.ParameterValues)
                    .First(p => p.ParameterTypeName == parameterTypeName)
                    .ParameterValues;
        }

        IEnumerable<ParameterValue> IRepository.GetParameterValues()
        {
            return
                _cloContext.ParameterValues.Include(p => p.ParameterType).AsNoTracking().ToList();
        }

        IEnumerable<ParameterType> IRepository.GetParameterTypes()
        {
            return
                _cloContext.ParameterTypes.AsNoTracking().ToList();
        }

        IEnumerable<AlertProcessor> IRepository.GetAlertProcessors()
        {
            return _cloContext.AlertProcessors.Where(a => !a.IsActive.HasValue || a.IsActive.Value).AsNoTracking();
        }


        RuleResult IRepository.ExecuteRule(Rule rule, string fundCode, int dateId)
        {

            RuleResult ruleResult = new RuleResult();
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() =>
            {
                using (CLOContext cloContext = new CLOContext())
                {
                    ruleResult.TopPositions = ExecuteRule(cloContext, "Top", rule, fundCode, dateId).ToList();
                }

            });
            tasks[1] = Task.Factory.StartNew(() =>
            {
                using (CLOContext cloContext = new CLOContext())
                {
                    ruleResult.BottomPositions = ExecuteRule(cloContext, "Bottom", rule, fundCode, dateId).ToList();
                }
            });

            Task.WaitAll(tasks);

            return ruleResult;
        }

        int IRepository.GetPrevDayDateId()
            => _cloContext.Database.SqlQuery<int>("SELECT CLO.GetPrevDayDateId()").First();

        int IRepository.GetPrevToPrevDayDateId()
            => _cloContext.Database.SqlQuery<int>("SELECT CLO.GetPrevToPrevDayDateId()").First();

        private IEnumerable<vw_AggregatePosition> ExecuteRule(CLOContext context, string ruleSectionName, Rule rule, string fundCode, int dateId)
        {
            try
            {
                SqlParameter paramRuleSectionName = new SqlParameter("@ruleSectionName", ruleSectionName);
                SqlParameter paramFundCode = new SqlParameter("@fundCode", fundCode);
                SqlParameter paramDateId = new SqlParameter("@dateId", dateId);
                context.Database.CommandTimeout = timeout_short;
                return context.vw_AggregatePosition.SqlQuery(
                    $"{rule.ExecutionStoredProcedure} @ruleSectionName, @fundCode, @dateId ",
                    paramRuleSectionName, paramFundCode, paramDateId).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        IEnumerable<int> IRepository.GetCreditScoreAlertIssuers(string fundCode, int dateId)
        {
            SqlParameter paramFundCode = new SqlParameter("@paramFundCode", fundCode);
            SqlParameter paramDateId = new SqlParameter("@dateId", dateId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<int>("CLO.spGetCreditScoreAlertIssuers @paramFundCode, @dateId", paramFundCode, paramDateId);
        }

        IEnumerable<SecurityOverride> IRepository.GetSecurityOverrides(LoadTypeEnum? loadTypeEnum, int? securityId)
        {
            DateTime dateTime = DateTime.Today;
            if (loadTypeEnum.HasValue && loadTypeEnum == LoadTypeEnum.Active)
            {
                return _cloContext.SecurityOverrides.AsNoTracking().Include(s => s.Security)
                    .Where(s => (!s.EffectiveTo.HasValue || s.EffectiveTo.Value >= dateTime)
                    && !(s.IsDeleted.HasValue && s.IsDeleted.Value)
                    ).OrderBy(a => a.Security.SecurityCode).ThenBy(a => a.Field.FieldTitle).ThenByDescending(a => a.SecurityOverrideId);
            }
            else if (loadTypeEnum.HasValue && loadTypeEnum == LoadTypeEnum.Historical)
            {
                return _cloContext.SecurityOverrides.AsNoTracking().Include(s => s.Security).Where(s =>
                      (!s.EffectiveFrom.HasValue || s.EffectiveFrom.Value <= dateTime) && s.EffectiveTo.HasValue && s.EffectiveTo.Value < dateTime && !(s.IsDeleted.HasValue && s.IsDeleted.Value)
                      ).OrderBy(a => a.Security.SecurityCode).ThenBy(a => a.Field.FieldTitle).ThenByDescending(a => a.SecurityOverrideId);
            }
            else if (loadTypeEnum.HasValue && loadTypeEnum == LoadTypeEnum.All)
            {
                return
                    _cloContext.SecurityOverrides.AsNoTracking().Include(s => s.Security)
                        .Where(s => !(s.IsDeleted.HasValue && s.IsDeleted.Value)).OrderBy(a => a.Security.SecurityCode).ThenBy(a => a.Field.FieldTitle).ThenByDescending(a => a.SecurityOverrideId); ;
            }
            else
            {
                return
                    _cloContext.SecurityOverrides.AsNoTracking().Include(s => s.Security)
                        .Where(s => !(s.IsDeleted.HasValue && s.IsDeleted.Value) && s.SecurityId == securityId
                        && !(s.IsDeleted.HasValue && s.IsDeleted.Value)
                        ).OrderBy(a => a.Security.SecurityCode).ThenBy(a => a.Field.FieldTitle).ThenByDescending(a => a.SecurityOverrideId);
            }
        }

        IEnumerable<Field> IRepository.GetSecurityOverrideableFields()
        {
            return _cloContext.Fields.AsNoTracking().Where(f => f.IsSecurityOverride.HasValue && f.IsSecurityOverride.Value).OrderBy(f => f.FieldTitle);
        }

        IEnumerable<Field> IRepository.GetFields(string[] fieldNames)
        {
            return _cloContext.Fields.AsNoTracking().Where(f => fieldNames.Contains(f.FieldName));
        }

        bool IRepository.SaveSecurityOverrides(SecurityOverride[] securityOverrides, string user)
        {
            securityOverrides.ToList().ForEach(so =>
            {
                if (so.SecurityOverrideId > 0)
                {
                    _cloContext.SecurityOverrides.Attach(so);
                    _cloContext.Entry(so).State = EntityState.Modified;
                    if (so.Field != null)
                    {
                        _cloContext.Entry(so.Field).State = EntityState.Unchanged;
                    }
                    if (so.Security != null)
                    {
                        _cloContext.Entry(so.Security).State = EntityState.Unchanged;
                    }

                    so.LastUpdatedOn = DateTime.Now;
                    so.LastUpdatedBy = user;
                }
                else
                {
                    if (!(so.IsDeleted.HasValue && so.IsDeleted.Value))
                    {
                        _cloContext.SecurityOverrides.Add(so);
                        _cloContext.Entry(so).State = EntityState.Added;

                        if (so.Field != null)
                        {
                            _cloContext.Entry(so.Field).State = EntityState.Unchanged;
                        }
                        if (so.Security != null)
                        {
                            _cloContext.Entry(so.Security).State = EntityState.Unchanged;
                        }

                        so.CreatedBy = user;
                        so.LastUpdatedBy = user;
                        so.CreatedOn = DateTime.Now;
                        so.LastUpdatedOn = DateTime.Now;
                    }
                }
            });


            _cloContext.SaveChanges();

            return true;
        }

        IEnumerable<vw_Security> IRepository.GetSecurities()
        {
            return _cloContext.vw_Security.Where(s => !s.SourceId.HasValue || s.SourceId.Value == 0).AsNoTracking().ToList();
        }

        IEnumerable<vw_SecurityMarketCalculation> IRepository.GetCurrentSecurities()
        {
            return _cloContext.vw_SecurityMarketCalculation.SqlQuery("CLO.spGetSecurities").ToList();
        }

        IEnumerable<Security> IRepository.GetSecuritiesForRecon()
        {
            var lastTempSecurityCreateDateTime =
                _cloContext.Securities.Where(s => s.SourceId == 1 && !(s.IsDeleted.HasValue && s.IsDeleted.Value))
                    .OrderBy(s => s.CreatedOn)
                    .FirstOrDefault();
            DateTime lastCreatedDateTime = DateTime.Now.AddDays(-7);

            if (lastTempSecurityCreateDateTime?.CreatedOn != null)
                lastCreatedDateTime = lastTempSecurityCreateDateTime.CreatedOn.Value.AddDays(-7);

            return _cloContext.Securities.Where(s => !(s.IsDeleted.HasValue && s.IsDeleted.Value) && s.CreatedOn >= lastCreatedDateTime)
                .Include(s => s.Issuer).AsNoTracking().ToList();
        }

        IEnumerable<Security> IRepository.GetSecurities(string[] securities)
        {
            return _cloContext.Securities.Where(s => (!s.SourceId.HasValue || s.SourceId.Value == 0) && securities.Contains(s.SecurityCode) && !(s.IsDeleted.HasValue && s.IsDeleted.Value)).AsNoTracking().ToList();
        }

        User IRepository.GetPerson(string userName)
        {
            //userId = (userId.IndexOf("YORK", StringComparison.Ordinal) >= 0 ? userId : "YORK\\" + userId).ToUpper();
            //var person = new v_PhoneList();//_cloContext.v_PhoneList.FirstOrDefault(v => v.NetworkId == userId);
            //person = person ??
            //		 new v_PhoneList() { FirstName = userId, LastName = userId, Email = userId + "@yorkcapital.com", DisplayName = userId };
            //return person;
            userName = userName.Substring(userName.LastIndexOf('\\') + 1).ToLower();
            var person = _cloContext.Users.FirstOrDefault(x => x.USerNAme == userName);
            if (person == null) person = new User();
            return person;

        }

        IEnumerable<vw_YorkCoreGenevaAnalyst> IRepository.GetAnalysts()
            => _cloContext.vw_YorkCoreGenevaAnalysts.AsNoTracking();

        IEnumerable<AnalystResearchFile> IRepository.GetAnalystResearchFiles()
            => _cloContext.AnalystResearchFiles.AsNoTracking();

        IEnumerable<AnalystResearchHeader> IRepository.GetAnalystResearchHeaders()
            => _cloContext.AnalystResearchHeaders.AsNoTracking();

        IEnumerable<AnalystResearchDetail> IRepository.GetAnalystResearchDetails()
            => _cloContext.AnalystResearchDetails.AsNoTracking();

        IEnumerable<AnalystResearchRowLocation> IRepository.GetAnalystResearchRowLocations()
            => _cloContext.AnalystResearchRowLocations.AsNoTracking();

        IEnumerable<AnalystResearch> IRepository.GetAnalystResearches()
        {
            return _cloContext.AnalystResearches.AsNoTracking().Include(a => a.Issuer).OrderBy(a => a.Issuer.IssuerDesc).ThenByDescending(a => a.AsOfDate).ToList();
        }

        IQueryable<vw_AnalystResearch> IRepository.GetActiveAnalystResearches()
        {
            return _cloContext.vw_AnalystResearch.AsNoTracking().OrderBy(a => a.IssuerCode);
        }

        DatabaseEntityOperationResult IRepository.SaveAnalystResearchHeader(AnalystResearchHeader existing
            , AnalystResearchHeader header)
        {
            using (var tran = _cloContext.Database.BeginTransaction())
            {
                var id = header?.Id ?? default(int);
                try
                {
                    var operation = GetOperationById(header.AnalystResearchHeaderId);
                    var histToAdd = existing ?? header;
                    var history = new AnalystResearchHeaderHistory(histToAdd, operation);
                    SaveEntityInternal(history);
                    SaveEntityInternal(header);
                    tran.Commit();
                    return new DatabaseEntityOperationResult(header.AnalystResearchHeaderId
                            , true, null);
                }
                catch (Exception ex)
                {
                    return new DatabaseEntityOperationResult(id
                            , false, ex);
                }
            }
        }

        DatabaseEntityOperationResult IRepository.SaveAnalystResearchDetail(AnalystResearchDetail detail)
            => SaveEntityInternal(detail);

        DatabaseEntityOperationResult IRepository.SaveAnalystResearchDetails(List<AnalystResearchDetail> existingDetails
            , List<AnalystResearchDetail> details)
        {
            using (var tran = _cloContext.Database.BeginTransaction())
            {
                try
                {
                    var priors = CreateAnalystDetailHistories(existingDetails, details);
                    SaveEntitiesInternal(priors);
                    SaveEntitiesInternal(details);
                    tran.Commit();
                    return new DatabaseEntityOperationResult(default(int)
                            , true, null);
                }
                catch (Exception ex)
                {
                    return new DatabaseEntityOperationResult(default(int)
                            , false, ex);
                }
            }
        }

        private List<AnalystResearchDetailHistory> CreateAnalystDetailHistories(List<AnalystResearchDetail> existingDetails
            , List<AnalystResearchDetail> details)
        {
            var histories = new List<AnalystResearchDetailHistory>();
            foreach (var detail in details)
            {
                var operation = GetOperationById(detail.AnalystResearchDetailId);
                var history = new AnalystResearchDetailHistory(detail, operation);
                histories.Add(history);
            }
            return histories;
        }

        private DatabaseOperation GetOperationById(int id)
            => id > 0 ? DatabaseOperation.Update : DatabaseOperation.Add;

        DatabaseEntityOperationResult IRepository.SaveEntity<T>(T entity)
            => SaveEntityInternal(entity);

        DatabaseEntityOperationResult AddOrUpdateEntityInternal<T>(T entity) where T : Entity
        {
            var id = default(int);
            try
            {
                if (entity.Id > 0)
                {
                    _cloContext.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    _cloContext.Set<T>().Add(entity);
                    _cloContext.Entry(entity).State = EntityState.Added;
                }
                _cloContext.SaveChanges();
                id = entity.Id;
            }
            catch (Exception ex)
            {
                return new DatabaseEntityOperationResult(default(int)
                    , false, ex);
            }
            return new DatabaseEntityOperationResult(id
                , true, null);
        }

        DatabaseEntityOperationResult SaveEntityInternal<T>(T entity) where T : Entity
        {
            var id = default(int);
            try
            {
                _cloContext.Set<T>().Add(entity);
                _cloContext.Entry(entity).State = entity.Id > 0 ? EntityState.Modified
                    : EntityState.Added;
                _cloContext.SaveChanges();
                id = entity.Id;
            }
            catch (Exception ex)
            {
                return new DatabaseEntityOperationResult(default(int)
                    , false, ex);
            }
            return new DatabaseEntityOperationResult(id
                    , true, null);
        }

        DatabaseEntityOperationResult DeleteEntityInternal<T>(T entity) where T : Entity
        {
            var id = default(int);
            try
            {
                _cloContext.Entry(entity).State = EntityState.Deleted;
                _cloContext.SaveChanges();
                id = entity.Id;
            }
            catch (Exception ex)
            {
                return new DatabaseEntityOperationResult(default(int)
                    , false, ex);
            }
            return new DatabaseEntityOperationResult(id
                , true, null);
        }

        List<DatabaseEntityOperationResult> IRepository.SaveEntities<T>(List<T> entities)
            => SaveEntitiesInternal(entities);

        List<DatabaseEntityOperationResult> SaveEntitiesInternal<T>(List<T> entities) where T : Entity
        {
            var results = new List<DatabaseEntityOperationResult>();
            foreach (var entity in entities)
            {
                _cloContext.Set<T>().Add(entity);
                _cloContext.Entry(entity).State = entity.Id > 0 ? EntityState.Modified
                    : EntityState.Added;
            }
            try
            {
                _cloContext.SaveChanges();
                foreach (var entity in entities)
                {
                    results.Add(new DatabaseEntityOperationResult(entity.Id, true, null));
                }
            }
            catch (Exception ex)
            {
                //TODO - come up with a better object for operation result lists
                results.Add(new DatabaseEntityOperationResult(default(int), false, ex));
            }
            return results;
        }

        bool IRepository.SaveAnalystResearches(AnalystResearch[] analystResearches)
        {
            analystResearches.ToList().ForEach(a =>
            {
                if (a.AnalystResearchId > 0)
                {
                    _cloContext.AnalystResearches.Attach(a);
                    _cloContext.Entry(a).State = EntityState.Modified;
                }
                else
                {
                    _cloContext.AnalystResearches.Add(a);
                    _cloContext.Entry(a).State = EntityState.Added;
                }
            });
            _cloContext.SaveChanges();
            return true;
        }

        bool IRepository.ClearExistingPrices(int dateId, int[] securities)
        {
            var existingPrices = _cloContext.Pricings.Where(
                price => price.DateId == dateId && securities.Contains(price.SecurityId));
            _cloContext.Pricings.RemoveRange(existingPrices);
            _cloContext.SaveChanges();
            return true;
        }

        bool IRepository.SavePrices(Pricing[] prices)
        {
            prices.ToList().ForEach(price =>
            {
                _cloContext.Pricings.Add(price);
                _cloContext.Entry(price).State = EntityState.Added;
            });
            _cloContext.SaveChanges();
            return true;
        }

        IQueryable<Issuer> IRepository.GetIssuers()
        {
            return _cloContext.Issuers.Where(i => i.IssuerId > 0);
        }

        IEnumerable<User> IRepository.GetUsers()
        {
            return _cloContext.Users;
        }

        IEnumerable<vw_Security> IRepository.GetSecurities(int[] securityIds)
        {
            return _cloContext.vw_Security.Where(s => securityIds.Contains(s.SecurityId)).AsNoTracking();
        }

        IEnumerable<SecurityOverride> IRepository.GetConflictingSecurityOverrides(int? securityId)
        {
            var currentDate = DateTime.Now.Date;
            return _cloContext.SecurityOverrides.Where(s =>
                                                            (!securityId.HasValue || s.SecurityId == securityId) &&
                                                                !(s.IsDeleted.HasValue && s.IsDeleted.Value)
                                                                &&
                                                                (!s.EffectiveFrom.HasValue ||
                                                                 s.EffectiveFrom.Value <= currentDate)
                                                                &&
                                                                (!s.EffectiveTo.HasValue ||
                                                                 s.EffectiveTo.Value >= currentDate) && s.IsConflict.HasValue && s.IsConflict.Value
                                                            ).AsNoTracking();
        }


        bool IRepository.DeleteSecurity(int securityId, string user)
        {
            var security = _cloContext.Securities.First(s => s.SecurityId == securityId);
            security.SecurityCode = string.Format(" {0} Deleted {1}", security.SecurityCode, Guid.NewGuid());
            security.LastUpdatedBy = user;
            security.IsDeleted = true;
            security.LastUpdatedOn = DateTime.Now;
            _cloContext.SaveChanges();
            return true;
        }


        bool IRepository.EndSecurityOverride(int securityOverrideId, DateTime endDate, string user)
        {
            var securityOverride = _cloContext.SecurityOverrides.First(s => s.SecurityOverrideId == securityOverrideId);
            securityOverride.EffectiveTo = endDate;
            securityOverride.IsConflict = false;
            securityOverride.LastUpdatedBy = user;
            securityOverride.LastUpdatedOn = DateTime.Now;
            _cloContext.SaveChanges();
            return true;
        }

        bool IRepository.ResetSecurityOverrideConflict(int securityOverrideId, int securityId, string user)
        {
            var securityOverride = _cloContext.SecurityOverrides.Include(s => s.Field).First(s => s.SecurityOverrideId == securityOverrideId);
            var vwSecurity = _cloContext.vw_Security.First(s => s.SecurityId == securityId);

            var newValue = vwSecurity.GetPropertyValue("Orig" + securityOverride.Field.JsonPropertyName.FirstLetterToUpper());

            securityOverride.ExistingValue = newValue?.ToString();

            securityOverride.IsConflict = false;
            securityOverride.LastUpdatedBy = user;
            securityOverride.LastUpdatedOn = DateTime.Now;
            _cloContext.SaveChanges();

            return true;
        }


        Trade IRepository.SaveTrade(Trade trade, string user)
        {
            trade.Security = null;
            trade.TradeAllocations.ToList().ForEach(ta =>
            {
                ta.Fund = null;
                if (ta.TradeAllocationId > 0)
                {
                    _cloContext.TradeAllocations.Attach(ta);
                    _cloContext.Entry(ta).State = EntityState.Modified;
                    ta.LastUpdatedBy = user;
                    ta.LastUpdatedOn = DateTime.Now;
                }
                else
                {
                    _cloContext.TradeAllocations.Add(ta);
                    _cloContext.Entry(ta).State = EntityState.Added;
                    ta.CreatedBy = user;
                    ta.CreatedOn = DateTime.Now;
                }
            });

            if (trade.TradeId > 0)
            {
                _cloContext.Trades.Attach(trade);
                _cloContext.Entry(trade).State = EntityState.Modified;
                trade.LastUpdatedBy = user;
                trade.LastUpdatedOn = DateTime.Now;
            }
            else
            {
                _cloContext.Trades.Add(trade);
                _cloContext.Entry(trade).State = EntityState.Added;
                trade.CreatedBy = user;
                trade.CreatedOn = DateTime.Now;
            }

            trade.FinalAllocation = trade.TradeAllocations.Sum(t => t.FinalAllocation.HasValue ? t.FinalAllocation.Value : 0);

            _cloContext.SaveChanges();
            var savedTrade = _cloContext.Trades.Include(t => t.TradeAllocations).Include("TradeAllocations.Fund").Include(t => t.Security).First(tr => tr.TradeId == trade.TradeId);

            return savedTrade;
        }

        IEnumerable<Trade> IRepository.GetTrades(bool includeCancelled, int dateId)
        {
            return _cloContext.Trades.Include(t => t.TradeAllocations).Include(t => t.Security).Where(t => (!includeCancelled
                                                                                                            && (!t.IsCancelled.HasValue || !t.IsCancelled.Value) || includeCancelled)
                && (t.KeepOnBlotter.HasValue && t.KeepOnBlotter.Value || t.DateId == dateId));
        }

        IEnumerable<Facility> IRepository.GetFacilities()
        {
            return _cloContext.Facilities.Where(f => f.FacilityId >= 0).ToList().OrderBy(f => f.FacilityDesc);
        }

        IEnumerable<AssetType> IRepository.GetAssetTypes()
        {
            return _cloContext.AssetType.Where(f => f.AssetId >= 0).ToList().OrderBy(f => f.AssetName);
        }

        IEnumerable<LienType> IRepository.GetLienTypes()
        {
            return _cloContext.LienTypes.ToList();
        }

        IEnumerable<Industry> IRepository.GetSnPIndustries()
        {
            return _cloContext.Industries.Where(i => i.IndustryId > 0 && i.IsSnP).ToList();
        }

        IEnumerable<Industry> IRepository.GetMoodyIndustries()
        {
            return _cloContext.Industries.Where(i => i.IndustryId > 0 && i.IsMoody).ToList();
        }


        Issuer IRepository.AddIfMissingIssuer(string issuer, string user)
        {
            var issuerDb = _cloContext.Issuers.FirstOrDefault(i => i.IssuerDesc == issuer);
            if (issuerDb == null)
            {
                _cloContext.Issuers.Add(new Issuer() { IssuerDesc = issuer, CreatedBy = user, CreatedOn = DateTime.Now });
                _cloContext.SaveChanges();
                issuerDb = _cloContext.Issuers.FirstOrDefault(i => i.IssuerDesc == issuer);
            }
            return issuerDb;
        }

        Facility IRepository.AddIfMissingFacility(string facility, string user)
        {
            var facilityDb = _cloContext.Facilities.FirstOrDefault(f => f.FacilityDesc == facility);
            if (facilityDb == null)
            {
                _cloContext.Facilities.Add(new Facility() { FacilityDesc = facility, CreatedOn = DateTime.Now, CreatedBy = user });
                _cloContext.SaveChanges();
                facilityDb = _cloContext.Facilities.FirstOrDefault(f => f.FacilityDesc == facility);
            }
            return facilityDb;
        }

        LienType IRepository.GetLienType(string lienType)
        {
            var lienTypeDb = _cloContext.LienTypes.FirstOrDefault(l => l.LienTypeDesc == lienType);
            return lienTypeDb;
        }

        IEnumerable<Rating> IRepository.GetRatings()
        {
            return _cloContext.Ratings.Where(r => r.RatingId > 0).ToList();
        }

        IEnumerable<Fund> IRepository.GetFunds()
        {
            var data = _cloContext.Funds.SqlQuery($"clo.spGetFunds").ToList();
            return data;
        }

        IEnumerable<Fund> IRepository.GetFundAllocation()
        {
            var data = _cloContext.Funds.SqlQuery($"clo.dbsp_GetFundAllocation").ToList();
            return data;
        }

        IEnumerable<TotalParChange> IRepository.GetTotalParChange(int startDateId, int endDateId)
            => _cloContext.Database.SqlQuery<TotalParChange>(
                "CLO.spGetTotalParDifference @CurrentDate, @PREVIOUSDATE",
                new SqlParameter("@CurrentDate", startDateId),
                new SqlParameter("@PREVIOUSDATE", endDateId));
        IEnumerable<MoodyRecoveryChange> IRepository.GetMoodyRecoveryChange(int startDateId, int endDateId)
            => _cloContext.Database.SqlQuery<MoodyRecoveryChange>(
                "CLO.spGetMoodyRecoveryChange @startDateId, @endDateId",
                new SqlParameter("@startDateId", startDateId),
                new SqlParameter("@endDateId", endDateId));

        Fund IRepository.SaveFund(Fund fund)
        {
            _cloContext.Funds.Attach(fund);
            _cloContext.Entry(fund).State = EntityState.Modified;
            _cloContext.SaveChanges();
            return fund;
        }

        Issuer IRepository.UpdateIsPrivate(int issuerId, bool isPrivate, string user)
        {
            var issuer = _cloContext.Issuers.FirstOrDefault(x => x.IssuerId == issuerId);
            issuer.IsPrivate = isPrivate;
            issuer.LastUpdatedOn = DateTime.Now;
            issuer.LastUpdatedBy = user;
            AddOrUpdateEntityInternal(issuer);
            //_cloContext.Issuers.Attach(issuer);
            //_cloContext.Entry(issuer).State = EntityState.Modified;
            //_cloContext.SaveChanges();
            return issuer;
        }

        Security IRepository.UpdateBbgId(int securityId, string bbgId, string user)
        {
            var security = _cloContext.Securities.FirstOrDefault(x => x.SecurityId == securityId);
            security.BBGId = bbgId;
            security.LastUpdatedOn = DateTime.Now;
            security.LastUpdatedBy = user;
            _cloContext.Securities.Attach(security);
            _cloContext.Entry(security).State = EntityState.Modified;
            _cloContext.SaveChanges();
            return security;
        }

        Security IRepository.AddUpdateSecurity(Security security, string user)
        {
            try
            {
                using (var duplicatecloContext = new CLOContext())
                {
                    var duplicateSecurityCode =
                    duplicatecloContext.Securities.FirstOrDefault(s => s.SecurityCode == security.SecurityCode);

                    if (duplicateSecurityCode != null)
                    {
                        security.SecurityId = duplicateSecurityCode.SecurityId;
                    }
                }

                security.IssuerId = security.Issuer.IssuerId;
                security.FacilityId = security.Facility.FacilityId;
                security.LienTypeId = security.LienType.LienTypeId;
                _cloContext.Entry(security.Issuer).State = EntityState.Unchanged;
                _cloContext.Entry(security.Facility).State = EntityState.Unchanged;
                _cloContext.Entry(security.LienType).State = EntityState.Unchanged;

                if (security.SecurityId <= 0)
                {
                    security.CreatedOn = DateTime.Now;
                    security.CreatedBy = user;
                    _cloContext.Securities.Add(security);
                    _cloContext.Entry(security).State = EntityState.Added;
                }
                else
                {
                    security.LastUpdatedOn = DateTime.Now;
                    security.LastUpdatedBy = user;
                    _cloContext.Securities.Attach(security);
                    _cloContext.Entry(security).State = EntityState.Modified;
                }

                _cloContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return security;
        }

        TradeSwap IRepository.GetLastTradeSwap()
        {
            return _cloContext.TradeSwaps.OrderByDescending(t => t.TradeSwapId).FirstOrDefault();
        }

        TradeSwapParam IRepository.GetTradeSwapParam(TradeSwap tradeSwap)
        {
            TradeSwapParam tradeSwapParam = new TradeSwapParam() { Criteria = new TradeSwapParamCriteria(), Constraints = new TradeSwapParamConstraints() };

            if (tradeSwap != null)
            {
                tradeSwapParam = JsonConvert.DeserializeObject<TradeSwapParam>(tradeSwap.Parameters);
            }

            return tradeSwapParam;
        }

        TradeSwapParam IRepository.SaveTradeSwap(TradeSwapParam tradeSwapParam, TradeSwap tradeSwap)
        {
            tradeSwap.Parameters = JsonConvert.SerializeObject(tradeSwapParam);

            if (tradeSwap.TradeSwapId > 0)
            {
                _cloContext.TradeSwaps.Attach(tradeSwap);
                _cloContext.Entry(tradeSwap).State = EntityState.Modified;
            }
            else
            {
                _cloContext.TradeSwaps.Add(tradeSwap);
                _cloContext.Entry(tradeSwap).State = EntityState.Added;
            }
            _cloContext.SaveChanges();

            return tradeSwapParam;
        }

        IEnumerable<vw_TradeSwap> IRepository.GetTradeSwaps(int fundId, int tradeSwapId)
        {
            SqlParameter paramFundId = new SqlParameter("@fundId", fundId);
            SqlParameter paramTradeSwapId = new SqlParameter("@tradeSwapId", tradeSwapId);
            return _cloContext.vw_TradeSwap.SqlQuery($"clo.spGetTradeSwaps @fundId,@tradeSwapId", paramFundId, paramTradeSwapId).ToList();
        }


        List<vw_AggregatePosition> IRepository.GetAllPositions(bool? onlyWithExposures)
        {
            try
            {
                SqlParameter paramSecurityId = new SqlParameter("@paramSecurityId", DBNull.Value);
                SqlParameter paramSecurityCode = new SqlParameter("@paramSecurityCode", DBNull.Value);
                SqlParameter paramOnlyWithExposures = new SqlParameter("@paramOnlyWithExposures", onlyWithExposures ?? false) { SqlDbType = SqlDbType.Bit };
                var data = _cloContext.Database.SqlQuery<vw_AggregatePosition>("CLO.spGetAllPositions @paramSecurityId, @paramSecurityCode, @paramOnlyWithExposures ", paramSecurityId, paramSecurityCode, paramOnlyWithExposures);
                return data?.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        IEnumerable<vw_AggregatePosition> IRepository.GetAllPositions(string securityCode)
        {
            SqlParameter paramSecurityId = new SqlParameter("@paramSecurityId", DBNull.Value);
            SqlParameter paramSecurityCode = new SqlParameter("@paramSecurityCode", securityCode);

            return _cloContext.Database.SqlQuery<vw_AggregatePosition>("CLO.spGetAllPositions @paramSecurityId, @paramSecurityCode", paramSecurityId, paramSecurityCode);
        }

        IEnumerable<vw_AggregatePosition> IRepository.GetAllPositions(int securityId)
        {
            SqlParameter paramSecurityId = new SqlParameter("@paramSecurityId", securityId);
            return _cloContext.Database.SqlQuery<vw_AggregatePosition>("CLO.spGetAllPositions @paramSecurityId", paramSecurityId);
        }

        IEnumerable<vw_AggregatePosition> IRepository.GetAllPositions(int[] securityIds)
        {
            SqlParameter paramSecurityIds = GetSecurityListParam(securityIds);
            return _cloContext.Database.SqlQuery<vw_AggregatePosition>("CLO.spGetAllPositionForSecurities @paramSecurityIds", paramSecurityIds);
        }


        IEnumerable<vw_PriceMove> IRepository.GetPriceMove(string section, int fromDateId, int toDateId)
        {
            using (CLOContext cloContext = new CLOContext())
            {
                SqlParameter paramFromDate = new SqlParameter("@fromDateId", fromDateId);
                SqlParameter paramToDateId = new SqlParameter("@toDateId", toDateId);
                SqlParameter paramSection = new SqlParameter("@sectionName", section);

                return cloContext.vw_PriceMove.SqlQuery($"clo.spGetPriceMove @fromDateId, @toDateId, @sectionName", paramFromDate, paramToDateId, paramSection).ToList();

            }
        }

        bool IRepository.CreateStalePositions(int fundId)
        {
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandLoadStalePositions = new SqlCommand("CLO.spLoadStalePositions", connection))
                    {
                        commandLoadStalePositions.CommandType = CommandType.StoredProcedure;
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@fundId", fundId));
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@currentDateId", Helper.GetPrevDayDateId()));
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@prevDateId", Helper.GetDateId(Helper.GetPrevBusinessDay(1))));
                        commandLoadStalePositions.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }

            return true;
        }


        bool IRepository.CaptureDailySnapshot(int fundId, int dateId)
        {
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandLoadStalePositions = new SqlCommand("CLO.spLoadFundDailySnapshot", connection))
                    {
                        commandLoadStalePositions.CommandType = CommandType.StoredProcedure;
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@dateId", Helper.GetPrevDayDateId()));
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@fundId", fundId));
                        commandLoadStalePositions.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }

            return true;
        }

        bool IRepository.CleanPositionsBasedOnPrincipalCash(int fundId, int dateId)
        {
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandLoadStalePositions = new SqlCommand("CLO.spCleanPositionsBasedOnPrincipalCash", connection))
                    {
                        commandLoadStalePositions.CommandType = CommandType.StoredProcedure;
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@fundId", fundId));
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@dateId", Helper.GetPrevDayDateId()));
                        commandLoadStalePositions.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }

            return true;
        }


        bool IRepository.RefillPositionsBasedOnPrincipalCash(int fundId, int dateId)
        {
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandLoadStalePositions = new SqlCommand("CLO.spRefillPositionsBasedOnPrincipalCash", connection))
                    {
                        commandLoadStalePositions.CommandType = CommandType.StoredProcedure;
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@fundId", fundId));
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@dateId", Helper.GetPrevDayDateId()));
                        commandLoadStalePositions.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }

            return true;
        }



        IEnumerable<Rating> IRepository.GetMoodyRatings()
        {
            return _cloContext.Ratings.Where(r => r.IsMoody.HasValue && r.IsMoody.Value).OrderByDescending(r => r.Rank).ToList();
        }

        IEnumerable<AssetExposure> IRepository.GetAssetPars(int dateId)
        {
            var param = new SqlParameter("@dateId", dateId);
            return _cloContext.Database.SqlQuery<AssetExposure>("CLO.spGetAssetPar @dateId", param).ToList();
        }

        IEnumerable<LoanPosition> IRepository.GetLoanPositions(int dateId, int priorDateId, int fundId)
        {
            var param = new SqlParameter("@dateId", dateId);
            var priorDateParam = new SqlParameter("@priorDateId", priorDateId);
            var fundParam = new SqlParameter("@fundId", fundId);
            return _cloContext.Database.SqlQuery<LoanPosition>("CLO.spGetAggregatedLoanPositions @dateId, @priorDateId, @fundId"
                , param, priorDateParam, fundParam).ToList();
        }

        IEnumerable<vw_ReinvestDetails> IRepository.GetReinvestDetails()
        {
            using (CLOContext context = new CLOContext())
            {
                return context.vw_ReinvestDetails.AsNoTracking().ToList();
            }
        }

        List<CreditScore> IRepository.GetCreditScores()
            => _cloContext.CreditScores.AsNoTracking().ToList();

        List<CustomView> IRepository.AddOrUpdateCustomView(CustomView view, string userName)
        {
            try
            {
                foreach (var customViewField in view.CustomViewFields)
                {
                    customViewField.IsPinned = customViewField.SortOrder <= 40;
                }
                if (view.Id > 0)
                {
                    foreach (var field in view.CustomViewFields)
                    {
                        if (field.Id == 0)
                        {
                            field.ViewId = view.ViewId;
                            _cloContext.Set<CustomViewField>().Add(field);
                            _cloContext.Entry(field).State = EntityState.Added;
                        }
                        else
                            _cloContext.Entry(field).State = EntityState.Modified;
                    }
                    //find deleted items
                    var fieldIds = view.CustomViewFields.Select(x => x.FieldId).ToList();
                    var cvfs = _cloContext.CustomViewFields.Where(x => x.ViewId == view.ViewId).ToList();
                    var deleted = cvfs.Where(x => !fieldIds.Contains(x.FieldId) && x.FieldId != Constants.FilterColumnFieldId).ToList();
                    foreach (var field in deleted)
                    {
                        _cloContext.Entry(field).State = EntityState.Deleted;
                        _cloContext.Set<CustomViewField>().Remove(field);
                    }
                    _cloContext.Entry(view).State = EntityState.Modified;
                }
                else
                {
                    //AddColumnIfMissing(view, userName, Constants.FilterColumnFieldId);
                    //AddColumnIfMissing(view, userName, Constants.SearchTextFieldId);
                    _cloContext.Set<CustomView>().Add(view);
                    _cloContext.Entry(view).State = EntityState.Added;
                }
                AddColumnIfMissing(view, userName, Constants.FilterColumnFieldId);
                AddColumnIfMissing(view, userName, Constants.SearchTextFieldId);
                _cloContext.SaveChanges();

                if (view.IsDefault)
                {
                    var oldDefault = _cloContext.UserDefaultCustomViews.FirstOrDefault(x => x.UserId == view.UserId);
                    if (oldDefault == null)
                        oldDefault = new UserDefaultCustomView
                        {
                            UserId = view.UserId,
                            CreatedBy = userName,
                            CreatedOn = DateTime.Now,
                        };
                    oldDefault.ViewId = view.ViewId;
                    oldDefault.LastUpdatedBy = userName;
                    oldDefault.CreatedOn = DateTime.Now;
                    oldDefault.LastUpdatedOn = DateTime.Now;

                    AddOrUpdateEntityInternal(oldDefault);
                }

                //var tempViews = _cloContext.CustomViews.ToList();
                //var oldDefaults = tempViews.Where(x => x.UserId == view.UserId
                //                                                     && x.ViewId != view.ViewId && x.IsDefault).ToList();
                //foreach (var oldDefault in oldDefaults)
                //{
                //	oldDefault.IsDefault = false;
                //}
                var views = GetCustomViewsInternal();
                return views;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private void AddColumnIfMissing(CustomView view, string userName, short fieldId)
        {
            if (view.CustomViewFields.Any(x => x.FieldId == fieldId))
                return;
            //add filter column
            var filterColumn = new CustomViewField
            {
                FieldId = fieldId,
                IsPinned = false,
                SortOrder = 9999999,
                CreatedBy = userName,
                LastUpdatedBy = userName
                ,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now,
                ViewId = view.ViewId
            };
            view.CustomViewFields.Add(filterColumn);
        }

        bool IRepository.ViewNameIsTaken(int userId, string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                return false;
            var exists = _cloContext.CustomViews.Any(x => (x.UserId == userId || x.IsPublic)
                && x.ViewName.Equals(viewName, StringComparison.CurrentCultureIgnoreCase));
            return exists;
        }

        List<CustomView> IRepository.DeleteCustomView(CustomView view)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandLoadStalePositions = new SqlCommand("CLO.spDeleteCustomView", connection))
                    {
                        commandLoadStalePositions.CommandType = CommandType.StoredProcedure;
                        commandLoadStalePositions.Parameters.Add(new SqlParameter("@viewId", view.ViewId));
                        commandLoadStalePositions.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                var views = _cloContext.CustomViews.ToList();
                return views;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        List<Field> IRepository.GetFieldsForCustomView(int viewId)
        {

            var view = _cloContext.CustomViews.FirstOrDefault(x => x.ViewId == viewId);
            var cfs = view.CustomViewFields.ToList();
            var fieldIds = cfs.Select(x => x.FieldId).ToList();
            //be sure to add fixed, hidden fields
            var fields = _cloContext.Fields.ToList()
                .Where(x => fieldIds.Contains(x.FieldId)
                    || (x.FieldGroupId == 5 && x.Hidden.GetValueOrDefault())
                    ).ToList();
            var hidden = fields.Where(x => x.Hidden.GetValueOrDefault() && x.FieldGroupId == 5).OrderBy(x => x.SortOrder);
            var sortOrder = 0;
            foreach (var field in hidden)
            {
                sortOrder++;
                field.SortOrder = sortOrder;
            }



            var impliedMatrixFields = fields.Where(x => x.FieldGroupId == 8).ToList();
            for (int i = 0; i < impliedMatrixFields.Count; i++)
            {
                impliedMatrixFields[i].Hidden = true;
            }


            fields = fields.OrderByDescending(x => x.FieldGroup.SortOrder)
                        .ThenBy(x => x.SortOrder).ToList();


            foreach (var field in fields)
            {
                var cf = cfs.FirstOrDefault(x => x.FieldId == field.FieldId);
                if (cf != null)
                {
                    field.PinnedLeft = cf.IsPinned;
                    field.SortOrder = cf.SortOrder;
                }

            }
            return fields;

        }

        List<UserDefaultCustomView> IRepository.GetUserDefaultCustomViews() => _cloContext.UserDefaultCustomViews.ToList();

        bool IRepository.UserIsAnAdmin(int userId)
        {
            var users = _cloContext.UserApplicationRoles.ToList();
            var user = users.FirstOrDefault(x => x.UserId == userId);
            var isAdmin = user?.ApplicationRole?.RoleDescription == "Admin";
            return isAdmin;
        }

        public IEnumerable<MatrixData> GenerateMinorMatrixData(int cloId)
        {
            List<MatrixData> minorMatrixDatas = new List<MatrixData>();
            List<MatrixData> spreadInterpolatedMinorDatas = new List<MatrixData>();
            using (CLOContext context = new CLOContext())
            {
                var majorMatrixDataSpreads = context.MatrixDatas.Where(m => m.FundId == cloId && m.DataPointType == 1).OrderBy(m => m.Diversity).ThenBy(m => m.Spread).ToList();
                for (int i = 0; i < majorMatrixDataSpreads.Count(); i++)
                {
                    var current = majorMatrixDataSpreads[i];
                    Console.WriteLine($"Current Row:{i} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity}, WarfModifier:{current.WarfModifier}");

                    if (i < majorMatrixDataSpreads.Count() - 1)
                    {

                        var next = majorMatrixDataSpreads[i + 1];
                        var numberOfSteps = (int)((next.Spread.Value - current.Spread.Value) / (decimal)0.0001);
                        numberOfSteps = numberOfSteps == 0 ? 1 : numberOfSteps;
                        var spreadStep = (next.Spread - current.Spread) / numberOfSteps;
                        var warfModifierStep = (next.WarfModifier - current.WarfModifier) / numberOfSteps;
                        var warfStep = (next.Warf - current.Warf) / numberOfSteps;
                        for (int j = 1; j < numberOfSteps; j++)
                        {
                            Console.WriteLine($"Spread Child Row:{j} Spread:{current.Spread + (spreadStep * j)}, Warf:{current.Warf + (warfStep * j)}, Diversity:{current.Diversity}, WarfModifier:{current.WarfModifier + (warfModifierStep * j)}");
                            var data = new MatrixData()
                            {
                                FundId = cloId,
                                Spread = current.Spread + (spreadStep * j),
                                Diversity = current.Diversity,
                                Warf = current.Warf + (warfStep * j),
                                WarfModifier = current.WarfModifier + (warfModifierStep * j),
                                DataPointType = 2,
                                InterpolationType = 1,
                                FromMajorMatrixDataId = current.Id,
                                ToMajorMatrixDataId = next.Id
                            };

                            minorMatrixDatas.Add(data);
                            spreadInterpolatedMinorDatas.Add(data);
                        }
                    }
                }

                var majorMatrixDataDiversities = context.MatrixDatas.Where(m => m.FundId == cloId && m.DataPointType == 1).OrderBy(m => m.Spread).ThenBy(m => m.Diversity).ToList();
                for (int i = 0; i < majorMatrixDataDiversities.Count(); i++)
                {
                    var current = majorMatrixDataDiversities[i];
                    Console.WriteLine($"Row:{i} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity}, WarfModifier:{current.WarfModifier}");

                    if (i < majorMatrixDataDiversities.Count() - 1)
                    {
                        var next = majorMatrixDataDiversities[i + 1];
                        var numberOfSteps = (int)((next.Diversity.Value - current.Diversity.Value) / (decimal)1);
                        var diveristyStep = (next.Diversity - current.Diversity) / numberOfSteps;

                        var warfModifierStep = (next.WarfModifier - current.WarfModifier) / numberOfSteps;
                        var warfStep = (next.Warf - current.Warf) / numberOfSteps;

                        for (int j = 1; j < numberOfSteps; j++)
                        {
                            Console.WriteLine($"Diversity Child Row:{j} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity + (diveristyStep * j)}, WarfModifier:{current.WarfModifier}");
                            minorMatrixDatas.Add(new MatrixData()
                            {
                                FundId = cloId,
                                Spread = current.Spread,
                                Diversity = current.Diversity + (diveristyStep * j),
                                Warf = current.Warf + (warfStep * j),
                                WarfModifier = current.WarfModifier + (warfModifierStep * j),
                                DataPointType = 2,
                                InterpolationType = 2,
                                FromMajorMatrixDataId = current.Id,
                                ToMajorMatrixDataId = next.Id
                            });
                        }
                    }
                }

                spreadInterpolatedMinorDatas = spreadInterpolatedMinorDatas.OrderBy(m => m.Spread).ThenBy(m => m.Diversity).ToList();

                for (int i = 0; i < spreadInterpolatedMinorDatas.Count(); i++)
                {
                    var current = spreadInterpolatedMinorDatas[i];
                    Console.WriteLine($"Row:{i} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity}, WarfModifier:{current.WarfModifier}");

                    if (i < spreadInterpolatedMinorDatas.Count() - 1)
                    {
                        var next = spreadInterpolatedMinorDatas[i + 1];
                        var numberOfSteps = (int)((next.Diversity.Value - current.Diversity.Value) / (decimal)1);
                        var diveristyStep = (next.Diversity - current.Diversity) / numberOfSteps;

                        var warfModifierStep = (next.WarfModifier - current.WarfModifier) / numberOfSteps;
                        var warfStep = (next.Warf - current.Warf) / numberOfSteps;

                        for (int j = 1; j < numberOfSteps; j++)
                        {
                            Console.WriteLine($"Diversity Child Row:{j} Spread:{current.Spread}, Warf:{current.Warf}, Diversity:{current.Diversity + (diveristyStep * j)}, WarfModifier:{current.WarfModifier}");
                            minorMatrixDatas.Add(new MatrixData()
                            {
                                FundId = cloId,
                                Spread = current.Spread,
                                Diversity = current.Diversity + (diveristyStep * j),
                                Warf = current.Warf + (warfStep * j),
                                WarfModifier = current.WarfModifier + (warfModifierStep * j),
                                DataPointType = 2,
                                InterpolationType = 3,
                                FromMajorMatrixDataId = 0,
                                ToMajorMatrixDataId = 0
                            });
                        }
                    }
                }


                return minorMatrixDatas;
            }


        }

        public IEnumerable<vw_MatrixData> GetMajorMatrixDatas(int fundId)
        {
            using (CLOContext context = new CLOContext())
            {

                return context.vw_MatrixDatas.AsNoTracking().Where(m => m.FundId == fundId && m.DataPoint == "Major").OrderBy(m => m.Spread).ThenBy(m => m.Diversity).ToList();
            }
        }

        public IEnumerable<vw_MatrixData> GetSpreadInterpolatedMinorMatrixDatas(int fundId, decimal fromSpread, decimal toSpread)
        {
            using (CLOContext context = new CLOContext())
            {
                return context.vw_MatrixDatas.AsNoTracking().Where(m => m.FundId == fundId && m.DataPoint == "Minor" && m.Spread > fromSpread && m.Spread < toSpread).OrderBy(m => m.Spread).ThenBy(m => m.Diversity).ToList();
            }
        }

        public IEnumerable<vw_MatrixData> GetDiversityInterpolatedMinorMatrixDatas(int fundId, decimal fromDiversity, decimal toDiversity)
        {
            using (CLOContext context = new CLOContext())
            {
                return context.vw_MatrixDatas.AsNoTracking().Where(m => m.FundId == fundId && m.DataPoint == "Minor" && m.Diversity > fromDiversity && m.Diversity < toDiversity).OrderBy(m => m.Diversity).ThenBy(m => m.Spread).ToList();
            }
        }

        public vw_MatrixData GetMatrixData(int fundId, decimal spread, decimal diversity)
        {
            using (CLOContext context = new CLOContext())
            {
                return context.vw_MatrixDatas.FirstOrDefault(m => m.FundId == fundId && m.Spread == spread && m.Diversity == diversity);
            }
        }


        public bool AddMatrixPoint(MatrixPoint matrixData, string user)
        {
            using (CLOContext context = new CLOContext())
            {
                context.MatrixPoints.Add(new MatrixPoint()
                {
                    FundId = matrixData.FundId,
                    CreatedBy = user,
                    CreatedOn = DateTime.Now,
                    Spread = matrixData.Spread,
                    Warf = matrixData.Warf,
                    DataPointType = matrixData.DataPointType,
                    WarfModifier = matrixData.WarfModifier,

                    LeftMajorDiversity = matrixData.LeftMajorDiversity,
                    RightMajorDiversity = matrixData.RightMajorDiversity,
                    TopMajorSpread = matrixData.TopMajorSpread,
                    BottomMajorSpread = matrixData.BottomMajorSpread,

                    LeftDiversity = matrixData.LeftDiversity,
                    RightDiversity = matrixData.RightDiversity,
                    TopSpread = matrixData.TopSpread,
                    BottomSpread = matrixData.BottomSpread,

                    Diversity = matrixData.Diversity
                });


                var fundRestrictions = (this as IRepository).GetFundRestrictions(matrixData.FundId).ToList();
                var fields = (this as IRepository).GetFields("Fund Restrictions").ToList();

                if (fundRestrictions.Count() == 0)
                {
                    fields.ForEach(f =>
                    {
                        fundRestrictions.Add(new FundRestriction()
                        {
                            Field = f,
                            FundId = matrixData.FundId,
                            FieldId = f.FieldId,
                            FundRestrictionTypeId = 1,
                            OperatorId = 5
                        });
                        fundRestrictions.Add(new FundRestriction()
                        {
                            Field = f,
                            FundId = matrixData.FundId,
                            FieldId = f.FieldId,
                            FundRestrictionTypeId = 2,
                            OperatorId = 4
                        });
                    }
                    );
                }

                var fundRestrictionFields = (this as IRepository).GetFields("Matrix Point Based Fund Restrictions Fields").ToList();

                var parameterValues = (this as IRepository).GetParameterValues("Matrix Point - Fund Restriction");

                var summary = (this as IRepository).GetSummaries(Helper.GetPrevDayDateId()).FirstOrDefault(summ => summ.FundId == matrixData.FundId);

                //Warf Recovery
                var warfConstant = parameterValues.FirstOrDefault(p => p.ParameterValueText == "WARF CONSTANT").ParameterMaxValueNumber;

                decimal matrixWarfRecovery = 0;
                if (summary.IsNewCalc == true)
                {
                    var diffRecovery = (summary.MoodyRecovery - parameterValues.FirstOrDefault(p => p.ParameterValueText == "WARF NEW RECOVERY").ParameterMaxValueNumber) * matrixData.WarfModifier;
                    if (summary.IsNewCalc == true)
                    {
                        diffRecovery = diffRecovery < warfConstant ? warfConstant : diffRecovery;
                    }
                    matrixWarfRecovery = Math.Round((matrixData.Warf + diffRecovery).Value);

                }
                else
                {
                    matrixWarfRecovery = Math.Round((matrixData.Warf + ((summary.MoodyRecovery - parameterValues.FirstOrDefault(p => p.ParameterValueText == "WARF RECOVERY").ParameterMaxValueNumber) * matrixData.WarfModifier)).Value);
                }
                //var matrixWarfRecovery = summary.IsNewCalc == true ? Math.Round((matrixData.Warf + ((summary.MoodyRecovery - parameterValues.FirstOrDefault(p => p.ParameterValueText == "WARF NEW RECOVERY").ParameterMaxValueNumber) * matrixData.WarfModifier)).Value)
                //           							: Math.Round((matrixData.Warf + ((summary.MoodyRecovery - parameterValues.FirstOrDefault(p => p.ParameterValueText == "WARF RECOVERY").ParameterMaxValueNumber) * matrixData.WarfModifier)).Value);



                //Spread
                var spreadFundRestrictions = fundRestrictions.Where(f => f.Field.FieldTitle == "SPREAD").ToList();
                spreadFundRestrictions.ForEach(fr =>
                {
                    fr.RestrictionValue = (fr.FundRestrictionTypeId == 1 ? (matrixData.Spread.Value + parameterValues.FirstOrDefault(p => p.ParameterValueText == "SPREAD").ParameterMaxValueNumber.Value) : matrixData.Spread.Value) * 100;
                });

                //Warf
                var warfFundRestrictions = fundRestrictions.Where(f => f.Field.FieldTitle == "WARF").ToList();
                warfFundRestrictions.ForEach(fr =>
                {
                    fr.RestrictionValue = (Int32)(fr.FundRestrictionTypeId == 1 ? (matrixWarfRecovery - parameterValues.FirstOrDefault(p => p.ParameterValueText == "WARF").ParameterMaxValueNumber.Value) : matrixWarfRecovery);
                });

                //Diversity
                var diversityFundRestrictions = fundRestrictions.Where(f => f.Field.FieldTitle == "DIVERSITY").ToList();
                diversityFundRestrictions.ForEach(fr =>
                {
                    fr.RestrictionValue = fr.FundRestrictionTypeId == 1 ? (matrixData.Diversity.Value + parameterValues.FirstOrDefault(p => p.ParameterValueText == "DIVERSITY").ParameterMaxValueNumber.Value) : matrixData.Diversity.Value;
                });

                (this as IRepository).SaveFundRestrictions(fundRestrictions);

                context.SaveChanges();
                return true;
            }
        }

        public IEnumerable<MatrixPoint> GetMatrixPoints(int fundId)
        {
            using (CLOContext context = new CLOContext())
            {
                return context.MatrixPoints.AsNoTracking().Where(m => m.FundId == fundId).OrderByDescending(m => m.Id).Take(100).ToList();
            }
        }

        public bool UpdateFundTriggersForMatrixPoint(int fundId)
        {
            using (CLOContext context = new CLOContext())
            {
                context.Database.CommandTimeout = timeout_short;

                string ConnectionString = context.Database.Connection.ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "CLO.spUpdateFundtriggersForMatrixPoint";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("FundId", fundId));
                    cmd.CommandTimeout = 0;
                    var affectedRows = cmd.ExecuteNonQuery();

                    return affectedRows >= 0;
                }
            }

        }


        public IEnumerable<AssetClass> GetAssetClasses()
        {
            using (CLOContext context = new CLOContext())
            {
                return context.AssetClasses.ToList();
            }
        }

        public IEnumerable<FundAssetClass> GetFundAssetClasses()
        {
            using (CLOContext context = new CLOContext())
            {
                return context.FundAssetClasses.OrderBy(a => a.AssetClassId).ToList();
            }
        }

        public IEnumerable<DefaultSecurity> GetDefaultSecurities(int dateId)
        {
            using (CLOContext context = new CLOContext())
            {
                return context.DefaultSecurities.Where(d => d.DateId == dateId).ToList();
            }
        }


        public IEnumerable<EquityOverride> GetEquityOverrides()
        {
            using (CLOContext context = new CLOContext())
            {
                return context.EquityOverrides.Where(d => d.IsDeleted == null || !d.IsDeleted.Value).ToList();
            }
        }


        public EquityOverride SaveEquityOverride(EquityOverride equityOverride, string user)
        {
            using (CLOContext context = new CLOContext())
            {
                if (equityOverride.Id == 0)
                {
                    context.EquityOverrides.Add(equityOverride);
                    context.Entry(equityOverride).State = EntityState.Added;
                    equityOverride.CreatedBy = user;
                    equityOverride.CreatedOn = DateTime.Now;
                }
                else
                {
                    context.EquityOverrides.Attach(equityOverride);
                    context.Entry(equityOverride).State = EntityState.Modified;
                    equityOverride.LastModifiedBy = user;
                    equityOverride.LastModifiedOn = DateTime.Now;

                }
                context.SaveChanges();
                return equityOverride;
            }
        }


        public FundAssetClass SaveFundAssetClass(FundAssetClass fundAssetClass, string user)
        {
            using (CLOContext context = new CLOContext())
            {
                //if (fundAssetClass.Id == 0)
                //{
                //    context.FundAssetClasses.Add(fundAssetClass);
                //    context.Entry(fundAssetClass).State = EntityState.Added;
                //    fundAssetClass.CreatedBy = user;
                //    fundAssetClass.CreatedOn = DateTime.Now;
                //}
                //else
                //{
                context.FundAssetClasses.Attach(fundAssetClass);
                context.Entry(fundAssetClass).State = EntityState.Modified;
                fundAssetClass.LastModifiedBy = user;
                fundAssetClass.LastModifiedOn = DateTime.Now;

                //}
                context.SaveChanges();
                return fundAssetClass;
            }
        }

        public IEnumerable<string> GetPermission(string UserName)
        {
            SqlParameter paramFieldGroupName = new SqlParameter("@Username", UserName);
            return _cloContext.Database.SqlQuery<string>("CLO.GetRolesandPermissions @Username", paramFieldGroupName);
        }

        public IEnumerable<TradeHistory> GetTradeHistory(string securityCode)
        {
            SqlParameter paramFieldSecurityCode = new SqlParameter("@securityCode", securityCode);
            //SqlParameter paramFieldPortfolioName = new SqlParameter("@portfolioName", portfolioName);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<TradeHistory>("CLO.spGetTradeHistory @securityCode", paramFieldSecurityCode);
        }

        IEnumerable<vw_Position> IRepository.AddOrUpdatePaydown(Paydown paydown, int dateId)
        {
            try
            {
                var queryPaydownObjectType =
                    (PaydownObjectTypeEnum)Enum.Parse(typeof(PaydownObjectTypeEnum), paydown.PaydownObjectTypeId.ToString());
                var queryObjectId = paydown.PaydownObjectId;

                if (paydown.PaydownId > 0)
                {
                    var paydownToUpdate = _cloContext.Paydowns.Single(w => w.PaydownId == paydown.PaydownId);

                    if (paydownToUpdate.PaydownObjectTypeId == (int)PaydownObjectTypeEnum.Issuer)
                    {
                        queryPaydownObjectType = PaydownObjectTypeEnum.Issuer;
                        queryObjectId = paydownToUpdate.PaydownObjectId;
                    }
                    paydownToUpdate.PaydownObjectTypeId = paydown.PaydownObjectTypeId;
                    paydownToUpdate.PaydownObjectId = paydown.PaydownObjectId;
                    paydownToUpdate.PaydownComments = paydown.PaydownComments;
                }
                else
                {
                    _cloContext.Paydowns.Add(paydown);
                }

                _cloContext.SaveChanges();

                if (queryPaydownObjectType == PaydownObjectTypeEnum.Issuer)
                {
                    return _cloContext.vw_Position.Where(p => p.IssuerId == queryObjectId).ToList();
                }
                else
                {
                    return _cloContext.vw_Position.Where(p => p.SecurityId == queryObjectId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        IEnumerable<vw_Position> IRepository.DeletePaydown(int paydownId, int dateId)
        {
            var paydownToDelete = _cloContext.Paydowns.SingleOrDefault(w => w.PaydownId == paydownId);
            if (paydownToDelete != null)
            {
                var queryPaydownObjectType =
                    (PaydownObjectTypeEnum)
                        Enum.Parse(typeof(PaydownObjectTypeEnum), paydownToDelete.PaydownObjectTypeId.ToString());
                var queryObjectId = paydownToDelete.PaydownObjectId;

                _cloContext.Paydowns.Remove(paydownToDelete);
                _cloContext.SaveChanges();

                if (queryPaydownObjectType == PaydownObjectTypeEnum.Issuer)
                {
                    return _cloContext.vw_Position.Where(p => p.IssuerId == queryObjectId).ToList();
                }
                else
                {
                    return _cloContext.vw_Position.Where(p => p.SecurityId == queryObjectId).ToList();
                }
            }
            else
            {
                return null;
            }
        }

        IEnumerable<vw_Security_Paydown> IRepository.GetSecurityPaydown(int[] securityIds)
        {

            SqlParameter paramSecurityIds = GetSecurityListParam(securityIds);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<vw_Security_Paydown>("CLO.spGetSecurityPaydown @paramSecurityIds", paramSecurityIds);
        }

        IEnumerable<Trader> IRepository.GetTraders()
        {
            return _cloContext.Traders.Where(i => i.TraderId > 0 && i.IsActive == true);
        }
        IEnumerable<TradeType> IRepository.GetTradeType()
        {
            return _cloContext.TradeType.Where(i => i.TradeTypeId > 0 && i.IsActive == true);
        }
        IEnumerable<CounterParty> IRepository.GetCounterParty()
        {
            return _cloContext.CounterParty.Where(i => i.PartyId > 0 && i.IsActive == true).OrderBy(i => i.PartyName);
        }
        IEnumerable<SettleMethods> IRepository.GetSettleMethods()
        {
            return _cloContext.SettleMethods.Where(i => i.MethodId > 0 && i.IsActive == true).OrderByDescending(i => i.MethodId);
        }
        IEnumerable<InterestTreatment> IRepository.GetInterestTreatment()
        {
            return _cloContext.InterestTreatment.Where(i => i.Id > 0 && i.IsActive == true);
        }
        IEnumerable<TradeComment> IRepository.GetTradeComment()
        {
            return _cloContext.TradeComment.Where(c => c.CommentId > 0 && c.IsActive == true).OrderBy(c => c.Comment);
        }

        public IEnumerable<TradeReason> GetTradeReasons()
        {
            return _cloContext.TradeReason.Where(c => c.TradeReasonId > 0 && c.IsActive == true);
        }
        IEnumerable<AllocationRule> IRepository.GetAllocationRule(int tradeTypeId)
        {
            return _cloContext.AllocationRule.Where(i => (i.TradeTypeId == tradeTypeId) && (i.IsActive == true));
        }

        public IEnumerable<TradeBooking> GetTradeBookingXML(int TradeId)
        {
            SqlParameter paramFieldTradeId = new SqlParameter("@TradeId", TradeId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<TradeBooking>("CLO.dbsp_GetTradeBooking_XML @TradeId", paramFieldTradeId);
        }

        public IEnumerable<TradeGroup> GetTradeGroupXML(int TradeId)
        {
            SqlParameter paramFieldTradeId = new SqlParameter("@TradeId", TradeId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<TradeGroup>("CLO.dbsp_GetTradeGroup_XML @TradeId", paramFieldTradeId);
        }

        public IEnumerable<TradeBookingDetail> GetTradeBookingDetailXML(int TradeId)
        {
            SqlParameter paramFieldTradeId = new SqlParameter("@TradeId", TradeId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<TradeBookingDetail>("CLO.dbsp_GetTradeBookingDetail_XML @TradeId", paramFieldTradeId);
        }

        IEnumerable<vw_IssuerSecurity> IRepository.SearchIssuerSecurities()
        {
            return _cloContext.vw_IssuerSecurity.SqlQuery("CLO.dbsp_SearchIssuerSecurity").ToList();
        }

        IEnumerable<vw_PositionIssuers> IRepository.GetIssuerList()
        {
            return _cloContext.vw_PositionIssuers.SqlQuery("CLO.dbsp_GetIssuerList").ToList();
        }

        IEnumerable<TradeBooking> IRepository.GetTradeBookings()
        {
            return _cloContext.Database.SqlQuery<TradeBooking>("CLO.dbsp_GetTradeBooking");
        }

        IEnumerable<TradeBooking> IRepository.GetTradeBookingHistory()
        {
            return _cloContext.Database.SqlQuery<TradeBooking>("CLO.dbsp_GetTradeBookingHistory");
        }
        IEnumerable<TradeBooking> IRepository.GetFilteredTrades(DateTime StartDate, DateTime EndDate)
        {
            SqlParameter paramFieldStartDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter paramFielEndDate = new SqlParameter("@EndDate", EndDate);
            return _cloContext.Database.SqlQuery<TradeBooking>("CLO.dbsp_GetFilteredTrades @StartDate,@EndDate", paramFieldStartDate, paramFielEndDate);
        }



        TradeBooking IRepository.RefreshTradeBooking(long TradeId)
        {
            SqlParameter paramFieldTradeId = new SqlParameter("@TradeId", TradeId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.TradeBooking.SqlQuery("CLO.dbsp_RefreshTradeBooking @TradeId", paramFieldTradeId).FirstOrDefault();
        }

        IEnumerable<TradeBookingDetail> IRepository.RefreshTradeBookingDetail(long TradeId)
        {
            SqlParameter paramFieldTradeId = new SqlParameter("@TradeId", TradeId);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<TradeBookingDetail>("CLO.dbsp_RefreshTradeBookingDetail  @TradeId", paramFieldTradeId);
        }

        int IRepository.SaveTradeBooking(TradeBooking tradebook, string user)
        {
            int intTradeBookingId = 0;
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandtradebooking = new SqlCommand("CLO.dbsp_InsertTradeBooking", connection))
                    {
                        commandtradebooking.CommandType = CommandType.StoredProcedure;
                        commandtradebooking.Parameters.Add(new SqlParameter("@TradeDate", tradebook.TradeDate));
                        commandtradebooking.Parameters.Add(new SqlParameter("@TradeTypeId", tradebook.TradeTypeId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@TraderId", tradebook.TraderId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@LoanXId", tradebook.LoanXId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@IssuerId", tradebook.IssuerId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@IssuerDesc", tradebook.IssuerDesc));
                        commandtradebooking.Parameters.Add(new SqlParameter("@FacilityId", tradebook.FacilityId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@CounterPartyId", tradebook.CounterPartyId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@SettleMethodId", tradebook.SettleMethodId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@InterestTreatmentId", tradebook.InterestTreatmentId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@Price", tradebook.Price));
                        commandtradebooking.Parameters.Add(new SqlParameter("@TotalQty", tradebook.TotalQty));
                        commandtradebooking.Parameters.Add(new SqlParameter("@RuleId", tradebook.RuleId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@TradeCommentId1", tradebook.TradeCommentId1));
                        commandtradebooking.Parameters.Add(new SqlParameter("@TradeCommentId2", tradebook.TradeCommentId2));
                        commandtradebooking.Parameters.Add(new SqlParameter("@TradeComment", tradebook.TradeComment));
                        commandtradebooking.Parameters.Add(new SqlParameter("@TradeReasonId", tradebook.TradeReasonId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@AssetTypeId", tradebook.AssetId));
                        commandtradebooking.Parameters.Add(new SqlParameter("@Cancel", false));
                        commandtradebooking.Parameters.Add(new SqlParameter("@UpdateFlag", false));
                        commandtradebooking.Parameters.Add(new SqlParameter("@Id", DbType.Int64) { Direction = ParameterDirection.Output });
                        commandtradebooking.ExecuteNonQuery();
                        intTradeBookingId = (int)commandtradebooking.Parameters["@Id"].Value;
                    }
                    connection.Close();
                }
            }
            return intTradeBookingId;
        }

        bool IRepository.SaveTradeBookingDetails(IEnumerable<TradeBookingDetail> tradebookdetail, long TradeId)
        {
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    foreach (var traderow in tradebookdetail)
                    {
                        using (SqlCommand commandtradebooking = new SqlCommand("CLO.dbsp_InsertTradeBookingDetail", connection))
                        {
                            commandtradebooking.CommandType = CommandType.StoredProcedure;
                            commandtradebooking.Parameters.Add(new SqlParameter("@TradeId", TradeId));
                            commandtradebooking.Parameters.Add(new SqlParameter("@PortFolioId", traderow.PortFolioId));
                            commandtradebooking.Parameters.Add(new SqlParameter("@Existing", traderow.Existing));
                            commandtradebooking.Parameters.Add(new SqlParameter("@Allocated", traderow.Allocated));
                            commandtradebooking.Parameters.Add(new SqlParameter("@Override", traderow.Override));
                            commandtradebooking.Parameters.Add(new SqlParameter("@FinalQty", traderow.FinalQty));
                            commandtradebooking.Parameters.Add(new SqlParameter("@TradeAmount", traderow.TradeAmount));
                            commandtradebooking.Parameters.Add(new SqlParameter("@IsIncluded", traderow.IsIncluded));
                            commandtradebooking.Parameters.Add(new SqlParameter("@Exposure", traderow.Exposure));
                            commandtradebooking.Parameters.Add(new SqlParameter("@NetPosition", traderow.NetPosition));
                            commandtradebooking.ExecuteNonQuery();
                        }
                    }
                    connection.Close();
                }
            }
            return true;
        }

        bool IRepository.UpdateSubmitDetails(long TradeId)
        {
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandtradebooking = new SqlCommand("CLO.dbsp_UpdateSubmitDetails", connection))
                    {
                        commandtradebooking.CommandType = CommandType.StoredProcedure;
                        commandtradebooking.Parameters.Add(new SqlParameter("@TradeId", TradeId));
                        commandtradebooking.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return true;
        }

        public IEnumerable<TradeBookingDetail> GetTradeFundAllocation(string ruleName, int issuerId, string LoanXId, string tradeType)
        {
            SqlParameter paramruleName = new SqlParameter("@ruleName", ruleName);
            SqlParameter paramissuerId = new SqlParameter("@issuerId", issuerId);
            SqlParameter paramLoanXId = new SqlParameter("@LoanXId", LoanXId);
            SqlParameter paramTradeType = new SqlParameter("@TradeType", tradeType);
            _cloContext.Database.CommandTimeout = timeout_short;
            return _cloContext.Database.SqlQuery<TradeBookingDetail>("CLO.dbsp_GetTradeBookingAllocation @ruleName,@issuerId,@LoanXId,@TradeType", paramruleName, paramissuerId, paramLoanXId, paramTradeType);
        }

        bool IRepository.CancelTradeBooking(long TradeId)
        {
            using (var cloContext = new CLOContext())
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand commandtradebooking = new SqlCommand("CLO.dbsp_UpdCancelTradeBooking", connection))
                    {
                        commandtradebooking.CommandType = CommandType.StoredProcedure;
                        commandtradebooking.Parameters.Add(new SqlParameter("@Id", TradeId));
                        commandtradebooking.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return true;
        }


        IEnumerable<Trends> IRepository.GetTrends(int trendTypeId, DateTime startDate, DateTime endDate, int periodId)
        {
            SqlParameter paramtrendTypeId = new SqlParameter("@TrendTypeId", trendTypeId);
            SqlParameter paramFieldStartDate = new SqlParameter("@StartDate", startDate);
            SqlParameter paramFielEndDate = new SqlParameter("@EndDate", endDate);
            SqlParameter paramPeriodId = new SqlParameter("@PeriodId", periodId);
            return _cloContext.Database.SqlQuery<Trends>("CLO.dbsp_GetTrends @TrendTypeId,@StartDate,@EndDate,@PeriodId", paramtrendTypeId, paramFieldStartDate, paramFielEndDate, paramPeriodId);
        }

        IEnumerable<TrendType> IRepository.GetTrendTypes()
        {
            return _cloContext.Database.SqlQuery<TrendType>("CLO.usp_GetTrendingTypes");
        }

        IEnumerable<TrendPeriod> IRepository.GetTrendPeriod()
        {
            return _cloContext.Database.SqlQuery<TrendPeriod>("CLO.usp_GetTrendPeriod");
        }

        List<List<DataExceptionObject>> IRepository.GetDataExceptionReporting()
        {
            List<List<DataExceptionObject>> bigObj = new List<List<DataExceptionObject>>();
            var command = _cloContext.Database.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "CLO.uspGetDataExceptionReport";

            try
            {
                _cloContext.Database.Connection.Open();
                //var reader = command.ExecuteReader();

                using (var reader = command.ExecuteReader())
                {
                    do
                    {
                        List<DataExceptionObject> obj = ((IObjectContextAdapter)_cloContext).ObjectContext.Translate<DataExceptionObject>(reader).ToList();
                        bigObj.Add(obj);
                    }
                    while (reader.NextResult());
                }
                return bigObj;
            }
            finally
            {
                _cloContext.Database.Connection.Close();
            }

        }
    }
}

