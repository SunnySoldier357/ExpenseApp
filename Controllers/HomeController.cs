using ExpenseApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var listings = new[]
            {
                new ExpenseListing
                {
                    Title = "Test Claim 1",
                    ListingStatus = Status.Saved
                },
                new ExpenseListing
                {
                    Title = "Test Claim 2",
                    ListingStatus = Status.Approved
                },
                new ExpenseListing
                {
                    Title = "Test Claim 3",
                    ListingStatus = Status.Rejected
                },
                new ExpenseListing
                {
                    Title = "Test Claim 4",
                    ListingStatus = Status.Submitted
                }
            };

            return View(listings);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(ExpenseForm form)
        {
            return View();
        }
    }
}