using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseApp.Models.DB;
using ExpenseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    public class FormController : Controller
    {
        private readonly ExpenseDBDataContext _db;

        public Employee SignedInEmployee;

        public FormController(ExpenseDBDataContext db)
        {
            _db = db;

            SignedInEmployee = _db.Employees.FirstOrDefault(
                e => e.Id == new Guid("46531176-18ac-43cf-921c-9729d59ab543"));
            SignedInEmployee.Approver = _db.Employees.FirstOrDefault(
                e => e.Id == SignedInEmployee.ApproverId);
        }

        public IActionResult Index()
        {
            var forms = from f in _db.ExpenseForms
                        where f.Employee.Id == SignedInEmployee.Id
                        select new { f.StatementNumber, f.Title, f.Status };

            List<ExpenseListing> listings = new List<ExpenseListing>();
            
            foreach (var item in forms)
            {
                listings.Add(new ExpenseListing()
                {
                    StatementNumber = item.StatementNumber,
                    Title = item.StatementNumber,
                    ListingStatus = item.Status
                });
            }

            return View(listings);
        }

        public IActionResult Create()
        {
            ViewBag.Accounts = _db.Accounts.ToArray();

            return View(new ExpenseForm(SignedInEmployee));
        }

        [HttpPost]
        public IActionResult Create(ExpenseForm form)
        {
            if (!ModelState.IsValid)
                return View();

            ViewBag.Accounts = _db.Accounts.ToArray();

            return View(new ExpenseForm(SignedInEmployee));
        }
    }
}