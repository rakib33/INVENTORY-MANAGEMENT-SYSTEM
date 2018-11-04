using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Models.Inventory;
using BookStore.Models.Infrastructure;
using BookStore.Models;
using BookStore.App_Code;

namespace BookStore.Controllers
{
   [Authorize]
    public class DataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

     
       [HttpPost]
        public JsonResult getSubCatagory(string id)
        {

            List<Catagory> MainCatagory = db.Catagories.Where(t => t.Parent_Id == id).ToList();

            List<SelectListItem> lists = new List<SelectListItem>();
            lists.Add(new SelectListItem { Text = "--Select Sub Catagory--", Value = null });

            foreach (var row in MainCatagory)
            {

                lists.Add(new SelectListItem { Text = row.Name, Value = row.Id });

            }          
            return Json(new SelectList(lists, "Value", "Text", JsonRequestBehavior.AllowGet));  
        }


        public JsonResult getProduct(string Catid)
        {

            List<Product> productList = db.Products.Where(t => t.Catagory_Id == Catid).ToList();

            List<SelectListItem> lists = new List<SelectListItem>();
            lists.Add(new SelectListItem { Text = "--Select Product--", Value = null });

            foreach (var row in productList)
            {

                lists.Add(new SelectListItem { Text = row.Name, Value = row.Id });

            }
            return Json(new SelectList(lists, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
      
        public JsonResult getBrand(string id)
        {

            List<Brand> brand = db.Brands.Where(t => t.Catagory_Id == id).ToList();

            List<SelectListItem> lists = new List<SelectListItem>();
            lists.Add(new SelectListItem { Text = "--Select Brand--", Value = null });

            foreach (var row in brand)
            {

                lists.Add(new SelectListItem { Text = row.Name, Value = row.Id });

            }
            return Json(new SelectList(lists, "Value", "Text", JsonRequestBehavior.AllowGet));

        }

        public ActionResult getRate(string productId)
        {
            Product List = new Product();
            string message;
            try
            {
              List = db.Products.Where(t => t.Id == productId).SingleOrDefault();
              message = "true";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new {message,List }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSaleRate(string productId,decimal SaleQty)
        {
            StockViewModel List = new StockViewModel();
        
            string message;
            try
            {
                if (SaleQty > 0)
                {
                  List=  ProcedureFunctionCalled.GetProductInStock(productId,DateTime.Today).SingleOrDefault();

                  if (SaleQty > List.ProductInStock)
                      List = null;
                    
                }
                else
                    List = null;
                message = "true";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message, List }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ProductInStock(string productId)
        {
            decimal Stock = 0;
            string message="false";
            try
            {
                if (productId !=null && !string.IsNullOrEmpty(productId))
                {
                    Stock = ProcedureFunctionCalled.GetProductInStock(productId,DateTime.Today).SingleOrDefault().ProductInStock;                       
                    //db.Stocks.Where(t => t.Product_Id == productId).SingleOrDefault().ProductInStock;                        
                    message = "true";
                }
                
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message, Stock }, JsonRequestBehavior.AllowGet);


        }

        //Called from SaleEdit.Js GetCustomerInfo(e)
        //Called from SaleEdit.Js GetEmployeeSalary
        public ActionResult GetCustomerInfo(string userId)
        {


            ApplicationUser user = new ApplicationUser();
            string message;
            try
            {
                user = db.Users.Find(userId);
                message = "true";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message, user }, JsonRequestBehavior.AllowGet);


        
        
        }
    }
}
