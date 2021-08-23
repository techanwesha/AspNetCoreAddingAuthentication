using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WishList.Models;
using Microsoft.AspNetCore.Identity;
using WishList.Models.AccountViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result =  _userManager.CreateAsync(user).Result;
                if (!result.Succeeded)
                {
                    foreach (var m in result.Errors)
                    {
                        ModelState.AddModelError("Password",m.Description);
                    }
                    return View(model);
                }
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result= _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false).Result;

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                
                return RedirectToAction("Index", "Item");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            //return View("Logout ");
            return RedirectToAction("Index", "Home");
        }


    }
}
