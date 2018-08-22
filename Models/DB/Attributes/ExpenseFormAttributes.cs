using System.ComponentModel.DataAnnotations;

namespace ExpenseApp.Models.DB
{
    public class ExpenseFormAttributes
    {
        [Key]
        [Display(Name = "Statement Number: ")]
        public object StatementNumber { get; set; }
    }
}