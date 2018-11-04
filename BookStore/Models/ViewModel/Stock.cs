using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class Stock :BaseClass
    {
        // when any product bieng purchsed then coresponding product Stock of this table
        //will be increased
        [Key,Column(Order=1)]
        [ForeignKey("Product")]
        [StringLength(128)]
        public string Product_Id { get; set; }
        public virtual Product Product { get; set; }
        public decimal ProductInStock { get; set; }

    }

    public class StockViewModel
    {
        public string Product_Id { get; set; }
        public string Name { get; set; }
          
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string BarCode { get; set; }  
        public decimal? PurchaseQty { get; set; }
        public decimal? SaleQty { get; set; }
        public decimal  ProductInStock { get; set; }
    
    
    }
}