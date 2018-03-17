using System;
using System.Collections.Generic;
using System.Linq;
using BW.BLL.Repository;
using BW.Models.Entities;
using BW.Models.IdentityModels;
using BW.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BW.BLL.Account.MembershipTools;

namespace BlogWebsite.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public async Task<ActionResult> Index(int id)
        {

            try
            {
                var article = await new Repository.ArticleRepo().GetByIdAsync(id);
                if (article == null)
                    return RedirectToAction("Index", "Home");
                var comments = new Repository.CommentRepo().Queryable().Where(x => x.ArticleId == id).OrderByDescending(x => x.CommentDate).ToList();
                var model = new ArticleCommentMultiViewModel()
                {
                    Article = article,
                    Comments = comments
                };
                var popular = new Repository.ArticleRepo().Queryable().OrderByDescending(x => x.Likes).ToList();
                ViewBag.popular = popular;
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize(Roles = "Admin,Editor,User")]
        public ActionResult Insert()
        {
            var popular = new Repository.ArticleRepo().Queryable().OrderByDescending(x => x.Likes).ToList();
            ViewBag.popular = popular;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(ArticleViewModel model)
        {
            var userStore = NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = await userManager.FindByIdAsync(HttpContext.User.Identity.GetUserId());
            var author = user.Name + " " + user.Surname;
            var newArticle = new Article()
            {
                Content = model.Content,
                Header = model.Header,
                Author = author,
                Keywords = model.Keywords,
                UserId = HttpContext.User.Identity.GetUserId()
            };
            var sonuc = await new Repository.ArticleRepo().InsertAsync(newArticle);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetByKeywords(string search)
        {
            try
            {
                var popular = new Repository.ArticleRepo().Queryable().OrderByDescending(x => x.Likes).ToList();
                ViewBag.popular = popular;
                if (search == null)
                    return RedirectToAction("Index", "Home");
                var articles = await new Repository.ArticleRepo().GetAllAsync();
                var model = new List<Article>();
                foreach (var item in articles)
                {
                    if (item.Keywords == null)
                        continue;
                    if (item.Keywords.Split(';').Contains(search))
                    {
                        model.Add(item);
                    }
                }
                if (model.Count == 0)
                    return RedirectToAction("Index", "Home");
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult ConfirmArticles()
        {
            try
            {
                var popular = new Repository.ArticleRepo().Queryable().OrderByDescending(x => x.Likes).ToList();
                ViewBag.popular = popular;
                var model = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == false).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult> ConfirmArticle(int id)
        {
            try
            {
                var popular = new Repository.ArticleRepo().Queryable().OrderByDescending(x => x.Likes).ToList();
                ViewBag.popular = popular;
                var userStore = NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);
                var admin = await userManager.FindByIdAsync(HttpContext.User.Identity.GetUserId());
                var article = await new Repository.ArticleRepo().GetByIdAsync(id);
                article.Confirmed = true;
                article.ConfirmedBy = admin.Name + " " + admin.Surname;
                await new Repository.ArticleRepo().UpdateAsync();
                return RedirectToAction("ConfirmArticles", "Article");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("ConfirmArticles", "Article");
            }
        }
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = await new Repository.ArticleRepo().GetByIdAsync(id);
                await new Repository.ArticleRepo().DeleteAsync(model);
                return RedirectToAction("ConfirmArticles", "Article");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("ConfirmArticles", "Article");
            }
        }
    }
}