using ExpenseApp.Models.DB;
using Microsoft.EntityFrameworkCore;
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

        public ExpenseCreateViewModel(ExpenseDBDataContext db, Employee SignedInEmployee)
        {
            Form = db.ExpenseForms
                .Include(ef => ef.Employee)
                    .ThenInclude(e => e.Location)
                .Include(ef => ef.Employee.Approver)
                .Include(ef => ef.Entries)
                    .ThenInclude(ee => ee.Account)
                .FirstOrDefault(ef => ef.StatementNumber == "0818-MICROSOFT-AURORA-01");
            Accounts = db.Accounts
                .OrderBy(a => a.Name)
                .ToList();
        }
    }
}