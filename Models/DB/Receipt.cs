using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class Receipt
    {
        // Public Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public byte[] ReceiptImage { get; set; }
        
        public string FileName { get; set; }
    }
}