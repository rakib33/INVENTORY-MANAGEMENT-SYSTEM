using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class Product : BaseClass
    {
      
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [Display(Name ="Cost Price")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Cost Price; Maximum Two Decimal Points.")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid  Cost Price; Max 18 digits")]
        public decimal CostPrice { get; set; }

        [Display(Name ="Sale Price")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Cost Price; Maximum Two Decimal Points.")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid  Cost Price; Max 18 digits")]
        public decimal SalePrice { get; set; }

        [Display(Name = "Noted Cost Price")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Cost Price; Maximum Two Decimal Points.")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid  Cost Price; Max 18 digits")]
        public decimal DisplayCostPrice { get; set; }  // used to display customer that its actual but actulay its damm


        [Display(Name ="Bar Code")]
        [StringLength(20)]
        public string BarCode { get; set; }

        [StringLength(128)]
        public string ParentId { get; set; }

        //fk of catagory table
        [Display(Name ="Catagory")]
        [StringLength(128)]
        public string Catagory_Id { get; set; }

        //fk of brand table nullable
        [Display(Name ="Brand")]
        [StringLength(128)]
        public string Brand_Id { get; set; }

        public virtual ICollection<UserImage> Images { get; set; }
    }



    //public class ProductBalanceSheet_ViewModel
    //{    
   
    //public string       ProductId {get;set;}
    //public string       Name {get;set;}
    //public string       BarCode {get;set;}

    //public decimal?     TotalBuyQty { get; set; }
    //public decimal?     AvgBuyRate {get;set;}
    //public decimal?     TotalBuyAmount { get; set; }

    //public decimal?     TotalSaleQty { get; set; }
    //public decimal?     AvgSaleRate {get;set;}
    //public decimal?     TotalSaleAmount { get; set; }
    //public decimal      StockQty { get; set; }
    //public decimal?     CurrentSaleRate { get; set; }
    //public decimal?     CurrentMarketPrice { get; set; }
    //public decimal?     RealizedGain { get; set; }
    //public decimal?     UnRealizedGain { get; set; }  
    
    //}
    public class ProductWiseViewModel
    {
        public DateTime? InvoiceDate { get; set; }
        //public string InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public string ProductId { get; set; }
       // public DateTime? CreatedDate { get; set; }       
        public string Name { get; set; }
        //public string BarCode { get; set; }     
        public decimal? BuyQty { get; set; }
        public decimal? BuyAmount { get; set; }
        public decimal? SaleQty { get; set; }
        public decimal? SaleAmount { get; set; }
        //public decimal Qty { get; set; }
        public decimal CurrentStockQty { get; set; }
        public decimal? AvgCost { get; set; } //AvgPurchaseRate
        public decimal? TotalCost { get; set; }

        public decimal? MarketRate { get; set; }
        public decimal? MarketValue { get; set; }
        public decimal? RealizedGain { get; set; }
        public decimal? UnRealizedGain { get; set; }

        public int Sequence { get; set; }

        public List<ProductWiseViewModel> list = new List<ProductWiseViewModel>();
    }

   

}