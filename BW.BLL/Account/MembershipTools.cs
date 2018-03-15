using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BW.DAL;
using BW.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BW.BLL.Account
{
    public class MembershipTools
    {
        public static UserStore<ApplicationUser> NewUserStore()
        {
            return new UserStore<ApplicationUser>(new MyContext());
        }

        public static UserManager<ApplicationUser> NewUserManager()
        {
            return new UserManager<ApplicationUser>(NewUserStore());
        }
        public static RoleStore<ApplicationRole> NewRoleStore()
        {
            return new RoleStore<ApplicationRole>(new MyContext());
        }

        public static RoleManager<ApplicationRole> NewRoleManager()
        {
            return new RoleManager<ApplicationRole>(NewRoleStore());
        }
    }
}
