
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class ResultController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public ResultController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient GetClient()
        {
            var client = _factory.CreateClient("RaceDayAPI");

            var cookie = HttpContext.Session
                .GetString("ApiCookie");

            if (!string.IsNullOrEmpty(cookie))
            {
                client.DefaultRequestHeaders
                    .TryAddWithoutValidation("Cookie", cookie);
            }

            return client;
        }

        private IActionResult? CheckRole(string requiredRole)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (HttpContext.Session.GetString("Role") != requiredRole)
            {
                return StatusCode(403,
                    "Access denied. Your account does not have permission.");
            }

            return null;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var denied = CheckRole("Organiser");
            if (denied != null)
                return denied;

            return View(new ResultViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultViewModel model)
        {
            var denied = CheckRole("Organiser");
            if (denied != null)
                return denied;

            if (!ModelState.IsValid)
                return View(model);

            using var client = GetClient();

            using var response = await client.PostAsJsonAsync(
                "api/Result", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Failed to save result: " +
                    await response.Content.ReadAsStringAsync());

                return View(model);
            }

            TempData["Success"] =
                "Race result captured successfully!";

            return RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var denied = CheckRole("Participant");
            if (denied != null)
                return denied;

            using var client = GetClient();

            using var response = await client.GetAsync(
                "api/Result/mine");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "Could not load your results: " +
                    await response.Content.ReadAsStringAsync();

                return View(new List<JsonElement>());
            }

            var results = await response.Content
                .ReadFromJsonAsync<List<JsonElement>>();

            return View(results ?? new List<JsonElement>());
        }
    }
}
