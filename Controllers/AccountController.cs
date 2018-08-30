using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class AccountController : Controller
    {
        // Private Properties
        private readonly ExpenseDBDataContext _db;

        // Constructors
        public AccountController(ExpenseDBDataContext db) => _db = db;

        // Public Methods
        [Route("accounts/{message?}")]
        public IActionResult List(string message)
        {
            if (message != null)
            {
                // Decoding message sent by Delete Action
                ViewBag.Message = message.Replace('?', '/');
            }
               

            var accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToArray();

            return View(accounts);
        }

        [HttpGet]
        [Route("accounts/create")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("accounts/create")]
        public IActionResult Create(Account account)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _db.Accounts.Add(account);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", 
                    "Database Error! The Name entered is already in the database!");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unknown Error! " + e.Message);
                return View();
            }

            return RedirectToAction("List", "Account", account.Slug);
        }

        [HttpGet]
        [Route("accounts/edit/{slug}")]
        public IActionResult Edit(string slug)
        {
            Account account = _db.Accounts
                .FirstOrDefault(a => a.Slug == slug);

            if (null == account)
                return NotFound();

            return View(account);
        }

        [HttpPost]
        [Route("accounts/edit/{slug}")]
        public IActionResult Edit(string slug, Account updated)
        {
            Account account = _db.Accounts
                .FirstOrDefault(a => a.Slug == slug);

            if (null == account)
                return NotFound();

            try
            {
                // Name is a key so it cannot be modified, therefore, the old
                // Account must be deleted before a new one is added
                _db.Accounts.Remove(account);
                _db.Accounts.Add(updated);

                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", 
                    "Database Error! The Name entered is already in the database!");
                
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unknown Error! " + e.Message);
                return View();
            }

            return RedirectToAction("List", "Account", updated.Slug);
        }

        [HttpGet]
        [Route("accounts/delete/{slug}")]
        public IActionResult Delete(string slug)
        {
            Account account = _db.Accounts
                .FirstOrDefault(a => a.Slug == slug);
            
            return View(account);
        }

        [HttpPost]
        [Route("accounts/delete/{slug}")]
        public IActionResult Delete(string slug, Account deleted)
        {
            Account account = _db.Accounts
                .FirstOrDefault(a => a.Slug == slug && 
                    a.Name == deleted.Name);

            if (account != null)
            {
                _db.Accounts.Remove(account);
                _db.SaveChanges();

                return RedirectToAction("List", "Account", new
                {
                    // Encoding slash so List Action interprets the whole message
                    message = string.Format("The Account \"{0}\" has been successfully deleted.",
                        account.Name.Replace('/', '?'))
                });
            }

            return RedirectToAction("List", "Account", new
            {
                // Encoding slash so List Action interprets the whole message
                message = string.Format("There was an error in deleting the Account \"{0}\".",
                        account.Name.Replace('/', '?'))
            });
        }
    }
}