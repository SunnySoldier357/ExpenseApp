using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Controllers
{
    [Route("form/edit/{statementNumber}/entry")]
    public class EntryController : Controller
    {
        // Private Properties
        private readonly ExpenseDBDataContext _db;

        // Constructors
        public EntryController(ExpenseDBDataContext db) => _db = db;

        // Public Methods
        [HttpGet]
        [Route("create")]
        public IActionResult Create(string statementNumber)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ViewBag.StatementNumber = statementNumber;
            ViewBag.Accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToList();

            return View(new ExpenseEntry());
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(string statementNumber, ExpenseEntry entry)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

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

            if (entry.ImageFormFile == null)
                entry.Receipt = null;
            else
            {
                // Getting the image uploaded and convert it to byte[]
                if (entry.ImageFormFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await entry.ImageFormFile.CopyToAsync(memoryStream);

                        entry.Receipt.FileName = entry.ImageFormFile.FileName
                            .Substring(entry.ImageFormFile.FileName.LastIndexOf('\\') + 1);

                        entry.Receipt.ReceiptImage = memoryStream.ToArray();
                    }
                }
            }

            _db.ExpenseEntries.Add(entry);
            _db.SaveChanges();

            return RedirectToAction("Edit", "Form", new
            {
                statementNumber
            });
        }

        [Route("{id}/{returnUrl}")]
        public IActionResult Details(string statementNumber, string id, string returnUrl)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseEntry entry = _db.ExpenseEntries
                .Include(ee => ee.Receipt)
                .FirstOrDefault(ee => ee.Id == new Guid(id));

            ViewBag.StatementNumber = statementNumber;
            ViewBag.ReturnUrl = Uri.UnescapeDataString(returnUrl);

            return View(entry);
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(string statementNumber, string id)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseEntry entry = _db.ExpenseEntries
                .Find(new Guid(id));

            ViewBag.StatementNumber = statementNumber;
            ViewBag.Accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToList();

            return View(entry);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(string statementNumber, string id,
            ExpenseEntry updated)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

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

            if (entry.ImageFormFile == null)
                entry.Receipt = null;
            else
            {
                // Getting the image uploaded and convert it to byte[]
                if (entry.ImageFormFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await entry.ImageFormFile.CopyToAsync(memoryStream);

                        entry.Receipt.FileName = entry.ImageFormFile.FileName
                            .Substring(entry.ImageFormFile.FileName.LastIndexOf('\\') + 1);

                        entry.Receipt.ReceiptImage = memoryStream.ToArray();
                    }
                }
            }

            _db.SaveChanges();

            return RedirectToAction("Edit", "Form", new
            {
                statementNumber
            });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(string statementNumber, string id)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseEntry entry = _db.ExpenseEntries
                .Include(ee => ee.Account)
                .FirstOrDefault(ee => ee.Id == new Guid(id));

            ViewBag.StatementNumber = statementNumber;

            return View(entry);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public IActionResult Delete(string statementNumber, string id, ExpenseEntry deleted)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

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