using CheckOver.Models;
using CheckOver.Models.ViewModels;
using CheckOver.Repository;
using CheckOver.Service;
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
        public async Task<IActionResult> SignUp(SignUpVM userModel)
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
                return RedirectToAction("SignIn", "Account");
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
        public async Task<IActionResult> SignIn(SignInVM signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(signInModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Exercise");
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

        [HttpGet]
        [Route("zmiana-hasla")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("zmiana-hasla")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePassword(changePasswordVM);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
            }
            return View(changePasswordVM);
        }
    }
}