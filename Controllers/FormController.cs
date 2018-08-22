using System;
using System.Collections.Generic;
using ExpenseApp.Models;
using ExpenseApp.Models.DB;
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
            var form = new ExpenseForm()
            {
                StatementNumber = "0818-MICROSOFT-AURORA-01",
                Title = "Microsoft Aurora Project Claim",
                Purpose = "Do something",
                From = new DateTime(2018, 8, 1),
                To = new DateTime(2018, 8, 28),
                Project = "MICROSOFT-AURORA",
                Comment = "A sample comment",
                Status = Status.Saved,
                Employee = new Employee()
                {
                    Name = "Sandeep Singh Sidhu",
                    Location = "Orange Studios US",
                    Approver = new Employee()
                    {
                        Name = "JPS Kohli"
                    }
                },
                Entries = new List<ExpenseEntry>()
                {
                    new ExpenseEntry()
                    {
                        Date = new DateTime(2018, 8, 1),
                        Account = Account.Airfare,
                        Description = "Flight from LAX to SG",
                        Transport = 1000,
                        ReceiptId = "21312"
                    },
                    new ExpenseEntry()
                    {
                        Date = new DateTime(2018, 8, 28),
                        Account = Account.EmployeeMoraleMeals,
                        Description = "Provided meal for lunch meeting",
                        Meals = 40,
                        ReceiptId = null
                    }
                }
            };
            return View(form);
        }
        
        [HttpPost]
        public IActionResult Create(ExpenseForm form)
        {
            return View();
        }
    }
}