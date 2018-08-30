using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    public class TeamController : Controller
    {
        // Public Methods
        public IActionResult Privacy() => View();

        public IActionResult Terms() => View();
    }
}