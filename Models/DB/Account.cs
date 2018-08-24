using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ExpenseApp.Models.DB
{
    public class Account
    {
        private string _name;

        [Key]
        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                TextInfo textInfo = new CultureInfo("en-US", true).TextInfo;
                _name = textInfo.ToTitleCase(value.Trim());
            }
        }

        [NotMapped]
        public string Slug
        {
            get
            {
                if (null == Name)
                    return null;

                string slug = Name.ToLower();

                // Replace slashes with dashes
                slug = slug.Replace('/', '-');

                //  Remove Invalid Chars           
                slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

                // Convert multiple spaces into one space   
                slug = Regex.Replace(slug, @"\s+", " ").Trim();

                // Cut and trim 
                slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();

                // Replace spaces as strings
                slug = Regex.Replace(slug, @"\s", "-");

                return slug;
            }
        }
    }
}