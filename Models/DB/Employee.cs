using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class Employee
    {
        public Employee()
        {
            Forms = new List<ExpenseForm>();
            IsAnApprover = false;
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Guid ApproverId { get; set; }
        
        [Required]
        [ForeignKey("ApproverId")]
        public virtual Employee Approver { get; set; }

        public List<ExpenseForm> Forms;

        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsAnApprover { get; set; }
    }
}