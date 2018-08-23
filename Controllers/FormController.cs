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
                        Account = new Account()
                        {
                            Name = "Airfare"
                        },
                        Description = "Flight from LAX to SG",
                        Transport = 1000,
                        ReceiptId = "21312"
                    },
                    new ExpenseEntry()
                    {
                        Date = new DateTime(2018, 8, 28),
                        Account = new Account()
                        {
                            Name = "Employee Morale - Meals"
                        },
                        Description = "Provided meal for lunch meeting",
                        Meals = 40,
                        ReceiptId = null
                    }
                }
            };

            ViewBag.Accounts = getAccounts();

            return View(form);
        }

        [HttpPost]
        public IActionResult Create(ExpenseForm form)
        {
            return View();
        }

        private List<Account> getAccounts()
        {
            return new List<Account>()
            {
                new Account { Name = "Airfare" },
                new Account { Name = "Broadband – Home" },
                new Account { Name = "Broadband – Travel" },
                new Account { Name = "Car Rental" },
                new Account { Name = "Cargo, Parcel, Shipping" },
                new Account { Name = "Catering" },
                new Account { Name = "Cell Phone" },
                new Account { Name = "Training" },
                new Account { Name = "Conference/Seminar" },
                new Account { Name = "Dues/Subscriptions" },
                new Account { Name = "Employee Morale" },
                new Account { Name = "Employee Morale – Meals" },
                new Account { Name = "Employee Development/Training" },
                new Account { Name = "Entertainment" },
                new Account { Name = "Event Services" },
                new Account { Name = "Event Sponsorships" },
                new Account { Name = "Event Venues & Site Selection" },
                new Account { Name = "Events" },
                new Account { Name = "Expense – Multi Purpose" },
                new Account { Name = "Gift Cert / Tangible Gifts" },
                new Account { Name = "Hardware" },
                new Account { Name = "Hardware Development" },
                new Account { Name = "Hotel" },
                new Account { Name = "Infrastructure & Ops Support" },
                new Account { Name = "Maintenance – Repairs" },
                new Account { Name = "Marketing Services" },
                new Account { Name = "Meals – Entertainment" },
                new Account { Name = "Meals – Multipurpose" },
                new Account { Name = "Meals – Travel" },
                new Account { Name = "Meals for Meetings" },
                new Account { Name = "Membership & Dues" },
                new Account { Name = "Mileage" },
                new Account { Name = "Not for Reimbursement – Others" },
                new Account { Name = "Other Travel Expenses" },
                new Account { Name = "Parking" },
                new Account { Name = "Personal" },
                new Account { Name = "Phone/Fax" },
                new Account { Name = "Postage" },
                new Account { Name = "Supplies – Computer Equipment" },
                new Account { Name = "Supplies – General Office" },
                new Account { Name = "Supplies – Reference Material" },
                new Account { Name = "Tips/Gratuity" },
                new Account { Name = "Transportation – Others" },
                new Account { Name = "Transportation – Rail" },
                new Account { Name = "Transportation – Taxi" },
                new Account { Name = "Travel Fee" },
                new Account { Name = "Visas/Vaccinations" }
            };
        }
    }
}