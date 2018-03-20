using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BW.BLL.Repository;

namespace BW.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int id=1)
        {
            var pageSize = 10;
            var model = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x=>x.AddDate).Skip((id - 1)*pageSize).Take(pageSize).ToList();
            var popular = new Repository.ArticleRepo().Queryable().Where(x=>x.Confirmed==true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
            ViewBag.popular = popular;
            var count = new Repository.ArticleRepo().GetAll().Count;
            ViewBag.count = count;
            return View(model);
        }

        public ActionResult Contact()
        {
            var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
            ViewBag.popular = popular;
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