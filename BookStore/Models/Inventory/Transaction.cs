using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class Transaction :BaseClass
    {
        //when due of a invoice is paid then transaction made
        //create date is transaction date
        //while paid due is decrease from due field of invoice table

        //kar kas theke taka nilam/ je buy korse se naki onno kew jemon chele meye
        [Required]
        public string ReceiveFrom { get; set; }

        //receiver phone number
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; } //Sale,Purchase,Salary,Expense,Others Profit

        [StringLength(50)]
        public string ExpenseType { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }  //Amount That is Paid

        //public decimal Amount { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }
        //[unique]
        [Required]
        [StringLength(50)]
        public string TransactionNo { get; set; }

        [StringLength(128)]
        public string Customer_Id { get; set; } 

        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; } //Will be Invoice No

        [ForeignKey("Invoice")]
        [StringLength(128)]
        public string Invoice_Id { get; set; }
        public virtual Invoice Invoice { get; set; }
      

    }
}