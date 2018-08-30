using ExpenseApp.Models.DB;
using ExpenseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseApp.Controllers
{
    [Route("approver")]
    public class ApproverFormController : Controller
    {
        public Employee SignedInApprover;

        private readonly ExpenseDBDataContext _db;

        public ApproverFormController(ExpenseDBDataContext db)
        {
            _db = db;

            SignedInApprover = _db.Employees
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == new Guid("9adada3b-2032-4b9d-b23d-afa8e07666da"));
        }

        [Route("")]
        public IActionResult List()
        {
            var forms = _db.ExpenseForms
                .Include(ef => ef.Employee)
                    .ThenInclude(ef => ef.Location)
                .Where(ef => ef.Employee.Location == SignedInApprover.Location && 
                    ef.Status != Status.Saved && ef.Status != Status.Rejected)
                .OrderByDescending(ef => ef.StatementNumber.Substring(2, 2))
                .ThenByDescending(ef => ef.StatementNumber.Substring(0, 2))
                .ThenBy(ef => ef.StatementNumber.Substring(5, ef.StatementNumber.Length - 8))
                .ThenByDescending(ef => ef.StatementNumber.Substring(ef.StatementNumber.Length - 2))
                .Select(ef => new { ef.StatementNumber, ef.Title, ef.Status, EmployeeName = ef.Employee.FullName });

            var listings = new List<ExpenseListViewModel>();

            foreach (var form in forms)
                listings.Add(new ExpenseListViewModel(form, true));

            return View(listings);
        }

        [Route("{statementNumber}")]
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
    }
}