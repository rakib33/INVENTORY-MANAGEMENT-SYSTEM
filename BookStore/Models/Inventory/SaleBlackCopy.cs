using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    //All Sale has a black copy here
    //also all edit exchange of any Invoice has a black carbon copy here 
    //So that any reason we can render any Invoice transaction to user emergency 
    public class SaleBlackCopy :BaseClass
    {

        [Required]
        public decimal Stock { get; set; }

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

        public decimal FreeStock { get; set; }

        public decimal FreeGiven { get; set; }

        [Required]
        //why you return purchase product
        [StringLength(200)]
        public string Remarks { get; set; }

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

        [ForeignKey("ApplicationUser")]
        public string FK_Customer_Id { get; set; } //FK_<TableName>_<primary Key>
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}