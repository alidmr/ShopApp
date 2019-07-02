using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.UI.Extensions;
using ShopApp.UI.Identity;
using ShopApp.UI.Models;
using ShopApp.UI.Models.Account;

namespace ShopApp.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ICartService _cartService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = code });

                //send mail
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız",
                    $"Litfen email hesabınızı onaylamak için <a href='http://localhost:51791{callbackUrl}'>tıklayınız.</a>");

                TempData.Put("message", new ResultMessageViewModel()
                {
                    Title = "Hesap Onayı",
                    Message = "Eposta adresineze gelen link ile hesabınızı onaylayınız.",
                    Css = "warning"
                });

                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu lütfen tekrar deneyiniz");
            return View(model);
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Bu email adresine ait bir hesp bulunamadı.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı email ile onaylayınız.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("", "Email adresi veya parola yanlış.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new ResultMessageViewModel()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınızdan güvenli bir şekilde çıkış yapıldı.",
                Css = "warning"
            });
            return Redirect("/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new ResultMessageViewModel()
                {
                    Title = "Hesap Onayı",
                    Message = "Hesap onayı için bilgileriniz geçersiz.",
                    Css = "danger"
                });
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    // create cart object
                    _cartService.InitializeCart(user.Id);

                    TempData.Put("message", new ResultMessageViewModel()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız başarı ile onaylanmış.",
                        Css = "success"
                    });
                    return RedirectToAction("Login");
                }
            }
            TempData.Put("message", new ResultMessageViewModel()
            {
                Title = "Hesap Onayı",
                Message = "Hesap onaylanmadı.",
                Css = "danger"
            });
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData.Put("message", new ResultMessageViewModel()
                {
                    Title = "Şifremi Unuttum",
                    Message = "Lütfen e-mail adresinizi giriniz.",
                    Css = "warning"
                });
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData.Put("message", new ResultMessageViewModel()
                {
                    Title = "Şifremi Unuttum",
                    Message = "Bu e-maile kayıtlı bir hesap bulunumadı.",
                    Css = "warning"
                });
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token = code });

            //send mail
            await _emailSender.SendEmailAsync(email, "Şifremi Unuttum",
                $"Şifrenizi sıfırlamak için lütfen linke <a href='http://localhost:51791{callbackUrl}'>tıklayınız.</a>");

            TempData.Put("message", new ResultMessageViewModel()
            {
                Title = "Hesap Onayı",
                Message = "Şifrenizi sıfırlamak için hesabınıza e-mail gönderildi.",
                Css = "warning"
            });


            return RedirectToAction("Login", "Account");
        }
        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("ForgotPassword");
            }
            var model = new ResetPasswordViewModel()
            {
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}