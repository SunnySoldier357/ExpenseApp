namespace ExpenseApp.Models
{    
    public class ExpenseListing
    {
        public string StatementNumber { get; set; }
        public string Title { get; set; }
        public Status ListingStatus { get; set; }
    }
}