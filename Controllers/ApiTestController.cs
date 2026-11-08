
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

                // Replace with your actual GET endpoint
                var response = await client.GetAsync(
                    "api/Events");

                var content =
                    await response.Content.ReadAsStringAsync();

                return Content(
                    $"API status: {(int)response.StatusCode}\n\n"
                    + content,
                    "text/plain");
            }
            catch (HttpRequestException ex)
            {
                return Content(
                    "API connection failed: " + ex.Message);
            }
        }
    }
}
