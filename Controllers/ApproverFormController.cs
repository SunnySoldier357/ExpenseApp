using ExpenseApp.Models.DB;
using ExpenseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static ExpenseApp.Models.Utility;

namespace ExpenseApp.Controllers
{
    [Route("approver")]
    public class ApproverFormController : Controller
    {
        // Private Properties
        private readonly ExpenseDBDataContext _db;

        // Static Properties
        public static Employee SignedInApprover;

        // Constructors
        public ApproverFormController(ExpenseDBDataContext db)
        {
            _db = db;

            if (AuthController.SignedIn)
            {
                SignedInApprover = _db.Employees
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == SignedInApprover.Id);
            }
        }

        // Public Methods
        [Route("")]
        public IActionResult List()
        {
            if (!AuthController.SignedIn)
            {
                return RedirectToAction("Register", "Auth", new
                {
                    returnLocation = (int)ReturnLocation.ApproverHomePage
                });
            }

            if (!AuthController.IsApprover)
                return RedirectToAction("AccessDenied", "Auth");

            var forms = _db.ExpenseForms
                .Include(ef => ef.Employee)
                    .ThenInclude(ef => ef.Location)
                .Where(ef => ef.Employee.Location == SignedInApprover.Location &&
                    ef.Status != Status.Saved && ef.Status != Status.Rejected)
                .OrderByDescending(ef => ef.StatementNumber.Substring(2, 2))
                .ThenByDescending(ef => ef.StatementNumber.Substring(0, 2))
                .ThenBy(ef => ef.StatementNumber.Substring(5, ef.StatementNumber.Length - 8))
                .ThenByDescending(ef => ef.StatementNumber.Substring(ef.StatementNumber.Length - 2))
                .Select(ef => new
                {
                    ef.StatementNumber,
                    ef.Title,
                    ef.Status,
                    EmployeeName = ef.Employee.FullName
                });

            var listings = new List<ExpenseListViewModel>();

            foreach (var form in forms)
                listings.Add(new ExpenseListViewModel(form, true));

            return View(listings);
        }

        [HttpGet]
        [Route("{statementNumber}")]
        public IActionResult Details(string statementNumber)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseForm form = _db.ExpenseForms
                .Include(ef => ef.Entries)
                    .ThenInclude(ee => ee.Account)
                .Include(ef => ef.Entries)
                    .ThenInclude(ee => ee.Receipt)
                .Include(ef => ef.Employee)
                    .ThenInclude(e => e.Approver)
                .Include(ef => ef.Employee.Location)
                .FirstOrDefault(ef => ef.StatementNumber == statementNumber);

            ViewBag.StatementNumber = statementNumber;

            return View(form);
        }

        [HttpPost]
        [Route("{statementNumber}")]
        public IActionResult Details(string statementNumber, ExpenseForm approved)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseForm form = _db.ExpenseForms
                .Include(ef => ef.Employee)
                    .ThenInclude(e => e.Approver)
                .FirstOrDefault(ef => ef.StatementNumber == statementNumber);

            if (approved.StatementNumber == form.StatementNumber)
            {
                form.Status = Status.Approved;
                _db.SaveChanges();

                // TODO: Uncomment this
                // SendEmailToUser(form);

                return RedirectToAction("List", "ApproverForm", statementNumber);
            }
            else
                return NotFound();
        }

        [HttpGet]
        [Route("{statementNumber}/reject")]
        public IActionResult Reject(string statementNumber)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseForm form = _db.ExpenseForms
                .Find(statementNumber);

            return View(form);
        }

        [HttpPost]
        [Route("{statementNumber}/reject")]
        public IActionResult Reject(string statementNumber, ExpenseForm rejected)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseForm form = _db.ExpenseForms
                .Include(ef => ef.Employee)
                    .ThenInclude(e => e.Approver)
                .FirstOrDefault(ef => ef.StatementNumber == statementNumber);

            if (null == rejected.RejectionComment)
            {
                ModelState.AddModelError("", "The rejection comment is a required field.");
                return View(rejected);
            }

            if (rejected.StatementNumber == form.StatementNumber)
            {
                form.Status = Status.Rejected;
                form.RejectionComment = rejected.RejectionComment;
                _db.SaveChanges();

                // TODO: Uncomment this
                // SendEmailToUser(form);

                return RedirectToAction("List", "ApproverForm");
            }
            else
                return NotFound();
        }

        [HttpGet]
        [Route("{statementNumber}/pay")]
        public IActionResult Pay(string statementNumber)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseForm form = _db.ExpenseForms
                .Find(statementNumber);

            return View(form);
        }

        [HttpPost]
        [Route("{statementNumber}/pay")]
        public IActionResult Pay(string statementNumber, ExpenseForm paid)
        {
            if (!AuthController.SignedIn)
                return RedirectToAction("AccessDenied", "Auth");

            ExpenseForm form = _db.ExpenseForms
                .Include(ef => ef.Employee)
                    .ThenInclude(e => e.Approver)
                .FirstOrDefault(ef => ef.StatementNumber == statementNumber);

            if (null == paid.PaymentReceiptNumber)
            {
                ModelState.AddModelError("", "The receipt number is a required field.");
                return View(paid);
            }

            if (paid.StatementNumber == form.StatementNumber)
            {
                form.Status = Status.Paid;
                form.PaymentReceiptNumber = paid.PaymentReceiptNumber;
                _db.SaveChanges();

                // TODO: Uncomment this
                // SendEmailToUser(form);

                return RedirectToAction("List", "ApproverForm", paid.StatementNumber);
            }
            else
                return NotFound();
        }
    }
}