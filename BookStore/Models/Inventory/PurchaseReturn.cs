using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class PurchaseReturn :BaseClass
    {

        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Required]
        public decimal Qty { get; set; }
        
        [Required]
        public decimal BuyRate { get; set; }
        
        [Required]
        public decimal BuyTotal { get; set; }
        
        [Required]
        public decimal SaleRate { get; set; }
        
        [Required]
        public decimal SaleTotal { get; set; }

        [Required]
        //why you return purchase product
        [StringLength(200)]
        public string Remarks { get; set; }

        [ForeignKey("Purchase")]
        [StringLength(128)]
        public string Purchase_Id { get; set; }
        public virtual Purchase Purchase { get; set; }

        [ForeignKey("Product")]
        [StringLength(128)]
        public string Product_Id { get; set; }
        public virtual Product Product { get; set; }

    
        [ForeignKey("ApplicationUser")]
        public string FK_Supplier_Id { get; set; } //FK_<TableName>_<primary Key>
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}