using ExpenseApp.Models.DB;

namespace ExpenseApp.ViewModels
{
    public class ExpenseListViewModel
    {
        public string StatementNumber { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }

        public ExpenseListViewModel(dynamic form)
        {
            StatementNumber = form.StatementNumber;
            Title = form.Title;
            Status = form.Status;
        }
    }
}