using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static ExpenseApp.Models.Utility;

namespace ExpenseApp.Models.DB
{
    public class Location
    {
        // Private Properties
        private string _name;

        // Public Properties
        [Key]
        [Required]
        [Display(Name = "Location")]
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

        // Overridden Methods
        public override bool Equals(object obj)
        {
            Location item = obj as Location;

            if (item == null)
                return false;

            return Name == item.Name;
        }
    }
}