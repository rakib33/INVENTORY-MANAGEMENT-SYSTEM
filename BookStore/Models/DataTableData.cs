using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.App_Code;
using BookStore.Models;
using BookStore.Models.Infrastructure;

namespace BookStore.Models
{
    public class DataTableData<T> 
    {
    public int draw { get; set; }
    public int recordsTotal { get; set; }
    public int recordsFiltered { get; set; }
    public List<T> data { get; set; }
    }
}