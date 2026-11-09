
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public EventController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient GetClient()
        {
            var client = _factory.CreateClient("RaceDayAPI");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            if (!string.IsNullOrWhiteSpace(cookie))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Cookie", cookie);
            }

            return client;
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        private bool IsOrganiser()
        {
            return IsLoggedIn() &&
                   HttpContext.Session.GetString("Role") == "Organiser";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            try
            {
                using var client = GetClient();

                using var response =
                    await client.GetAsync("api/Event");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        $"Could not load events. API returned {(int)response.StatusCode}.";

                    return View(new List<EventViewModel>());
                }

                var events = await response.Content
                    .ReadFromJsonAsync<List<EventViewModel>>(
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "Cannot connect to RaceDay API. Make sure TestingPoe is running.";

                return View(new List<EventViewModel>());
            }
            catch (JsonException)
            {
                TempData["Error"] =
                    "The API returned event data in an unexpected format.";

                return View(new List<EventViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            if (!IsOrganiser())
                return Forbid();

            return View(new EventViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            if (!IsOrganiser())
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            using var client = GetClient();

            try
            {
                using var response =
                    await client.PostAsJsonAsync("api/Event", model);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("",
                        "Could not create event: " +
                        await response.Content.ReadAsStringAsync());

                    return View(model);
                }

                TempData["Success"] = "Event created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("",
                    "Cannot connect to RaceDay API.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            if (!IsOrganiser())
                return Forbid();

            using var client = GetClient();

            try
            {
                using var response =
                    await client.GetAsync($"api/Event/{id}");

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var model = await response.Content
                    .ReadFromJsonAsync<EventViewModel>();

                return model == null ? NotFound() : View(model);
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Cannot connect to RaceDay API.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, EventViewModel model)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            if (!IsOrganiser())
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            using var client = GetClient();

            try
            {
                using var response =
                    await client.PutAsJsonAsync(
                        $"api/Event/{id}", model);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("",
                        "Could not update event: " +
                        await response.Content.ReadAsStringAsync());

                    return View(model);
                }

                TempData["Success"] = "Event updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("",
                    "Cannot connect to RaceDay API.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            if (!IsOrganiser())
                return Forbid();

            using var client = GetClient();

            try
            {
                using var response =
                    await client.DeleteAsync($"api/Event/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] =
                        "Event deleted successfully!";
                }
                else
                {
                    TempData["Error"] =
                        "Could not delete event. API returned " +
                        (int)response.StatusCode;
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "Cannot connect to RaceDay API.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
