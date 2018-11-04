using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.Inventory
{
    public class DbBackUp :BaseClass
    {
        [Required]
        [StringLength(50)]
        public string DriveName { get; set; } //C: D: upper case

        [StringLength(50)]
        public string DirectoryPath { get; set; } // "E:\\BackUp"; you just input BackUp here
    }
}