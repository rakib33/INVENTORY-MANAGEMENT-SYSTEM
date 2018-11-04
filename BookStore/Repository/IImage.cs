using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Infrastructure;
using System.Web;

namespace BookStore.Repository
{
    interface IImage
    {
        UserImage SetImage(HttpPostedFileBase file);
       
    }
}
