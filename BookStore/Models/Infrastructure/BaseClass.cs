using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace BookStore.Models.Infrastructure
{
    public abstract class BaseClass
    {
        [Key]
        [StringLength(128)]
        public string Id { get; set; }

        [Display(Name="Created By")]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Updated By")]
        [StringLength(100)]
        public string UpdateBy { get; set; }

        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }
        
    }
}