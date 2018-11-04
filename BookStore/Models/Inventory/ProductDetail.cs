using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class ProductDetail:BaseClass
    {
        public byte[] ImageStream { get; set; }

        [ForeignKey("Product")]
        public string FK_Product_Id { get; set; } //FK_<TableName>_<primary Key>
        public virtual Product Product { get; set; }
    }
}