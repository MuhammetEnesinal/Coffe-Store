using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CoffeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeStore.Controllers
{   


    public class AccountController : Controller
    {
         private readonly UserManager<AppUser> _userManager;
         private readonly SignInManager<AppUser> _signInManager;
         private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }
        
         public ActionResult Login()
        {

            return View();

        }


        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginModel model,string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                

                if(user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.BeniHatirla, true);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);
                        
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);

                        }
                        else
                        {
                        return RedirectToAction("Index", "Home");
                            
                        }



                    }else if (result.IsLockedOut)
                    {
                        var timeEnd = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = timeEnd.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Parolanız Hatalıdır.Hesabınız {timeLeft.Minutes + 1} dakikalığına kitlenmiştir");


                    }
                    else
                    {
                        ModelState.AddModelError("", "Hatalı parola");

                    }
                }
                else
                {

                    ModelState.AddModelError("", "Hatalı email");

                }
                

            }

            return View();

        }

        public ActionResult Create()
        {


            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Create(AccountCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email, Email = model.Email, AdSoyad = model.AdSoyad };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }

            return View(model);
        }


        [Authorize]
        public async Task<ActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");

        }

        [Authorize]
        public async Task<ActionResult> EditUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AccountEditUserModel
            {
                AdSoyad = user.AdSoyad,
                Email = user.Email!
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> EditUser(AccountEditUserModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId!);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                user.AdSoyad = model.AdSoyad;
                user.Email = model.Email;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("EditUser", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);

        }


        public ActionResult AccessDenied()
        {
            return View();
        }


        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(AccountChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId!);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (email == null)
            {
                TempData["Mesaj"] = "Lütfen e-posta adresinizi giriniz.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {

                TempData["Mesaj"] = "Şifre sıfırlama talimatları e-posta adresinize gönderildi.";

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token });
                await _emailService.SendEmailAsync(user.Email!, "Şifre Sıfırlama", $"Şifre sıfırlama talimatları için <a href=\"{url}\">tıklayınız</a>");

                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["Mesaj"] = "E-posta adresi bulunamadı.";
                return View();
            }
        }

        public ActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AccountResetPasswordModel { Email = email, Token = token };
            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> ResetPassword(AccountResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}