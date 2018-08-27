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
        private readonly ExpenseDBDataContext _db;

        public AccountController(ExpenseDBDataContext db) => _db = db;

        [Route("accounts/{message?}")]
        public IActionResult List(string message)
        {
            if (message != null)
                ViewBag.Message = message.Replace('?', '/');

            var accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToArray();

            return View(accounts);
        }

        [HttpGet, Route("accounts/create")]
        public IActionResult Create() => View();

        [HttpPost, Route("accounts/create")]
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
                ModelState.AddModelError("", "Database Error! The Name entered is already in the database!");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unknown Error! " + e.Message);
                return View();
            }

            return RedirectToAction("List", "Account", account.Slug);
        }

        [HttpGet, Route("accounts/edit/{slug}")]
        public IActionResult Edit(string slug)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.Slug == slug);

            if (null == account)
                return NotFound();

            return View(account);
        }

        [HttpPost, Route("accounts/edit/{slug}")]
        public IActionResult Edit(string slug, Account updated)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.Slug == slug);

            if (null == account)
                return NotFound();

            try
            {
                _db.Accounts.Remove(account);
                _db.Accounts.Add(updated);

                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Database Error! The Name entered is already in the database!");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unknown Error! " + e.Message);
                return View();
            }

            return RedirectToAction("List", "Account", updated.Slug);
        }

        [HttpGet, Route("accounts/delete/{slug}")]
        public IActionResult Delete(string slug)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.Slug == slug);
            return View(account);
        }

        [HttpPost, Route("accounts/delete/{slug}")]
        public IActionResult Delete(string slug, Account deleted)
        {
            Account account = _db.Accounts.FirstOrDefault(
                a => a.Slug == slug && a.Name == deleted.Name);

            if (account != null)
            {
                _db.Accounts.Remove(account);
                _db.SaveChanges();

                return RedirectToAction("List", "Account", new
                {
                    message = string.Format("The Account \"{0}\" has been successfully deleted.",
                        account.Name.Replace('/', '?'))
                });
            }

            return RedirectToAction("List", "Account", new
            {
                message = string.Format("There was an error in deleting the Account \"{0}\".",
                        account.Name.Replace('/', '?'))
            });
        }
    }
}