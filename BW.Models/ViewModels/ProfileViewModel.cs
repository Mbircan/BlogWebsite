using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BW.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; }
    }
}
