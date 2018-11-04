using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;
using BookStore.Models.ViewModel;

namespace BookStore.Models.Inventory
{
    public class Purchase :BaseClass
    {
        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Required]
        public DateTime? InvoiceDate { get; set; }

        [Required]
        public decimal  Qty { get; set; }

        [Required]
        public decimal BuyRate { get; set; }

        [Required]
        public decimal  BuyTotal { get; set; }

        [Required]
        public decimal SaleRate { get; set; }

        [Required]
        public decimal SaleTotal { get; set; }

        //status pending can not be saled .sale this purchase product only its approved
        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [ForeignKey("Product")]
        [StringLength(128)]
        public string Product_Id { get; set; }
        public virtual Product Product { get; set; }


        [ForeignKey("Invoice")]
        [StringLength(128)]
        public string Invoice_Id { get; set; }
        public virtual Invoice Invoice { get; set; }

        [Display(Name="Supplier Name")]
        [ForeignKey("ApplicationUser")]
        public string FK_Supplier_Id { get; set; } //FK_<TableName>_<primary Key>
        public virtual ApplicationUser ApplicationUser { get; set; }

      }

    public class PurchaseViewModel {
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string Invoice { get; set; }

        //created Date
        [Required]
        public DateTime? InvoiceDate { get; set; }

        public List<BulkProduct> Purchase { get; set; }
    
        public PurchaseViewModel()
        {
          Purchase = new List<BulkProduct>();
        }        
    }

    public class PurchaseDislayModel
    {
      public string Id { get; set; }
      public string InvoiceNo { get; set; }
      public string SupplierName { get; set; }
      public string ProductName { get; set; }
       
      public decimal Qty {get; set;}
      public decimal BuyRate {get; set;}
      public decimal BuyTotal {get; set;} 
      public decimal SaleRate {get; set;} 
      public decimal SaleTotal {get; set;}
      public string Status { get; set; }
      public string CreatedBy {get; set;}
      public string CreatedDate { get; set; }
   
      
                            
    }
}