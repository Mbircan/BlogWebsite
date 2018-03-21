using BW.BLL.Account;
using BW.BLL.Repository;
using BW.BLL.Settings;
using BW.Models.IdentityModels;
using BW.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BW.Web.MVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userManager = MembershipTools.NewUserManager();
            var checkUser = userManager.FindByName(model.UserName);
            if (checkUser != null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten alınmış.");
                return View(model);
            }
            checkUser = userManager.FindByEmail(model.Email);
            if (checkUser != null)
            {
                ModelState.AddModelError("", "Bu e-mail zaten alınmış.");
                return View(model);
            }

            var activationCode = Guid.NewGuid().ToString().Replace("-", "");
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                Name = model.Name,
                Surname = model.Surname,
                ActivationCode = activationCode,
                PhotoURL = "/images/user.png"
            };
            var result = userManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, userManager.Users.Count() == 1 ? "Admin" : "Passive");
                var siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
                              (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                if (userManager.Users.Count() > 1)
                {
                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "Murat Bircan Blog - Aktivasyon",
                        Message = $"Merhaba {user.Name} {user.Surname}</br> Hesabınızı aktifleştirmek için <b><a href='{siteUrl}/Account/Activation?code={activationCode}&u={user.Id}'>buraya</a></b> tıklayınız."

                    });
                }
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Kayıt işleminde bir hata oluştu.");
            return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userManager = MembershipTools.NewUserManager();
            var user = await userManager.FindAsync(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View();
            }
            var authManager = HttpContext.GetOwinContext().Authentication;
            var userIdentity =
                await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignIn(new AuthenticationProperties()
            {
                IsPersistent = model.RememberMe
            }, userIdentity);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
            ViewBag.popular = popular;
            var userManeger = MembershipTools.NewUserManager();
            var user = await userManeger.FindByIdAsync(HttpContext.User.Identity.GetUserId());
            var model = new ProfilePasswordMultiViewModel();
            var modelProfile = new ProfileViewModel()
            {
                Email = user.Email,
                Name = user.Name,
                Bio = user.Bio,
                PhotoURL = user.PhotoURL,
                Surname = user.Surname,
                UserName = user.UserName,
                RegisterDate = user.RegisterDate
            };
            model.ProfileViewModel = modelProfile;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(ProfilePasswordMultiViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var userStore = MembershipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = await userManager.FindByIdAsync(HttpContext.User.Identity.GetUserId());
                user.Name = model.ProfileViewModel.Name;
                user.Surname = model.ProfileViewModel.Surname;
                user.Bio = model.ProfileViewModel.Bio;
                user.Email = model.ProfileViewModel.Email;
                if (model.ProfileViewModel.Photo != null && model.ProfileViewModel.Photo.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.ProfileViewModel.Photo.FileName);
                    string extensionName = Path.GetExtension(model.ProfileViewModel.Photo.FileName);
                    fileName = SiteSettings.UrlFormatConverter(fileName);
                    fileName += Guid.NewGuid().ToString().Replace("-", "");
                    var directoryPath = Server.MapPath("~/Uploads/ProfilePhotos/");
                    var filePath = Server.MapPath("~/Uploads/ProfilePhotos/" + fileName + extensionName);
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    model.ProfileViewModel.Photo.SaveAs(filePath);
                    WebImage img = new WebImage(filePath);
                    img.Resize(800, 800, false);
                    img.AddTextWatermark("Murat Bircan Blog", fontColor: "antiquewhite", fontSize: 18, fontFamily: "Verdana");
                    img.Save(filePath);
                    if (!string.IsNullOrEmpty(user.PhotoURL) && user.PhotoURL != "/images/user.png")
                        System.IO.File.Delete(Server.MapPath(user.PhotoURL));
                    user.PhotoURL = $@"/Uploads/ProfilePhotos/{fileName}{extensionName}";
                }
                await userStore.UpdateAsync(user);
                await userStore.Context.SaveChangesAsync();
                return RedirectToAction("Profile", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Güncelleme sırasında bir hata oluştu.{ex.Message}");
                return View("Profile");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ProfilePasswordMultiViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Profile", "Account");
            try
            {
                var userStore = MembershipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = await userManager.FindByIdAsync(HttpContext.User.Identity.GetUserId());
                user = await userManager.FindAsync(user.UserName, model.ChangePasswordViewModel.OldPassword);
                if (user == null)
                {
                    ModelState.AddModelError("OldPassword", "Mevcut şifreniz hatalı");
                    return View("Profile");
                }
                await userStore.SetPasswordHashAsync(user,
                    userManager.PasswordHasher.HashPassword(model.ChangePasswordViewModel.Password));
                await userStore.UpdateAsync(user);
                await userStore.Context.SaveChangesAsync();
                HttpContext.GetOwinContext().Authentication.SignOut();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Güncelleme işleminde bir hata oluştu.{ex.Message}");
                return View("Profile");
            }
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user1 = await userManager.FindByEmailAsync(model.Email);
            if (user1 == null)
            {
                ModelState.AddModelError("", "Bu e-mail adresi kayıtlı değildir.");
                return View(model);
            }

            var user2 = await userManager.FindByNameAsync(model.Username);
            if (user2 == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı değildir.");
                return View(model);
            }

            if (user1.Id != user2.Id)
            {
                ModelState.AddModelError("", "Girdiğiniz bilgiler geçerli değil");
                return View(model);
            }

            var randomPassword = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            await userStore.SetPasswordHashAsync(user1, userManager.PasswordHasher.HashPassword(randomPassword));
            await userStore.Context.SaveChangesAsync();
            await SiteSettings.SendMail(new MailModel()
            {
                To = user1.Email,
                Subject = "Şifreniz değişti",
                Message = $"Merhaba {user1.Name} <br>Yeni Şifreniz <b>{randomPassword}</b>"
            });
            return RedirectToAction("Login", "Account");
        }

        public async Task<ActionResult> Activation(string code, string u)
        {
            try
            {
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(u))
                    return RedirectToAction("Index", "Home");
                var userStore = MembershipTools.NewUserStore();
                var sonuc = userStore.Context.Set<ApplicationUser>()
                    .FirstOrDefault(x => x.Id == u && x.ActivationCode == code);
                if (sonuc == null)
                {
                    ViewBag.sonuc = "<span class='text-danger'>Aktivasyon işlemi başarısız</span>";
                    return View();
                }
                if (sonuc.EmailConfirmed)
                {
                    ViewBag.sonuc = "<span class='text-warning'>E-posta adresiniz zaten onaylanmış.</span>";
                    return View();
                }
                sonuc.EmailConfirmed = true;
                await userStore.UpdateAsync(sonuc);
                await userStore.Context.SaveChangesAsync();
                var userManager = MembershipTools.NewUserManager();
                await userManager.RemoveFromRoleAsync(sonuc.Id, "Passive");
                await userManager.AddToRoleAsync(sonuc.Id, "User");
                ViewBag.sonuc = $"<span class='text-success'>Hoşgeldiniz {sonuc.Name} {sonuc.Surname} e-posta adresiniz başarıyla onaylanmıştır.</span>";
                await SiteSettings.SendMail(new MailModel()
                {
                    To = sonuc.Email,
                    Subject = "Aktivasyon",
                    Message = ViewBag.sonuc.ToString()
                });
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.sonuc = "<span class='text-danger'>Aktivasyon işlemi başarısız.</span>";
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditUsers()
        {
            var popular = new Repository.ArticleRepo().Queryable().Where(x => x.Confirmed == true).OrderByDescending(x => x.LikeCount).Take(5).ToList();
            ViewBag.popular = popular;
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var model = userManager.Users.ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> MakeEditor(string id)
        {
            try
            {
                var userStore = MembershipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = await userManager.FindByIdAsync(id);
                await userManager.RemoveFromRoleAsync(user.Id, "User");
                await userManager.AddToRoleAsync(user.Id, "Editor");
                await userStore.Context.SaveChangesAsync();
                return RedirectToAction("EditUsers", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("EditUsers", "Account");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);
            return RedirectToAction("EditUsers", "Account");

        }
    }
}