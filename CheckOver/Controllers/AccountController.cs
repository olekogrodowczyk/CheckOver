using CheckOver.Models;
using CheckOver.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;


        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [Route("rejestracja")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [Route("rejestracja")]
        public async Task<IActionResult> SignUp(SignUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(userModel);
        }

        [HttpGet]
        [Route("logowanie")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("logowanie")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(signInModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Błędny adres e-mail albo hasło");
                }
            }
            return View(signInModel);            
        }

        [Route("wylogowywanie")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
