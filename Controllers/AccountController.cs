using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using dcoreidentity.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace dcoreidentity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //private readonly UserManager<CustomUser> userManager;
        private readonly UserManager<IdentityUser> userManager;

        // public AccountController(UserManager<CustomUser> userManager){
        public AccountController(UserManager<IdentityUser> userManager){
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model) {
            if (ModelState.IsValid) {
                var user = await userManager.FindByNameAsync(model.UserName);

                if (user == null) {
                    user = new IdentityUser {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName
                    };

                    var result = await userManager.CreateAsync(user, model.Password);
                }

                return View("Success");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model) {
            if (ModelState.IsValid) {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password)) {
                    var identity = new ClaimsIdentity("Cookies");
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Account");
                }
                ModelState.AddModelError("", "Invalid UserName or Password.");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}