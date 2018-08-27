using ExpenseApp.Models.DB;
using ExpenseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ExpenseDBDataContext _db;

        public AdminController(ExpenseDBDataContext db) => _db = db;

        public IActionResult Index()
        {
            // TODO: Implement Admin Home Page
            return View();
        }

        // Accounts

        [Route("accounts/{message?}")]
        public IActionResult AccountList(string message)
        {
            if (message != null)
                ViewBag.Message = message.Replace('?', '/');

            var accounts = _db.Accounts
                .OrderBy(a => a.Name)
                .ToArray();

            return View(accounts);
        }

        [HttpGet, Route("accounts/create")]
        public IActionResult AccountCreate() => View();

        [HttpPost, Route("accounts/create")]
        public IActionResult AccountCreate(Account account)
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

            return RedirectToAction("AccountList", "Admin", account.Slug);
        }

        [HttpGet, Route("accounts/edit/{slug}")]
        public IActionResult AccountEdit(string slug)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.Slug == slug);

            if (null == account)
                return NotFound();

            return View(account);
        }

        [HttpPost, Route("accounts/edit/{slug}")]
        public IActionResult AccountEdit(string slug, Account updated)
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

            return RedirectToAction("AccountList", "Admin", updated.Slug);
        }

        [HttpGet, Route("accounts/delete/{slug}")]
        public IActionResult AccountDelete(string slug)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.Slug == slug);
            return View(account);
        }

        [HttpPost, Route("accounts/delete/{slug}")]
        public IActionResult AccountDelete(string slug, Account deleted)
        {
            Account account = _db.Accounts.FirstOrDefault(
                a => a.Slug == slug && a.Name == deleted.Name);

            if (account != null)
            {
                _db.Accounts.Remove(account);
                _db.SaveChanges();

                return RedirectToAction("AccountList", new
                {
                    message = string.Format("The Account \"{0}\" has been successfully deleted.",
                        account.Name.Replace('/', '?'))
                });
            }

            return RedirectToAction("AccountList", new
            {
                message = string.Format("There was an error in deleting the Account \"{0}\".",
                        account.Name.Replace('/', '?'))
            });
        }

        // Approvers

        [Route("approvers")]
        public IActionResult ApproverList()
        {
            var employees = from e in _db.Employees.ToList()
                            orderby e.Location.Name, e.IsAnApprover descending, e.LastName, e.FirstName
                            select e;

            return View(employees);
        }

        [Route("approvers/{id}")]
        public IActionResult ApproverDetails(string id)
        {
            Employee employee = _db.Employees
                .Include(e => e.Approver)
                .FirstOrDefault(e => e.Id == new Guid(id));

            if (null == employee)
                return NotFound();

            return View(employee);
        }

        [HttpGet, Route("approvers/edit/{id}")]
        public IActionResult ApproverEdit(string id)
        {
            Employee employee = _db.Employees
                .Include(e => e.Approver)
                .FirstOrDefault(e => e.Id == new Guid(id));

            if (null == employee)
                return NotFound();

            var possibleReplacements = _db.Employees
                .Where(e => e.Location == employee.Location && e.Id != employee.Id)
                .OrderBy(e => e.FullName)
                .ToList();

            var viewModel = new ApproverEditViewModel(employee, possibleReplacements);

            if (!employee.IsAnApprover)
                viewModel.ApproverRemoved = employee.Approver;

            return View(viewModel);
        }

        [HttpPost, Route("approvers/edit/{id}")]
        public IActionResult ApproverEdit(string id, ApproverEditViewModel viewModel)
        {
            Employee employee = _db.Employees.Find(new Guid(id));

            viewModel.PupulateFields(_db);

            viewModel.ApproverAdded.Approver = null;
            viewModel.ApproverRemoved.Approver = viewModel.ApproverAdded;

            var possibleReplacements = _db.Employees
                .Where(e => e.Location == employee.Location && 
                    e.Id != viewModel.ApproverAdded.Id && 
                    e.Id != viewModel.ApproverRemoved.Id)
                .ToList();

            foreach (Employee approvee in possibleReplacements)
                approvee.Approver = viewModel.ApproverAdded;

            _db.SaveChanges();

            return RedirectToAction("ApproverList", "Admin", viewModel.ApproverAdded.Id);
        }

        // Locations

        [Route("locations/{message?}")]
        public IActionResult LocationList(string message)
        {
            if (message != null)
                ViewBag.Message = message.Replace('?', '/');

            var locations = _db.Locations
                .OrderBy(l => l.Name)
                .ToArray();

            return View(locations);
        }

        [HttpGet, Route("locations/create")]
        public IActionResult LocationCreate() => View();

        [HttpPost, Route("locations/create")]
        public IActionResult LocationCreate(Location location)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _db.Locations.Add(location);
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

            return RedirectToAction("LocationList", "Admin", location.Slug);
        }

        [HttpGet, Route("locations/edit/{slug}")]
        public IActionResult LocationEdit(string slug)
        {
            Location location = _db.Locations.FirstOrDefault(l => l.Slug == slug);

            if (null == location)
                return NotFound();

            return View(location);
        }

        [HttpPost, Route("locations/edit/{slug}")]
        public IActionResult LocationEdit(string slug, Location updated)
        {
            Location location = _db.Locations.FirstOrDefault(l => l.Slug == slug);

            if (null == location)
                return NotFound();

            try
            {
                _db.Locations.Remove(location);
                _db.Locations.Add(updated);

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

            return RedirectToAction("LocationList", "Admin", updated.Slug);
        }

        [HttpGet, Route("locations/delete/{slug}")]
        public IActionResult LocationDelete(string slug)
        {
            Location location = _db.Locations.FirstOrDefault(l => l.Slug == slug);
            return View(location);
        }

        [HttpPost, Route("locations/delete/{slug}")]
        public IActionResult LocationDelete(string slug, Location deleted)
        {
            Location location = _db.Locations.FirstOrDefault(
                l => l.Slug == slug && l.Name == deleted.Name);

            if (location != null)
            {
                _db.Locations.Remove(location);
                _db.SaveChanges();

                return RedirectToAction("LocationList", new
                {
                    message = string.Format("The Location \"{0}\" has been successfully deleted.",
                        location.Name.Replace('/', '?'))
                });
            }

            return RedirectToAction("LocationList", new
            {
                message = string.Format("There was an error in deleting the Location \"{0}\".",
                        location.Name.Replace('/', '?'))
            });
        }
    }
}