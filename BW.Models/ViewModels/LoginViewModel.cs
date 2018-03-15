using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BW.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifreniz En az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
