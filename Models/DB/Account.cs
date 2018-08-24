using System.ComponentModel.DataAnnotations;

namespace ExpenseApp.Models.DB
{
    public class Account
    {
        [Key]
        public string Name { get; set; }
    }
}