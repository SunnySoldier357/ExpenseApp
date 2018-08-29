using System;
using System.Linq;
using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet, Route("edit/{id}")]
        public IActionResult Edit(string statementNumber, string id)
        {
            ExpenseEntry entry = _db.ExpenseEntries
                .Find(new Guid(id));

            ViewBag.StatementNumber = statementNumber;
            ViewBag.Accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToList();

            return View(entry);
        }

        [HttpPost, Route("edit/{id}")]
        public IActionResult Edit(string statementNumber, string id, ExpenseEntry updated)
        {
            ExpenseEntry entry = _db.ExpenseEntries
                .Include(ee => ee.Account)
                .FirstOrDefault(ee => ee.Id == new Guid(id));
            
            if (!ModelState.IsValid)
            {
                ViewBag.StatementNumber = statementNumber;
                ViewBag.Accounts = _db.Accounts
                    .OrderBy(a => a.Name)
                    .ToList();

                return View(updated);
            }

            entry.Account = _db.Accounts
                .Find(entry.Account.Name);
            entry.Cost = updated.Cost;
            entry.Date = updated.Date;
            entry.Description = updated.Description;
            entry.Receipt = updated.Receipt;

            _db.SaveChanges();

            return RedirectToAction("Edit", "Form", new
            {
                statementNumber
            });
        }

        [Route("{id}/{returnUrl}")]
        public IActionResult Details(string statementNumber, string id, string returnUrl)
        {
            ExpenseEntry entry = _db.ExpenseEntries
                .Find(new Guid(id));

            ViewBag.StatementNumber = statementNumber;
            ViewBag.ReturnUrl = Uri.UnescapeDataString(returnUrl);

            return View(entry);
        }

        [HttpGet, Route("delete/{id}")]
        public IActionResult Delete(string statementNumber, string id)
        {
            ExpenseEntry entry = _db.ExpenseEntries
                .Include(ee => ee.Account)
                .FirstOrDefault(ee => ee.Id == new Guid(id));
            
            ViewBag.StatementNumber = statementNumber;

            return View(entry);
        }

        [HttpPost, Route("delete/{id}")]
        public IActionResult Delete(string statementNumber, string id, ExpenseEntry deleted)
        {
            ExpenseEntry entry = _db.ExpenseEntries
                .Include(ee => ee.Account)
                .FirstOrDefault(ee => ee.Id == new Guid(id));
            
            if (entry != null)
            {
                _db.ExpenseEntries.Remove(entry);
                _db.SaveChanges();

                return RedirectToAction("Edit", "Form", new
                {
                    statementNumber,
                    message = "The Expense Claim has been successfully deleted."
                });
            }

            return RedirectToAction("Edit", "Form", new
            {
                message = "There was an error in deleting the Expense Claim."
            });
        }
    }
}