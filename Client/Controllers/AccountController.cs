using API.DTOs.Accounts;
using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository repository;

        public AccountController(IAccountRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginAccountDto login)
        {
            var result = await repository.Login(login);

            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Status == "BadRequest")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            else if (result.Status == "OK")
            {
                HttpContext.Session.SetString("JWToken", result.Data);
                return RedirectToAction("Index", "Employee");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterAccountDto register)
        {
            var result = await repository.Register(register);
            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Status == "BadRequest")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = $"Something Went Wrong! - {result.Message}!";
                return View();
            }
            else if (result.Status == "OK")
            {
                TempData["Success"] = $"Data Has Been Successfully Registered! - {result.Message}!";
                return RedirectToAction("Index", "Account");
            }
            return View();
        }


        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}

