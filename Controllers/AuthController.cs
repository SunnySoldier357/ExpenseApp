using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseApp.Models.DB;
using ExpenseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        // Static Properties
        public static bool IsApprover = false;
        public static bool SignedIn = false;

        // Private Properties
        private readonly ExpenseDBDataContext _db;

        // Constructors
        public AuthController(ExpenseDBDataContext db) => _db = db;

        // Public Methods
        [Route("error")]
        public IActionResult AccessDenied() => View();

        [HttpGet]
        [Route("register/{returnLocation:int}")]
        public IActionResult Register(int returnLocation)
        {
            var employees = _db.Employees.Count();

            ViewBag.FirstEmployee = employees == 0;
            ViewBag.Locations = _db.Locations
                .OrderBy(l => l.Name)
                .ToList();

            return View(new Employee());
        }

        [HttpPost]
        [Route("register/{returnLocation:int}")]
        public IActionResult Register(int returnLocation, Employee newEmployee)
        {
            if (newEmployee.Id == new Guid())
            {
                ViewBag.FirstEmployee = _db.Employees.Count() == 0;
                ViewBag.Locations = new List<Location>();

                ModelState.AddModelError("",
                    "You must use the Microsoft Teams Application to view this website.");
                return View(newEmployee);
            }

            var employee = _db.Employees
                .Include(e => e.Approver)
                .FirstOrDefault(e => e.Id == newEmployee.Id);

            if (employee != null)
            {
                FormController.SignedInEmployee = employee;
                ApproverFormController.SignedInApprover = employee;
                SignedIn = true;
                IsApprover = employee.IsAnApprover;

                return rerouteSignedInUser(employee, (ReturnLocation)returnLocation);
            }

            if (string.IsNullOrWhiteSpace(employee.FirstName))
                ModelState.AddModelError("", "First Name is a required field.");
            
            if (string.IsNullOrWhiteSpace(employee.LastName))
                ModelState.AddModelError("", "Last Name is a required field.");
            
            if (string.IsNullOrWhiteSpace(employee.Location.Name))
                ModelState.AddModelError("", "Location is a required field.");

            if (!ModelState.IsValid)
            {
                ViewBag.FirstEmployee = _db.Employees.Count() == 0;
                ViewBag.Locations = _db.Locations
                    .OrderBy(l => l.Name)
                    .ToList();

                return View(newEmployee);
            }

            if (_db.Locations.Count() != 0)
                newEmployee.Location = _db.Locations
                    .Find(newEmployee.Location.Name);

            var approver = _db.Employees
                .Include(e => e.Location)
                .Include(e => e.Approver)
                .ToList()
                .FirstOrDefault(e => e.IsAnApprover == true && e.Location.Name == newEmployee.Location.Name);

            newEmployee.Approver = approver;

            var user = _db.Employees.Add(newEmployee);
            _db.SaveChanges();

            FormController.SignedInEmployee = user.Entity;
            ApproverFormController.SignedInApprover = user.Entity;
            SignedIn = true;
            IsApprover = newEmployee.IsAnApprover;

            return rerouteSignedInUser(newEmployee, (ReturnLocation)returnLocation);
        }

        // Private Methods
        private IActionResult rerouteSignedInUser(Employee employee, ReturnLocation returnLocation)
        {
            if (returnLocation == ReturnLocation.UserHomePage)
                return RedirectToAction("Index", "Form");
            if (employee.IsAnApprover && returnLocation == ReturnLocation.ApproverHomePage)
                return RedirectToAction("List", "ApproverForm");
            else if (employee.IsAnApprover && returnLocation == ReturnLocation.AdminHomePage)
                return RedirectToAction("Index", "Admin");
            else
            {
                return RedirectToAction("AccessDenied", "Auth", new
                {
                    returnLocation = "nowhere"
                });
            }
        }
    }
}