using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BW.Models.IdentityModels;

namespace BW.Models.Entities
{
    public class Article
    {
        public string Content { get; set; }
        public string Head { get; set; }
        [Key]
        public int ArticleID { get; set; }
        public string Author { get; set; }
        public bool Confirmed { get; set; } = false;
        public string ConfirmedBy { get; set; }
        public string Keywords { get; set; }
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

    }
}
