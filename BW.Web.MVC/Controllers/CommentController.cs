using System.Threading.Tasks;
using BW.Models.ViewModels;
using System.Web.Mvc;
using BW.BLL.Repository;
using BW.Models.Entities;

namespace BW.Web.MVC.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(ArticleCommentMultiViewModel model)
        {
            var newComment = new Comment()
            {
                CommentContent = model.NewComment.CommentContent,
                UserId =model.CommenterId,
                ArticleId = model.Article.ArticleId
            };
            await new Repository.CommentRepo().InsertAsync(newComment);
            return RedirectToAction("Single","Article",new{id=model.Article.ArticleId,route=model.Article.Header.Replace(' ','-')});
        }
    }
}