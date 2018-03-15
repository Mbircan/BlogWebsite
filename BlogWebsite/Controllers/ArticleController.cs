using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BW.BLL.Repository;
using BW.Models.Entities;
using BW.Models.ViewModels;

namespace BlogWebsite.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Insert(ArticleViewModel model)
        {
            var newArticle = new Article()
            {
                Content = model.Content,
                Author = model.Author,
                Keywords = model.Keywords
            };
            var sonuc = await new Repository.ArticleRepo().InsertAsync(newArticle);
            return RedirectToAction("Index", "Home");
        }
    }
}