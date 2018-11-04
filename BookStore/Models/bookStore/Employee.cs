using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.Models.bookStore
{
    public class Employee :BaseClass
    {
                  
            [Required]
            [StringLength(50)]
            public string Name
            {
                get;
                set;
            }
            
            [Required]
            [StringLength(100)]
            public string Description
            {
                get;
                set;
            }
            public decimal? Salary
            {
                get;
                set;
            }
            [Required]
            [StringLength(50)]
            public string Country
            {
                get;
                set;
            }
           
            public DateTime DateofBirth
            {
                get;
                set;
            }
            public bool IsActive
            {
                get;
                set;
            }
         
    }
}