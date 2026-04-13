using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.MVC.Models;

namespace OnlineTravelBooking.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAdminAuthService _adminAuthService;

        public AuthController(IAdminAuthService adminAuthService)
        {
            _adminAuthService = adminAuthService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _adminAuthService.LoginAsync(model.Email, model.Password, isPersistent: true);
            if (result.IsFailure)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _adminAuthService.LogoutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
