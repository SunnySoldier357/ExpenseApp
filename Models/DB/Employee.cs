using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class Employee
    {
        // Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Employee Approver { get; set; }

        public List<ExpenseForm> Forms;

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public Location Location { get; set; }

        [NotMapped]
        [Display(Name = "Approver?")]
        public bool IsAnApprover => null == Approver;

        [NotMapped]
        [Display(Name = "Approver")]
        public string FullName => FirstName + " " + LastName;

        // Constructors
        public Employee() => Forms = new List<ExpenseForm>();

        public Employee(string id) => Id = new Guid(id);
    }
}