using ExpenseApp.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseApp.ViewModels
{
    public class ApproverEditViewModel
    {
        public Employee ApproverAdded { get; set; }
        public Employee ApproverRemoved { get; set; }

        public List<Employee> PossibleReplacements { get; set; }

        public bool RemovingApproverStatus { get; set; }
        public bool ApproverTransferAllowed => null != PossibleReplacements;

        public ApproverEditViewModel()
        {
            ApproverAdded = new Employee();
            ApproverRemoved = new Employee();

            PossibleReplacements = new List<Employee>();
        }

        public ApproverEditViewModel(Employee employeeToBeEdited, List<Employee> employees)
        {
            if (employeeToBeEdited.IsAnApprover)
                ApproverRemoved = employeeToBeEdited;
            else
                ApproverAdded = employeeToBeEdited;

            if (null == ApproverAdded)
                RemovingApproverStatus = true;
            else
                RemovingApproverStatus = false;

            if (employeeToBeEdited.IsAnApprover)
                ApproverAdded = new Employee();
            else
                ApproverRemoved = new Employee();

            PossibleReplacements = employees;
        }

        public void PupulateFields(ExpenseDBDataContext db)
        {
            ApproverAdded = db.Employees
                .Include(e => e.Approver)
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == ApproverAdded.Id);

            ApproverRemoved = db.Employees
                .Include(e => e.Approver)
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == ApproverRemoved.Id);
        }
    }
}