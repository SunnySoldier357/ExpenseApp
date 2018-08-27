using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ExpenseDBDataContext _db;

        public AdminController(ExpenseDBDataContext db) => _db = db;

        public IActionResult Index()
        {
            // TODO: Implement Admin Home Page
            return View();
        }
    }
}