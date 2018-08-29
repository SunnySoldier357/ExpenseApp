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
        private const string TEMP_PLACEHOLDER_STRING = "aqw(*&^%$#@!#$%^TYUI<>>>,.";
        private readonly DateTime TEMP_PLACEHOLDER_DATE = new DateTime(3333, 5, 11);

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
                        orderby f.StatementNumber.Substring(2, 2) descending,
                            f.StatementNumber.Substring(0, 2) descending,
                            f.StatementNumber.Substring(5, f.StatementNumber.Length - 8),
                            f.StatementNumber.Substring(f.StatementNumber.Length - 2) descending
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
            var signedInUser = _db.Employees
                .Include(e => e.Approver)
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == SignedInEmployee.Id);

            ViewBag.ShowErrors = true;

            return View(new ExpenseForm(signedInUser));
        }

        [HttpPost]
        public IActionResult Create(ExpenseForm form, string command)
        {
            if (command == "Save")
            {
                if (string.IsNullOrWhiteSpace(form.Title))
                {
                    ModelState.AddModelError("", "A title is required to save this report.");
                    ViewBag.ShowErrors = false;
                    return View(form);
                }

                fillInTempValues(form);

                _db.ExpenseForms.Add(form);
                _db.SaveChanges();

                return RedirectToAction("Index", "Form", form.StatementNumber);
            }
            else if (command == "Submit")
            {
                ViewBag.ShowErrors = true;

                if (form.Entries.Count() == 0)
                    ModelState.AddModelError("", "There must be at least 1 Expense Entry");

                if (!ModelState.IsValid)
                    return View(form);

                return RedirectToAction("Index", "Form", form.StatementNumber);
            }
            else
            {
                ViewBag.ShowErrors = true;
                return View(form);
            }
        }

        [HttpGet]
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

            if (form.Status == Status.Saved)
                replaceTempValues(form, true);

            return View(form);
        }

        [HttpGet]
        [Route("Form/GetNextIdNumber/{statementNumber}")]
        public IActionResult GetNextIdNumber(string statementNumber) => 
            Json(data: getNextIdNumber(statementNumber));

        private int getNextIdNumber(string statementNumber)
        {
            var forms = from ef in _db.ExpenseForms
                        where ef.StatementNumber.StartsWith(statementNumber + "-")
                        select ef;

            return forms.Count() + 1;
        }

        private void fillInTempValues(ExpenseForm form)
        {
            if (string.IsNullOrWhiteSpace(form.Purpose))
                form.Purpose = TEMP_PLACEHOLDER_STRING;

            if (form.From == new DateTime())
                form.From = TEMP_PLACEHOLDER_DATE;

            if (form.To == new DateTime())
                form.To = TEMP_PLACEHOLDER_DATE;

            if (string.IsNullOrWhiteSpace(form.Project))
                form.Project = TEMP_PLACEHOLDER_STRING;

            form.Status = Status.Saved;

            DateTime now = DateTime.Now;
            string year = ("" + now.Year).Substring(2, 2);
            string month = "" + (now.Month > 10 ? "" : "0") + now.Month;

            form.StatementNumber = string.Format("{0}{1}-TEMP", month, year);
            int nextNum = getNextIdNumber(form.StatementNumber);
            form.StatementNumber += "-";
            if(("" + nextNum).Length == 1)
                form.StatementNumber += "0";
            form.StatementNumber += nextNum;
        }

        private void replaceTempValues(ExpenseForm form, bool detailsPage = false)
        {
            if (form.Purpose == TEMP_PLACEHOLDER_STRING)
                form.Purpose = detailsPage ? "<Blank>" : "";
            
            if (form.From == TEMP_PLACEHOLDER_DATE)
                form.From = new DateTime();
            
            if (form.To == TEMP_PLACEHOLDER_DATE)
                form.To = new DateTime();
            
            if (form.Project == TEMP_PLACEHOLDER_STRING)
                form.Project = detailsPage ? "<Blank>" : "";
        }
    }
}