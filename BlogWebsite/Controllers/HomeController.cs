using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogWebsite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult HeaderPartialViewResult()
        {
            return PartialView("_HeaderPartialView");
        }
        public PartialViewResult MainContentPartialViewResult()
        {
            return PartialView("_MainContentPartialView");
        }
        public PartialViewResult EndMainContentPartialViewResult()
        {
            return PartialView("_EndMainContentPartialView");
        }
    }
}