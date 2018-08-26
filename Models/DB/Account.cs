using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static ExpenseApp.Models.Utility;

namespace ExpenseApp.Models.DB
{
    public class Account
    {
        private string _name;

        [Key]
        [Required]
        public string Name
        {
            get => _name;
            set
            {
                TextInfo textInfo = new CultureInfo("en-US", true).TextInfo;
                _name = textInfo.ToTitleCase(value.Trim());
            }
        }

        [NotMapped]
        public string Slug => Slugify(Name);
    }
}