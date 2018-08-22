namespace ExpenseApp.Models
{
    public enum Status
    {
        Saved,
        Submitted,
        Approved,
        Rejected,
        Paid
    }
    
    public class ExpenseListing
    {
        public string Title { get; set; }
        public Status ListingStatus { get; set; }
    }
}