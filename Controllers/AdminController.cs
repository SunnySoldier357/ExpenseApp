using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        // Public Methods
        public IActionResult Index()
        {
            if (!AuthController.SignedIn)
            {
                return RedirectToAction("Register", "Auth", new 
                {
                    returnLocation = (int) ReturnLocation.AdminHomePage
                });
            }

            if (!AuthController.IsApprover)
                return RedirectToAction("AccessDenied", "Auth");

            return View();
        }
    }
}