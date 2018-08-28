using ExpenseApp.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseApp.ViewModels
{
    public class ExpenseCreateViewModel
    {
        public ExpenseForm Form { get; set; }
        public List<Account> Accounts { get; set; }

        public ExpenseCreateViewModel()
        {
            Form = new ExpenseForm();
            Accounts = new List<Account>();
        }

        public ExpenseCreateViewModel(ExpenseDBDataContext db, Guid signedInUserId)
        {
            Employee signedInEmployee = db.Employees
                .Include(e => e.Approver)
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Id == signedInUserId);
            Form = new ExpenseForm(signedInEmployee);
            Accounts = db.Accounts
                .OrderBy(a => a.Name)
                .ToList();
        }
    }
}