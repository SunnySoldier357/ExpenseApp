using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class Employee
    {
        public Employee() => Forms = new List<ExpenseForm>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Employee Approver { get; set; }

        public List<ExpenseForm> Forms;

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public Location Location { get; set; }

        [NotMapped]
        [Display(Name = "Approver?")]
        public bool IsAnApprover => null == Approver;

        [NotMapped]
        [Display(Name = "Approver")]
        public string FullName => FirstName + " " + LastName;
    }
}