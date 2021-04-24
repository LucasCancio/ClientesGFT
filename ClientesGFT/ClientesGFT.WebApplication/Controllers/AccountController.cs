using ClientesGFT.Domain.Interfaces.Services;
using ClientesGFT.Domain.Util;
using ClientesGFT.WebApplication.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClientesGFT.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _loginService;

        public AccountController(IUserService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            try
            {
                if (!ModelState.IsValid) return View();

                string returnUrl = HttpContext.Request.Query["ReturnUrl"].ToString();
                if (string.IsNullOrEmpty(returnUrl)) returnUrl = "/";

                var userFromDb = _loginService.Login(loginVM.Login, loginVM.Senha);
                if (userFromDb == null)
                {
                    ModelState.AddModelError("", "Usuario e/ou inválidos!");
                    return View();
                }

                var roles = EnumHelper.PerfilListParaString(userFromDb.Roles);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromDb.Name),
                new Claim("Roles", roles),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}
