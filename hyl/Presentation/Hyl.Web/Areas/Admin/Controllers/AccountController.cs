using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hyl.Core.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;

namespace Hyl.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string u, string p)
        {
            if (u == "admin" && p == "admin")
            {
                //you can add all of ClaimTypes in this collection 
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,u)
                };

                //init the identity instances 
                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "SuperSecureLogin"));
                await HttpContext.Authentication.SignInAsync(BaseConfig.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = false
                });
            }
            return View();
        }

        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.Authentication.SignOutAsync(BaseConfig.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Forbidden()
        {
            return View();
        }

    }
}