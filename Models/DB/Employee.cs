using System.ComponentModel.DataAnnotations;

namespace ExpenseApp.Models.DB
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }
        
        public string ApproverId { get; set; }
        public virtual Employee Approver { get; set; }
        
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsAnApprover { get; set; }
    }
}