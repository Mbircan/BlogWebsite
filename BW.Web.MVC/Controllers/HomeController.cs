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
        public ActionResult Index(int page=1)
        {
            var pageSize = 5;
            var model = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x=>x.AddDate).Skip((page - 1)*pageSize).Take(pageSize).ToList();
            var count = new Repository.ArticleRepo().GetAll().Count;
            ViewBag.count = count;
            return View(model);
        }

        public ActionResult Contact()
        {
            return View();
        }
        
    }
}