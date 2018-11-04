using BookStore.Hubs;
using BookStore.App_Code;
using BookStore.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//


namespace BookStore.Controllers
{
    
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                ViewBag.Title = "Home Page";
                List<ProductWiseViewModel> GetList = null;
               // var result = InventoryGainLossDashboard(FromDate, ToDate);
                return View();
             
            }
            catch (Exception ex)
            {
                string ms = ex.Message;
                return RedirectToAction("notFound", "Error");
            }
        }


        [HttpGet]
        public ActionResult GetAllData()
        {
            var result = InventoryGainLossDashboard(null, null);
            return PartialView("ProductGainLoss", result);
        }
        public List<ProductWiseViewModel> InventoryGainLossDashboard(DateTime? FromDate,DateTime? ToDate)
        {

          List<ProductWiseViewModel> GetList = null;
          try
          {
              if (ToDate == null)
                  ToDate = DateTime.Today;
              GetList = ProcedureFunctionCalled.GetInventoryGainLossDashBoard(FromDate, ToDate);

          }
          catch
          {

          }

         return GetList;

           
        }

    
    }

}
