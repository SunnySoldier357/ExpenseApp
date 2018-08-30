using ExpenseApp.Models.DB;
using ExpenseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ExpenseApp.Controllers
{
    [Route("admin")]
    public class ApproverController : Controller
    {
        // Private Properties
        private readonly ExpenseDBDataContext _db;

        // Constructors 
        public ApproverController(ExpenseDBDataContext db) => _db = db;

        // Public Methods
        [Route("approvers")]
        public IActionResult List()
        {
            var employees = _db.Employees
                .Include(e => e.Location)
                .OrderBy(e => e.Location.Name)
                .ThenByDescending(e => e.IsAnApprover)
                .ThenBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToList();

            return View(employees);
        }

        [Route("approvers/{id}")]
        public IActionResult Details(string id)
        {
            Employee employee = _db.Employees
                .Include(e => e.Approver)
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == new Guid(id));

            if (null == employee)
                return NotFound();

            return View(employee);
        }

        [HttpGet]
        [Route("approvers/edit/{id}")]
        public IActionResult Edit(string id)
        {
            Employee employee = _db.Employees
                .Include(e => e.Approver)
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == new Guid(id));

            if (null == employee)
                return NotFound();

            var possibleReplacements = _db.Employees
                .Include(e => e.Location)
                .Where(e => e.Location == employee.Location && e.Id != employee.Id)
                .OrderBy(e => e.FullName)
                .ToList();

            var viewModel = new ApproverEditViewModel(employee, possibleReplacements);

            if (!employee.IsAnApprover)
                viewModel.ApproverRemoved = employee.Approver;

            return View(viewModel);
        }

        [HttpPost]
        [Route("approvers/edit/{id}")]
        public IActionResult Edit(string id, ApproverEditViewModel viewModel)
        {
            Employee employee = _db.Employees.Find(new Guid(id));

            viewModel.PupulateFields(_db);

            viewModel.ApproverAdded.Approver = null;
            viewModel.ApproverRemoved.Approver = viewModel.ApproverAdded;

            var possibleReplacements = _db.Employees
                .Include(e => e.Location)
                .Where(e => e.Location == employee.Location &&
                    e.Id != viewModel.ApproverAdded.Id &&
                    e.Id != viewModel.ApproverRemoved.Id)
                .ToList();

            foreach (Employee approvee in possibleReplacements)
                approvee.Approver = viewModel.ApproverAdded;

            _db.SaveChanges();

            return RedirectToAction("List", "Approver", viewModel.ApproverAdded.Id);
        }
    }
}