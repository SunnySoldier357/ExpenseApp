using System.Text.RegularExpressions;

namespace ExpenseApp.Models
{
    public static class Utility
    {
        /// <summary>
        /// Converts a string into a slug (clean, human-readable url)
        /// </summary>
        /// <param name="text">The string to convert to a slug.</param>
        /// <returns>The slug created from <paramref name="text"/>.</returns>
        public static string Slugify(string text)
        {
            if (null == text)
                return null;

            string slug = text.ToLower();

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