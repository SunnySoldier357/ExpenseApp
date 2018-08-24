using ExpenseApp.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly EmployeeDataContext _db;

        public AdminController(EmployeeDataContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // TODO: Implement Admin Home Page
            return View();
        }

        [Route("accounts/{message?}")]
        public IActionResult List(string message)
        {
            ViewBag.Message = message;

            var accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToArray();

            return View(accounts);
        }

        [HttpGet, Route("accounts/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("accounts/create")]
        public IActionResult Create(Account account)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _db.Add(account);
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

            return RedirectToAction("List", "Admin", account.Slug);
        }

        [HttpGet, Route("accounts/edit/{slug}")]
        public IActionResult Edit(string slug)
        {
            var account = _db.Accounts.FirstOrDefault(a => a.Slug == slug);

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
                _db.Remove(account);
                _db.Add(updated);

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

            return RedirectToAction("List", "Admin", updated.Slug);
        }

        [HttpGet, Route("accounts/delete/{slug}")]
        public IActionResult Delete(string slug)
        {
            var account = _db.Accounts.FirstOrDefault(a => a.Slug == slug);
            return View(account);
        }

        [HttpPost, Route("accounts/delete/{slug}")]
        public IActionResult Delete(string slug, Account deleted)
        {
            var account = _db.Accounts.FirstOrDefault(
                a => a.Slug == slug && a.Name == deleted.Name);

            if (account != null)
            {
                _db.Accounts.Remove(account);
                _db.SaveChanges();

                return RedirectToAction("List", new
                {
                    message = string.Format("The Account \"{0}\" has been successfully deleted.", 
                        account.Name)
                });
            }

            return RedirectToAction("List", new
            {
                message = string.Format("There was an error in deleting the Account \"{0}\".",
                        account.Name)
            });
        }
    }
}