using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.Web.Mvc;

namespace BookStore.App_Code
{
    public class MyViewEngine : RazorViewEngine
    {
         //This is for Partial View
       private static string[] NewPartialViewFormats = new[] 
       {
           
           "~/Views/{1}/{0}.cshtml",

           "~/Views/RIPView/{1}/{0}.cshtml",
           "~/Views/Admin/{1}/{0}.cshtml",     

       };

        public MyViewEngine()
        {
           base.ViewLocationFormats = new string[] { 
                            
                "~/Views/RIPView/{1}/{0}.cshtml",
                "~/Views/Admin/{1}/{0}.cshtml",     //no need to add any view map of controller under admin folder
                            
                //For RIPUser
                "~/Views/RIPView/E-BOOK/{0}.cshtml",        
                "~/Views/RIPView/ALQURAN/{0}.cshtml",
                "~/Views/RIPView/DUA/{0}.cshtml",
                "~/Views/RIPView/FINDER(QUBLA)/{0}.cshtml",
                "~/Views/RIPView/HOW TO/{0}.cshtml",
                "~/Views/RIPView/ISLAM-O-SUSASTHO/{0}.cshtml",
                "~/Views/RIPView/MONTH OF RAMDAN/{0}.cshtml",              
                "~/Views/RIPView/NAMAJ/{0}.cshtml",
                "~/Views/RIPView/ZAKAT CALCULATOR/{0}.cshtml",                
                "~/Views/RIPView/HAJJ/{0}.cshtml",

                "~/Views/RIPView/Option/{0}.cshtml",
            };

            
        }
    }
}