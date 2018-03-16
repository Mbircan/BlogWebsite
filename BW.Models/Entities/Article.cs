﻿using System;
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
        public string Header { get; set; }
        [Key]
        public int ArticleId { get; set; }
        public string Author { get; set; }
        public bool Confirmed { get; set; } = false;
        public string ConfirmedBy { get; set; }
        public string Keywords { get; set; }
        public int Likes { get; set; } = 0;
        public DateTime? AddDate { get; set; }=DateTime.Now;
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual List<Comment> Comments { get; set; }=new List<Comment>();

    }
}
