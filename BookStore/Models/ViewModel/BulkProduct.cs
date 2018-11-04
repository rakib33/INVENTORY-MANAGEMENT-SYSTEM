using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace BookStore.Models.ViewModel
{
    public class BulkProduct
    {

        [Required]
        public string MainCatagoryId { get; set; }

        //public List<SelectListItem> MainCatagory //using System.Web.WebPages.Html; for sale return dropdown
        //{
        //    get;
        //    set;
        //}
        

        [Required]
        public string SubCatagoryId { get; set; }

        //public List<SelectListItem> SubCatagory //using System.Web.WebPages.Html; for sale return dropdown
        //{
        //    get;
        //    set;
        //}

        [Required]
        public string Product_Id { get; set; }

        //public List<SelectListItem> ProductList //using System.Web.WebPages.Html; for sale return dropdown
        //{
        //    get;
        //    set;
        //}

        [Required]
        public decimal Stock { get; set; }

        [Required]
        public decimal Qty { get; set; }

        public decimal BuyRate { get; set; }

        public decimal BuyTotal { get; set; }

        public decimal Discount { get; set; }

        [Required]
        public decimal SaleRate { get; set; }

        [Required]
        public decimal SaleTotal { get; set; }

        [Required]
        public string Status { get; set; }
    }

}