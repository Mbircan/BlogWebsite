using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BW.BLL.Account;
using BW.Models.IdentityModels;
using Microsoft.AspNet.Identity;

namespace BlogWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var roleManager = MembershipTools.NewRoleManager();
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Admin",
                    Description = "Yönetici"
                });
            }
            if (!roleManager.RoleExists("Editor"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Editor",
                    Description = "Editör"
                });
            }
            if (!roleManager.RoleExists("User"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "User",
                    Description = "Kullanıcı"
                });
            }
            if (!roleManager.RoleExists("Passive"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Passive",
                    Description = "Aktivasyon Yapmamış Kullanıcı Rolü"
                });
            }
        }
    }
}
