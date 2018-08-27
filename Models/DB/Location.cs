using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static ExpenseApp.Models.Utility;

namespace ExpenseApp.Models.DB
{
    public class Location
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

        public override bool Equals(object obj)
        {
            var item = obj as Location;

            if (item == null)
                return false;

            return Name == item.Name;
        }
    }
}