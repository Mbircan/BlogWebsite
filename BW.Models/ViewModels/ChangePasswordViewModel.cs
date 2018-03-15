using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BW.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Eski Şifre")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
    }
}
