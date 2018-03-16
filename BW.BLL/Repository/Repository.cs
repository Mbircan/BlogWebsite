using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BW.Models.Entities;

namespace BW.BLL.Repository
{
    public class Repository
    {
        public class ArticleRepo : RepositoryBase<Article, int> { }
        public class CommentRepo : RepositoryBase<Comment, int> { }
    }
}
