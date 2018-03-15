using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BW.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BW.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        [StringLength(25)]
        public string Surname { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string ActivationCode { get; set; }
        public string PhotoURL { get; set; }
        [NotMapped]
        public HttpPostedFileBase Photo { get; set; }
        public virtual List<Article> Articles { get; set; } = new List<Article>();
    }
}
