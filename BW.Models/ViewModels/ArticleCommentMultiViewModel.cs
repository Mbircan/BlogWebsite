using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BW.Models.Entities;

namespace BW.Models.ViewModels
{
    public class ArticleCommentMultiViewModel
    {
        public Article Article { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment NewComment { get; set; }
    }
}
