using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseApp.Models.DB;
using ExpenseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                new Guid("4c21f8bf-8d79-48fb-bc05-8e846714a006"));
        }

        public IActionResult Index()
        {
            var forms = from f in _db.ExpenseForms
                        where f.Employee.Id == SignedInEmployee.Id
                        select new { f.StatementNumber, f.Title, f.Status };

            var listings = new List<ExpenseListViewModel>();
            
            foreach (var form in forms)
                listings.Add(new ExpenseListViewModel(form));

            ViewBag.FirstName = SignedInEmployee.FirstName;

            return View(listings);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ExpenseCreateViewModel(_db, SignedInEmployee.Id));
        }

        [HttpPost]
        public IActionResult Create(ExpenseCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            return View(new ExpenseCreateViewModel(_db, SignedInEmployee.Id));
        }

        public IActionResult Details(string statementNumber)
        {
            ExpenseForm form = _db.ExpenseForms
                .Include(ef => ef.Entries)
                    .ThenInclude(ee => ee.Account)
                .Include(ef => ef.Entries)
                    .ThenInclude(ee => ee.Receipt)
                .Include(ef => ef.Employee)
                    .ThenInclude(e => e.Approver)
                .Include(ef => ef.Employee.Location)
                .FirstOrDefault(ef => ef.StatementNumber == statementNumber);

            return View(form);
        }

        [HttpGet]
        [Route("Form/GetNextIdNumber/{statementNumber}")]
        public IActionResult GetNextIdNumber(string statementNumber)
        {
            var forms = from ef in _db.ExpenseForms
                        where ef.StatementNumber.StartsWith(statementNumber + "-")
                        select ef;

            return Json(data: forms.Count() + 1);
        }
    }
}