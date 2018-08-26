using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenseApp.Models.DB
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
        public ExpenseForm() : this(null) { }

        public ExpenseForm(Employee signedInEmployee)
        {
            Entries = new List<ExpenseEntry>();
            Employee = signedInEmployee;
        }
        
        [Key]
        [Display(Name = "Statement Number: ")]
        public string StatementNumber { get; set; }
        
        public Employee Employee { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Purpose { get; set; }
        
        [Required]
        public DateTime From { get; set; }
        
        [Required]
        public DateTime To { get; set; }
        
        [Required]
        public string Project { get; set; }
        
        public string Comment { get; set; }
        public string RejectionComment { get; set; }
        public Status Status { get; set; }
        
        public List<ExpenseEntry> Entries { get; set; }
    }
}