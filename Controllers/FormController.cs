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

            SignedInEmployee = _db.Employees.Find(
                new Guid("46f81b60-6c36-4039-ba37-ffc83d9ac3f2"));
        }

        public IActionResult Index()
        {
            var forms = from f in _db.ExpenseForms
                        where f.Employee.Id == SignedInEmployee.Id
                        select new { f.StatementNumber, f.Title, f.Status };

            var listings = new List<ExpenseListViewModel>();
            
            foreach (var form in forms)
                listings.Add(new ExpenseListViewModel(form));

            return View(listings);
        }

        public IActionResult Create()
        {
            return View(new ExpenseCreateViewModel(_db, SignedInEmployee));
        }

        [HttpPost]
        public IActionResult Create(ExpenseCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            return View(new ExpenseCreateViewModel(_db, SignedInEmployee));
        }
    }
}