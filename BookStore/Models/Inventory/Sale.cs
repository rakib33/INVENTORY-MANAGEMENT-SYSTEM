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
    public class Sale :BaseClass
    {
        //created date will be the sale date

        [Required]
        public DateTime? InvoiceDate { get; set; }

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

        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        public string SubCatagory_Id { get; set; }

        [Required]
        public string MainCatagory_Id { get; set; }


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


        //NOT DataBase Field 
       
    }

    public class SaleViewModel
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Your must provide a PhoneNumber")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
       // [RegularExpression(@"^(?:\+88|01)?\d{11}\r?$", ErrorMessage = "Not a valid Phone number")]

        public string CustomerPhone { get; set; }
        [Required]
        public string Invoice { get; set; }

        //created Date
        [Required]
        public DateTime? InvoiceDate { get; set; }

       

        [Required]
        public List<BulkProduct> Sale { get; set; }

        public SaleViewModel()
        {
            this.Sale = new List<BulkProduct>();
        }
    }

    
    public class SaleDisplayModel
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhone { get; set; }
        [Required]
        public string Invoice { get; set; }

        //created Date
        [Required]
        public DateTime? InvoiceDate { get; set; }

        [Required]
        public List<Sale> Sale { get; set; }

       
    }


  
}