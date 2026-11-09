
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public RegistrationController(IHttpClientFactory factory)
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

        private bool IsParticipant()
        {
            return HttpContext.Session.GetInt32("UserId") != null
                && HttpContext.Session.GetString("Role")
                    == "Participant";
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetInt32("UserId") != null
                && HttpContext.Session.GetString("Role")
                    == "Organiser";
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            using var client = GetClient();

            using var response = await client.GetAsync(
                "api/Registration/mine");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "Could not load your registrations.";

                return View(new List<JsonElement>());
            }

            var registrations = await response.Content
                .ReadFromJsonAsync<List<JsonElement>>();

            return View(registrations ?? new List<JsonElement>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(int eventId)
        {
            if (!IsParticipant())
            {
                if (HttpContext.Session.GetInt32("UserId") == null)
                    return RedirectToAction("Login", "Account");

                return Forbid();
            }

            if (eventId <= 0)
            {
                TempData["Error"] =
                    "Invalid event ID. The event identifier was not loaded correctly.";

                return RedirectToAction("Index", "Event");
            }

            using var client = GetClient();

            using var response = await client.PostAsJsonAsync(
                "api/Registration",
                new { eventId });

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Successfully enrolled for the event!";

                return RedirectToAction("Mine");
            }

            var error = await response.Content.ReadAsStringAsync();

            TempData["Error"] =
                $"Enrolment failed ({(int)response.StatusCode}): {error}";

            return RedirectToAction("Index", "Event");
        }

        [HttpGet]
        public async Task<IActionResult> EventRegistrations(int eventId)
        {
            if (!IsOrganiser())
            {
                if (HttpContext.Session.GetInt32("UserId") == null)
                    return RedirectToAction("Login", "Account");

                return Forbid();
            }

            if (eventId <= 0)
                return BadRequest("Invalid event ID.");

            using var client = GetClient();

            using var response = await client.GetAsync(
                $"api/Registration/event/{eventId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "Could not load event registrations.";

                return View(new List<JsonElement>());
            }

            var registrations = await response.Content
                .ReadFromJsonAsync<List<JsonElement>>();

            return View(registrations ?? new List<JsonElement>());
        }
    }
}
