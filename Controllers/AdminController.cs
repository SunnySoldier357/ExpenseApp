using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        // Public Methods
        public IActionResult Index() => View();
    }
}