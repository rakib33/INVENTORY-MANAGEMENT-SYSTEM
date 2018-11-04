using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class Brand :BaseClass
    {
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name="Country")]
        [StringLength(100)]
        public string CountryName { get; set; }

        [StringLength(50)]
        public string ISIN { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

      //  [Required]
        public string Status { get; set; }

        [ForeignKey("Catagory")]
        [StringLength(128)]
        public string Catagory_Id { get; set; }
        public virtual Catagory Catagory { get; set; }

    }

    public class BrandViewModel
    {
        public string Catagory_Id { get; set; }
        public List<Brand> Brand { get; set; }

        public BrandViewModel()
        {
            Brand = new List<Brand>();
        }
    
    }
}