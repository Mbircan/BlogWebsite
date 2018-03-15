using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BW.Models.ViewModels
{
    public class ProfilePasswordMultiViewModel
    {
        public ProfileViewModel ProfileViewModel { get; set; } = new ProfileViewModel();
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; } = new ChangePasswordViewModel();
    }
}
