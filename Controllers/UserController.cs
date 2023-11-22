using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using desafio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using System.Security.Policy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace desafio.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<User> _logger;

        public UserController(UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<User> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnURL = null)
        {
            ViewBag.returnURL = returnURL;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginViewModel model, string returnURL=null)
        {
            ViewBag.returnURL = returnURL;
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.keepLoggedIn,
                    lockoutOnFailure: false
                );
                if(result.Succeeded)
                {
                    _logger.LogInformation("Autenticate User!");
                    return RedirectToLocal(returnURL);
                }
            }    
            
            return View(model);
        }
        private IActionResult RedirectToLocal(string returnURL)
        {
            if (Url.IsLocalUrl(returnURL))
            {
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult NewUser()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(NewLoginModel model, 
        string returnURL = null)
        {
            ViewBag.ReturnUrl = returnURL;
            if(ModelState.IsValid)
            {
                var user = new User{UserName = model.Email,Email=model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _signInManager.SignInAsync(user,isPersistent: false);

                    TempData["type"] = "Success";
                    TempData["title"] = "Logon:";
                    TempData["body"] = "Usuario cadastrado com sucesso!";

                    return RedirectToLocal(returnURL);
                }
                AddErrors(result);
            }
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        
    }
}