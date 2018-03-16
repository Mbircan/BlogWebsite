using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BW.BLL.Repository;

namespace BlogWebsite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var model = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).ToList();
            var popular = new Repository.ArticleRepo().Queryable().OrderByDescending(x => x.Likes).ToList();
            ViewBag.popular = popular;
            return View(model);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult AboutUs()
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