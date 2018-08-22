using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class ExpenseEntry
    {
        public string Id { get; set; }

        /*
        public string ExpenseFormId { get; set; }
        public virtual ExpenseForm ExpenseForm { get; set; }
        */
        public DateTime Date { get; set; }
        public virtual Account Account { get; set; }
        public string Description { get; set; }
        public string Cost { get; set; }
        
        [NotMapped]
        public decimal Hotel { get; set; }
        
        [NotMapped]
        public decimal Transport { get; set; }
        
        [NotMapped]
        public decimal Fuel { get; set; }
        
        [NotMapped]
        public decimal Meals { get; set; }
        
        [NotMapped]
        public decimal Phone { get; set; }
        
        [NotMapped]
        public decimal Entertainment { get; set; }
        
        [NotMapped]
        public decimal Misc { get; set; }
        
        [NotMapped]
        public decimal Total 
        {
            get => Hotel + Transport + Fuel + Meals + Phone
                + Entertainment + Misc;
        }

        public string ReceiptId { get; set; } = null;
    }
}