using ExpenseApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Index()
        {
            var listings = new[]
            {
                new ExpenseListing
                {
                    StatementNumber = "0818-CLIENT-PROJECT-01",
                    Title = "Test Claim 1",
                    ListingStatus = Status.Saved
                },
                new ExpenseListing
                {
                    StatementNumber = "0818-CLIENT-PROJECT-02",
                    Title = "Test Claim 2",
                    ListingStatus = Status.Approved
                },
                new ExpenseListing
                {
                    StatementNumber = "0818-CLIENT-PROJECT-03",
                    Title = "Test Claim 3",
                    ListingStatus = Status.Rejected
                },
                new ExpenseListing
                {
                    StatementNumber = "0818-CLIENT-PROJECT-04",
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