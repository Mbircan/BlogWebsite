﻿using System;
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

namespace BW.Web.MVC.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        [Route("{route}")]
        public async Task<ActionResult> Index(int id, string route)
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
                var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
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
            var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
            ViewBag.popular = popular;
            var kategorilist = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Kategori Seçiniz",
                    Value = null
                }

            };
            foreach (var category in new Repository.CategoryRepo().GetAll())
            {
                kategorilist.Add(new SelectListItem()
                {
                    Text = category.CategoryName,
                    Value = category.CategoryId.ToString()
                });
            }
            ViewBag.kategorilist = kategorilist;
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
                CategoryId = model.CategoryId,
                UserId = HttpContext.User.Identity.GetUserId()
            };
            await new Repository.ArticleRepo().InsertAsync(newArticle);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetByKeywords(string search)
        {
            try
            {
                var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
                ViewBag.popular = popular;
                if (search == null)
                    return RedirectToAction("Index", "Home");
                var model = new Repository.ArticleRepo()
                    .Queryable()
                    .Where(x => x.Header.Contains(search)
                             || x.User.UserName.Contains(search)
                             || x.User.Surname.Contains(search)
                             || x.Content.Contains(search)
                             || x.Category.CategoryName.Contains(search)).ToList();
                if(model.Count==0)
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
                var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
                ViewBag.popular = popular;
                var model = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == false).OrderByDescending(x=>x.AddDate).ToList();
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
                var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
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

        public async Task<ActionResult> Like(int id, string uid)
        {
            try
            {
                var exists = new Repository.LikeRepo().Queryable().Where(x => x.UserId == uid && x.ArticleId == id)
                    .Any();
                var article = await new Repository.ArticleRepo().GetByIdAsync(id);
                var like = new Like()
                {
                    ArticleId = id,
                    UserId = uid
                };
                if (!exists)
                {
                    await new Repository.LikeRepo().InsertAsync(like);
                    article.LikeCount++;
                    await new Repository.ArticleRepo().UpdateAsync();
                }             
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<ActionResult> Dislike(int id, string uid)
        {
            try
            {
                var exists = new Repository.LikeRepo().Queryable().Where(x => x.UserId == uid && x.ArticleId == id)
                    .Any();
                var article = await new Repository.ArticleRepo().GetByIdAsync(id);
                var like = new Repository.LikeRepo().Queryable().Where(x => x.UserId == uid && x.ArticleId == id)
                    .FirstOrDefault();
                if (exists)
                {
                    await new Repository.LikeRepo().DeleteAsync(like);
                    article.LikeCount--;
                    await new Repository.ArticleRepo().UpdateAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}