
using Microsoft.AspNetCore.Mvc;

namespace RaceDayMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Organiser()
        {
            return View();
        }

        public IActionResult Participant()
        {
            return View();
        }
    }
}
