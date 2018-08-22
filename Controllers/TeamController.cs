using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Terms()
        {
            return View();
        }
    }
}