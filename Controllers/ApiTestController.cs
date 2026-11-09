
using Microsoft.AspNetCore.Mvc;

namespace RaceDayMVC.Controllers
{
    public class ApiTestController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ApiTestController(
            IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _clientFactory
                    .CreateClient("RaceDayAPI");

                // Correct RaceDay API endpoint
                var response = await client.GetAsync("api/Event");

                var content = await response.Content
                    .ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(
                        "SUCCESS! RaceDay MVC is connected to the API!\n\n"
                        + content,
                        "text/plain");
                }

                return Content(
                    $"API responded with status: {(int)response.StatusCode}\n\n"
                    + content,
                    "text/plain");
            }
            catch (HttpRequestException ex)
            {
                return Content(
                    "API connection failed: " + ex.Message,
                    "text/plain");
            }
        }
    }
}
