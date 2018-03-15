using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BW.Models.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
    }
}
