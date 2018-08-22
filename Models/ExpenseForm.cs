using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenseApp.Models
{
    public enum Status : int
    {
        Saved,
        Submitted,
        Approved,
        Rejected,
        Paid
    }
    
    public class ExpenseForm
    {
        [Key]
        public string StatementNumber { get; set; }
        
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        
        public string Title { get; set; }
        public string Purpose { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Project { get; set; }
        public string Comment { get; set; }
        public string RejectionComment { get; set; }
        public Status Status { get; set; }
        
        public List<ExpenseEntry> Entries { get; set; }
    }
}