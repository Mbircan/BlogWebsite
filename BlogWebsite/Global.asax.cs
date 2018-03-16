using BlogWebsite.App_Start;
using BW.BLL.Account;
using BW.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BlogWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundle(BundleTable.Bundles);

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
