using BW.Models.Entities;

namespace BW.BLL.Repository
{
    public class Repository
    {
        public class ArticleRepo : RepositoryBase<Article, int> { }
        public class CommentRepo : RepositoryBase<Comment, int> { }
        public class LikeRepo : RepositoryBase<Like, int> { }
        public class CategoryRepo : RepositoryBase<Category, int> { }
    }
}
