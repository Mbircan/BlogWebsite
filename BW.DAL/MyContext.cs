using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BW.Models.Entities;
using BW.Models.IdentityModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BW.DAL
{
    public class MyContext : IdentityDbContext<ApplicationUser>
    {
        public MyContext() : base("name=MyCon")
        {
        }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Like> Likes{ get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
