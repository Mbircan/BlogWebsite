using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BW.Models.ViewModels
{
    public class ArticleViewModel
    {      
        public string Content { get; set; }
        public string Header { get; set; }
        public string UserId { get; set; }       
        public int ArticleId { get; set; }
        public int CategoryId { get; set; }
        public int Likes { get; set; }
    }
}
