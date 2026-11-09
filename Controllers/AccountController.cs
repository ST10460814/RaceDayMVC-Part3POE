
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public AccountController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        // LOGIN PAGE
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient("RaceDayAPI");

            try
            {
                using var response = await client.PostAsJsonAsync(
                    "api/Auth/login",
                    new
                    {
                        model.Email,
                        model.Password
                    });

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("",
                        "Invalid email or password.");
                    return View(model);
                }

                using var json = JsonDocument.Parse(
                    await response.Content.ReadAsStringAsync());

                var user = json.RootElement;

                int userId = user.GetProperty("id").GetInt32();

                string role = user.GetProperty("role")
                    .GetString() ?? "";

                string firstName = user.GetProperty("firstName")
                    .GetString() ?? "";

                if (role != "Organiser" && role != "Participant")
                {
                    ModelState.AddModelError("",
                        "Invalid user role.");
                    return View(model);
                }

                // Get the correct API session cookie
                string? apiCookie = null;

                if (response.Headers.TryGetValues(
                    "Set-Cookie", out var cookies))
                {
                    foreach (var cookie in cookies)
                    {
                        var cookiePair = cookie.Split(';')[0];

                        if (cookiePair.StartsWith(
                            "RaceDay.Session=",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            apiCookie = cookiePair;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(apiCookie))
                {
                    ModelState.AddModelError("",
                        "API did not return RaceDay.Session cookie.");
                    return View(model);
                }

                // Save MVC session information
                HttpContext.Session.Clear();

                HttpContext.Session.SetInt32(
                    "UserId", userId);

                HttpContext.Session.SetString(
                    "Role", role);

                HttpContext.Session.SetString(
                    "FirstName", firstName);

                HttpContext.Session.SetString(
                    "ApiCookie", apiCookie);

                // Redirect according to user role
                if (role == "Organiser")
                {
                    return RedirectToAction(
                        "Organiser", "Dashboard");
                }

                return RedirectToAction(
                    "Participant", "Dashboard");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("",
                    "Cannot connect to RaceDay API. " +
                    "Make sure TestingPoe is running.");

                return View(model);
            }
            catch (JsonException)
            {
                ModelState.AddModelError("",
                    "Invalid response received from API.");

                return View(model);
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError("",
                    "API login response is missing user details.");

                return View(model);
            }
        }

        // REGISTRATION PAGE
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient("RaceDayAPI");

            try
            {
                using var response = await client.PostAsJsonAsync(
                    "api/Auth/register",
                    new
                    {
                        model.FirstName,
                        model.LastName,
                        model.Email,
                        model.Password,
                        model.Role
                    });

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] =
                        "Registration successful! Please log in.";

                    return RedirectToAction("Login");
                }

                var error = await response.Content
                    .ReadAsStringAsync();

                ModelState.AddModelError("",
                    "Registration failed: " + error);

                return View(model);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("",
                    "Cannot connect to RaceDay API.");

                return View(model);
            }
        }

        // LOGOUT USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var apiCookie = HttpContext.Session
                .GetString("ApiCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                try
                {
                    var client = _factory
                        .CreateClient("RaceDayAPI");

                    using var request = new HttpRequestMessage(
                        HttpMethod.Post,
                        "api/Auth/logout");

                    request.Headers.TryAddWithoutValidation(
                        "Cookie", apiCookie);

                    using var response =
                        await client.SendAsync(request);
                }
                catch (HttpRequestException)
                {
                    // Clear MVC session even if API is unavailable.
                }
            }

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
