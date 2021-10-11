using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private static readonly ILog _logger;


        readonly IRepository _repository;
        private readonly IRuleEngine _ruleEngine;

        //public HomeController() : this(new Repository(), new RuleEngine())
        //{

        //}

        public HomeController(IRepository repository, IRuleEngine ruleEngine)
        {
            _repository = repository;
            _ruleEngine = ruleEngine;
        }

        [Authorize]
        public ActionResult Index()
        {

            /* Commented by Diwakar on 10-Jun
            var roles = GetRoles(User.Identity.Name);
            for (int i = 0; i < roles.Length; i++)
            {
                _logger.Info("Role=" + roles[i]);
            }
            
            if (!roles.Contains("CLO"))
            {
                Response.Redirect(ConfigurationManager.AppSettings["BaseWebPath"] + "/error/restrictedusers?userId=" + User.Identity.Name + "&app=" + "CLO");
                return View();
            }
            
            
            
            var permissions = GetPermissions(User.Identity.Name);
            */
            #region  application permissions
            //var permissions = 1;
            //_logger.Info($"permissions: {permissions}");
            _logger.Info("Logged In User Name:" + System.Web.HttpContext.Current.User.Identity.Name);
            var permissions = GetRolesPermissions(User.Identity.Name);
            _logger.Info($"User {User.Identity.Name} has permissions to {string.Join(",", permissions.ToArray())}");

            ViewBag.HasPermission = true;
            ViewBag.HasPositionScreenPermission = false;
            ViewBag.HasTop10Bottom10Permission = false;
            ViewBag.HasTradePermission = false;
            ViewBag.HasTradeSwapPermission = false;
            ViewBag.HasAnalystResearchPermission = false;
            ViewBag.HasBidOfferUploadPermission = false;
            ViewBag.HasFundTriggersPermission = false;
            ViewBag.HasFundOverridesPermission = false;
            ViewBag.HasParametersPermission = false;
            ViewBag.HasLoanAttributeOverridePermission = false;
            ViewBag.HasNewLoanReconPermission = false;
            ViewBag.HasLoanAttributeOverridesReconPermission = false;
            ViewBag.HasAdminPermission = false;
            ViewBag.HasReportEditPermission = false;

            if (permissions.Count > 0)
            {
                ViewBag.HasPermission = true;
                foreach (var item in permissions.OrderBy(i => i))
                {

                    switch (item)
                    {
                        case "Admin":
                            ViewBag.HasAdminPermission = true;
                            break;
                        case "BidOfferUpload":
                            if(ViewBag.HasAdminPermission)
                                ViewBag.HasBidOfferUploadPermission = true;
                            break;
                        case "CollateralQualityMatrix":
                            if (ViewBag.HasAdminPermission)
                                ViewBag.HasBidOfferUploadPermission = true;
                            break;
                        case "FundsOverrides":
                            if (ViewBag.HasAdminPermission)
                                ViewBag.HasFundOverridesPermission = true;
                            break;
                        case "FundTriggers":
                            if (ViewBag.HasAdminPermission)
                                ViewBag.HasFundTriggersPermission = true;
                            break;
                        case "LOanAttributeOverrideRecon":
                            if (ViewBag.HasAdminPermission)
                                ViewBag.HasLoanAttributeOverridesReconPermission = true;
                            break;
                        case "LoanOverrideAttributes":
                            if (ViewBag.HasAdminPermission)
                                ViewBag.HasLoanAttributeOverridePermission = true;
                            break;
                        case "NewLoanRecon":
                            if (ViewBag.HasAdminPermission)
                                ViewBag.HasNewLoanReconPermission = true;
                            break;
                        case "PArameters":
                            if (ViewBag.HasAdminPermission)
                                ViewBag.HasParametersPermission = true;
                            break;
                        case "Positions":
                            ViewBag.HasPositionScreenPermission = true;
                            break;
                        case "Reporting":
                            ViewBag.HasReportEditPermission = true;
                            break;
                        case "Top10Bottom10":
                            ViewBag.HasTop10Bottom10Permission = true;
                            break;
                        case "Trades":
                            ViewBag.HasTradePermission = true;
                            break;
                        case "TradesSwapping":
                            ViewBag.HasTradeSwapPermission = true;
                            break;
                    }
                }
            }
            //else
            //{
            //    ViewBag.HasPermission = false;
            //}

            //ViewBag.HasPermission = permissions != 0 && (permissions & (int)CLOEntitlements.ApplicationAccess) == (int)CLOEntitlements.ApplicationAccess;
            //ViewBag.HasPositionScreenPermission = permissions != 0 && (permissions & (int)CLOEntitlements.Position) == (int)CLOEntitlements.Position;
            //ViewBag.HasTop10Bottom10Permission = permissions != 0 && (permissions & (int)CLOEntitlements.Top10Bottom10) == (int)CLOEntitlements.Top10Bottom10;
            //ViewBag.HasTradePermission = permissions != 0 && (permissions & (int)CLOEntitlements.Trade) == (int)CLOEntitlements.Trade;
            //ViewBag.HasTradeSwapPermission = permissions != 0 && (permissions & (int)CLOEntitlements.TradeSwapping) == (int)CLOEntitlements.TradeSwapping;
            //ViewBag.HasAnalystResearchPermission = permissions != 0 && (permissions & (int)CLOEntitlements.AnalystResearch) == (int)CLOEntitlements.AnalystResearch;
            //ViewBag.HasBidOfferUploadPermission = permissions != 0 && (permissions & (int)CLOEntitlements.BidOfferUpload) == (int)CLOEntitlements.BidOfferUpload;
            //ViewBag.HasFundTriggersPermission = permissions != 0 && (permissions & (int)CLOEntitlements.FundTriggers) == (int)CLOEntitlements.FundTriggers;
            //ViewBag.HasFundOverridesPermission = permissions != 0 && (permissions & (int)CLOEntitlements.FundOverrides) == (int)CLOEntitlements.FundOverrides;
            //ViewBag.HasParametersPermission = permissions != 0 && (permissions & (int)CLOEntitlements.Parameters) == (int)CLOEntitlements.Parameters;
            //ViewBag.HasLoanAttributeOverridePermission = permissions != 0 && (permissions & (int)CLOEntitlements.LoanAttributeOverride) == (int)CLOEntitlements.LoanAttributeOverride;
            //ViewBag.HasNewLoanReconPermission = permissions != 0 && (permissions & (int)CLOEntitlements.NewLoanRecon) == (int)CLOEntitlements.NewLoanRecon;
            //ViewBag.HasLoanAttributeOverridesReconPermission = permissions != 0 && (permissions & (int)CLOEntitlements.LoanAttributeOverridesRecon) == (int)CLOEntitlements.LoanAttributeOverridesRecon;
            //ViewBag.HasAdminPermission = permissions != 0 && (permissions & (int)CLOEntitlements.Admin) == (int)CLOEntitlements.Admin;




            //ViewBag.HasReportEditPermission = roles.Contains("CLO-CanEditReportData");

            //ViewBag.HasPermission = true;
            //ViewBag.HasPositionScreenPermission = true;
            //ViewBag.HasTop10Bottom10Permission = false;
            //ViewBag.HasTradePermission = true;
            //ViewBag.HasTradeSwapPermission = true;
            ViewBag.HasAnalystResearchPermission = true;
            //ViewBag.HasBidOfferUploadPermission = true;
            //ViewBag.HasFundTriggersPermission = true;
            //ViewBag.HasFundOverridesPermission = true;
            //ViewBag.HasParametersPermission = true;
            //ViewBag.HasLoanAttributeOverridePermission = true;
            //ViewBag.HasNewLoanReconPermission = true;
            //ViewBag.HasLoanAttributeOverridesReconPermission = true;
            //ViewBag.HasAdminPermission = true;
            //ViewBag.HasReportEditPermission = true;

            #endregion  application permissions

            return View();
        }

            public  List<string> GetRolesPermissions(string name)
            {
                name = System.Web.HttpContext.Current.User.Identity.Name;
                return _repository.GetPermission(name.Substring(name.IndexOf("\\") + 1).ToLower()).ToList();
            }

            public static string[] GetRoles(string userName)
            {
                _logger.Info($"Getting permissions for: {userName ?? "null"}");
                using (var client = new WebClient())
                {
                    var baseWebPath = ConfigurationManager.AppSettings["BaseWebPath"];
                    _logger.Info($"Base Web Path: {baseWebPath ?? "null"}");

                    var permissions = client.DownloadString(string.Concat(baseWebPath
                        , "/Entitlement/GetUserRoles?userid=", userName
                    ));

                    return permissions.Replace("\"", "").Split(new char[] { ',' });
                }
            }

            [AllowAnonymous]
            public ViewResult TopBottomPriceMovers()
            {
                int fromDateId = Helper.GetPrevDayDateId();
                int toDateId;

                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                {
                    toDateId = Helper.GetDateId(Helper.GetPrevBusinessDay(3));
                }
                else
                {
                    toDateId = Helper.GetDateId(Helper.GetPrevBusinessDay(1));
                }

                var funds = _repository.GetFunds();

                DateTime Prev5BusinessDay = new DateTime();
                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                {
                    Prev5BusinessDay = Helper.GetPrevBusinessDay(7);
                }
                else
                {
                    Prev5BusinessDay = Helper.GetPrevBusinessDay(5);
                }

                int to5DateId = Helper.GetDateId(Prev5BusinessDay);

                ViewBag.PrevDayTop = _repository.GetPriceMove("Top", fromDateId, toDateId);
                ViewBag.PrevDayBottom = _repository.GetPriceMove("Bottom", fromDateId, toDateId);
                ViewBag.IsStale = funds.Any(f => ((f.IsStale.HasValue && f.IsStale.Value) || (f.IsPrincipalCashStale.HasValue && f.IsPrincipalCashStale.Value)));

                ViewBag.Prev5BusinessDay = Prev5BusinessDay.ToShortDateString();
                ViewBag.Prev5DayTop = _repository.GetPriceMove("Top", fromDateId, to5DateId);
                ViewBag.Prev5DayBottom = _repository.GetPriceMove("Bottom", fromDateId, to5DateId);

                return View();
            }

            [AllowAnonymous]
            public ViewResult RatingChanges(int? startDateId = null, int? endDateId = null)
            {
                if (startDateId == null)
                {
                    var today = DateTime.Today;

                    if (today.DayOfWeek == DayOfWeek.Monday)
                    {
                        startDateId = Helper.GetDateId(today.AddDays(-4));
                    }
                    else
                    {
                        startDateId = Helper.GetDateId(today.AddDays(-2));
                    }
                }

                if (endDateId == null)
                {
                    var today = DateTime.Today;
                    endDateId = Helper.GetDateId(today.AddDays(-1));
                }

                var ratingChanges = _repository.GetRatingChanges(
                    startDateId.Value, endDateId.Value)
                    .OrderBy(rc => rc.Issuer)
                    .ToList();

                return View(ratingChanges);
            }


        [AllowAnonymous]
        public ViewResult totalParChanges(int? startDateId = null, int? endDateId = null)
        {
            if (startDateId == null)
            {
                var today = DateTime.Today;

                if (today.DayOfWeek == DayOfWeek.Monday)
                {
                    startDateId = Helper.GetDateId(today.AddDays(-4));
                }
                else
                {
                    startDateId = Helper.GetDateId(today.AddDays(-2));
                }
            }

            if (endDateId == null)
            {
                var today = DateTime.Today;
                endDateId = Helper.GetDateId(today.AddDays(-1));
            }

            var totalParChanges = _repository.GetTotalParChange(
                startDateId.Value, endDateId.Value)
                .OrderBy(rc => rc.Fund)
                .ToList();

            return View(totalParChanges);
        }

        [AllowAnonymous]
            public ViewResult MisMatchData()
            {
                MismatchData mismatchData = new MismatchData();
                mismatchData.Cfrs = _repository.GetMismatchData(13)
                    .OrderBy(rc => rc.SecurityCode)
                    .ToList();

                mismatchData.CfrAdjs = _repository.GetMismatchData(14)
                    .OrderBy(rc => rc.SecurityCode)
                    .ToList();

                mismatchData.Facilities = _repository.GetMismatchData(15)
                    .OrderBy(rc => rc.SecurityCode)
                    .ToList();

                if (mismatchData.CfrAdjs.Count >= 0 || mismatchData.Cfrs.Count >= 0 || mismatchData.Facilities.Count >= 0)
                {
                    return View(mismatchData);
                }
                else
                {
                    throw new Exception("No Mismatch, hence can't show anything.");
                }
            }



            [AllowAnonymous]
            public ViewResult FundStaleStatus()
            {
                ViewBag.Funds = _repository.GetFunds();
                return View();
            }

            static HomeController()
            {
                _logger = LogManager.GetLogger(typeof(HomeController));
                PositionCacheManager positionCacheManager = new PositionCacheManager();
                positionCacheManager.Check();
            }

        private static string _currentVersion;
        public static string GetCurrentVersion()
        {
            if (string.IsNullOrEmpty(_currentVersion))
            {
                _currentVersion = typeof(HomeController).Assembly.GetName().Version.ToString();
            }
            return _currentVersion;
        }


        public static int GetPermissions(string userName)
        {
            _logger.Info($"Getting permissions for: {userName ?? "null"}");
            using (var client = new WebClient())
            {
                var baseWebPath = ConfigurationManager.AppSettings["BaseWebPath"];
                _logger.Info($"Base Web Path: {baseWebPath ?? "null"}");

                var domainIndex = userName.IndexOf('\\');
                var permissions = int.Parse(client.DownloadString(string.Concat(baseWebPath
                    , "/Entitlement/GetUserPermission?userid=", domainIndex == -1 ? userName : userName.Substring(domainIndex + 1)
                    , "&appname=clo"
                )));
                _logger.Info($"Successfully retrieved permissions: {permissions}");

                return permissions;
            }
        }



    }
}