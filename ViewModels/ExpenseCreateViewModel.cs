using ExpenseApp.Models.DB;
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
            Form = new ExpenseForm(SignedInEmployee);
            Accounts = db.Accounts
                .OrderBy(a => a.Name)
                .ToList();
        }
    }
}