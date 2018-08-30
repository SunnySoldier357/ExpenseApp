using ExpenseApp.Models.DB;

namespace ExpenseApp.ViewModels
{
    public class ExpenseListViewModel
    {
        // Public Properties
        public Status Status { get; set; }

        public string EmployeeName { get; set; }
        public string StatementNumber { get; set; }
        public string Title { get; set; }

        // Constructors
        public ExpenseListViewModel(dynamic form, bool approverController = false)
        {
            StatementNumber = form.StatementNumber;
            Title = form.Title;
            if (!approverController)
            {
                if (form.Status == Status.Paid)
                    Status = Status.Approved;
                else
                    Status = form.Status;
            }
            else
                Status = form.Status;

            if (approverController)
                EmployeeName = form.EmployeeName;
        }
    }
}