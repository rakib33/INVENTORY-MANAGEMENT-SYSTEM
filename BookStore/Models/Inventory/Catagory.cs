using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    /// <summary>
    /// Mobile,Sweet,Cold Drinks, are some catagory
    /// each catagory has brand.So brand has a foreign key
    /// each product must be under one catagory.such electronic Shop one catagory .
    /// under its many electornic catagory has thouse catagory has unique brand
    /// Mobile Shop is one catagory(mobile,charger,walpaper,glass its subcatagory)
    /// Fast Food is one catagory (cake,sandwich,bargar,chicken its subcatagory)
    /// </summary>
    public class Catagory :BaseClass
    {
        public Catagory()
        {
            this.Brands = new HashSet<Brand>();
        }
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }
        /// <summary>
        /// such Shop catagory has 3 more catagory mini Shop,big shop mid shop
        /// each price is diferent.so when we add product we can 
        /// </summary>
        [StringLength(128)]
        public string Parent_Id { get; set; } //such 

        public virtual ICollection<Brand> Brands { get; set; }
   
    }
}