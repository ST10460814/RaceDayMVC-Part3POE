
using Microsoft.AspNetCore.Mvc;

namespace RaceDayMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Organiser()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Forbid();

            return View();
        }

        public IActionResult Participant()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Participant")
                return Forbid();

            return View();
        }
    }
}
