using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class Invoice :BaseClass
    {
        public  Invoice()
        {
           this.Sales = new HashSet<Sale>();
           this.Purchases = new HashSet<Purchase>();
           this.Transactions = new HashSet<Transaction>();
        }
        //unique 5digitunique        
        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Required]
        public DateTime? InvoiceDate { get; set; }


        [Required]
        [StringLength(20)]
        public string InvoiceType { get; set; } //Purchase,Sale,Salary for employee

        //manually take for previous Saved Customer/old customer
        //[ForeignKey("ApplicationUser")]

        [Required]
        [StringLength(128)]
        public string Customer_Id { get; set; } 
        //public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } //supplier name if Invoice type= purchase

        //[Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        public decimal Total { get; set; } //Total price

        [Required]
        public decimal Discount { get; set; } //any discount

        #region EmployeeExtraBillEntity

            //This is also for Employee Profit
            public decimal BonusOrExtra { get; set; } //any discount

            public decimal MobileBill { get; set; }

            public decimal TransportBill { get; set; }

        #endregion

        #region EmployeerProfitExpense

            [Display(Name="House Or Room Rent")]
            public decimal RentExpense { get; set; }
            public decimal ElectricityBill { get; set; }
            public decimal GassBill { get; set; }
            public decimal WaterBill { get; set; }
            public decimal OtherExpense { get; set; }

            #endregion

            [Required]
        public decimal Payable { get; set; } //Total - Discount

        [Required]
        public decimal Paid { get; set; } // how he paid 

        [Required]
        public decimal Due { get; set; }  //Payable - Paid

        [StringLength(50)]
        public string Status { get; set; } //Pending = you can add Payable from SaleInvoice
                                            //After Pay some amount if has due then Status =Accept and you have to pay due in transaction.if full pay then status=Paid

        [StringLength(150)]
        public string Description { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public class VM_InvoiceWiseLedger
    {
        public string InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public decimal TotalBuyQty { get; set; }
        public decimal AvgBuyRate { get; set; }
        public decimal TotalBuyAmount { get; set; }
        public decimal TotalSaleQty { get; set; }
        public decimal AvgSaleRate { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal TotalPaid { get; set; }

        public decimal BuyPaidAmt { get; set; }
        public decimal SalePaidAmt { get; set; }
        public decimal SalaryPaidAmt { get; set; }
        public decimal ProfitExpensePaidAmt { get; set; }
        public decimal RealizedGainLoss { get; set; }
    }
}