using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExpenseApp.Models.Utility;

namespace ExpenseApp.Models.DB
{
    public class Account
    {
        // Private Properties
        private string _name;

        // Public Properties
        [Key]
        [Required]
        [Display(Name = "Account")]
        public string Name
        {
            get => _name;
            set => _name = ToTitleCase(value);
        }

        [NotMapped]
        public string Slug => Slugify(Name);
    }
}