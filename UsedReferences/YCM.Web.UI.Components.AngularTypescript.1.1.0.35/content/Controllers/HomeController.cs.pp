using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $rootnamespace$.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
    }
}