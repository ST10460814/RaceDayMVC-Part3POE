
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public AccountController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient("RaceDayAPI");

            // Replace with your actual API login route.
            var response = await client.PostAsJsonAsync(
                "api/Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Login failed. Check your credentials.");
                return View(model);
            }

            // TODO: Read the API's authenticated response,
            // preserve its session cookie or token,
            // and obtain the verified user role.
            // Do not trust a role supplied by the browser.

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient("RaceDayAPI");

            // Replace with your actual API registration route.
            var response = await client.PostAsJsonAsync(
                "api/Auth/register", model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Login");

            ModelState.AddModelError("",
                "Registration failed. Please check your details.");

            return View(model);
        }
    }
}
