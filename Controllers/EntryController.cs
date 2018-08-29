using System;
using System.Linq;
using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    [Route("form/edit/{statementNumber}/entry")]
    public class EntryController : Controller
    {
        private readonly ExpenseDBDataContext _db;

        public EntryController(ExpenseDBDataContext db) => _db = db;

        [HttpGet, Route("create")]
        public IActionResult Create(string statementNumber)
        {
            ViewBag.StatementNumber = statementNumber;
            ViewBag.Accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToList();

            return View(new ExpenseEntry());
        }

        [HttpPost, Route("create")]
        public IActionResult Create(string statementNumber, ExpenseEntry entry)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StatementNumber = statementNumber;
                ViewBag.Accounts = _db.Accounts
                    .OrderBy(a => a.Name)
                    .ToList();

                return View(entry);
            }

            entry.Form = _db.ExpenseForms.Find(statementNumber);
            entry.Account = _db.Accounts
                .Find(entry.Account.Name);

            _db.ExpenseEntries.Add(entry);
            _db.SaveChanges();

            return RedirectToAction("Edit", "Form", new
            {
                statementNumber
            });
        }
    }
}