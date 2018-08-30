using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class ExpenseForm : IValidatableObject
    {
        // Public Properties
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

        [Display(Name = "Rejection Comment")]
        public string RejectionComment { get; set; }

        public Status Status { get; set; }

        [InverseProperty("Form")]
        public List<ExpenseEntry> Entries { get; set; }

        // Constructors 
        public ExpenseForm()
        {
            Entries = new List<ExpenseEntry>();
        }

        public ExpenseForm(Employee signedInEmployee)
        {
            Entries = new List<ExpenseEntry>();
            Employee = signedInEmployee;
        }

        // Interface Implementations
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (From.Month != To.Month)
            {
                yield return new ValidationResult(
                    "The Period From data must have the same month as the Period To date."
                );
            }

            if (From.Year != To.Year)
            {
                yield return new ValidationResult(
                    "The Period From data must have the same year as the Period To date."
                );
            }

            if (!Project.Contains('-') || Project.Split('-').Length != 2)
            {
                yield return new ValidationResult(
                    "Project must follow this convention: CLIENT-PROJECT.", new[] { "Project" }
                );
            }
            
            if (Entries.Count != 0)
            {
                foreach (var entry in Entries)
                {
                    if (entry.Date.CompareTo(From) < 0)
                    {
                        yield return new ValidationResult(
                            "The From date has to be the same or earlier than any of the Expense " +
                            "Claim dates (" + entry.Date.ToShortDateString() + ").",
                            new[] { "From" }
                        );
                    }

                    if (entry.Date.CompareTo(To) > 0)
                    {
                        yield return new ValidationResult(
                            "The To date has to be the same or later than any of the Expense " +
                            "Claim dates (" + entry.Date.ToShortDateString() + ").",
                            new[] { "To" }
                        );
                    }
                }
            }
        }
    }
}