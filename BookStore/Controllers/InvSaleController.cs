using BookStore.Hubs; //add this to braodcast changes to all connected client
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
using BookStore.Models.ViewModel;


namespace BookStore.Controllers
{
    [Authorize]
    public class InvSaleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private Variable variable = new Variable(); 

        // GET: /InvSale/
        
        public ActionResult Index(string Option)
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);              
                 var Customer =
                   db.Users.Where(t => t.UserType == ConstantMessage.UserTypeCustomer).Select(t => new UserIdNameModel
                   {
                       Id = t.Id,
                       UserName = t.UserName + "(" + t.Name + ")"
                   }).ToList();

            ViewBag.Customer = new SelectList(Customer, "Id", "UserName");
            ViewBag.Option = Option;
         //   var v = ProcedureFunctionCalled.GetInvoiceInfo("", ConstantMessage.InvoiceTypeSale, "", "", null, null);

            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
        }




        //Write action for return json data
        [HttpPost]
        public ActionResult LoadData(string InvoiceNo, string CustomerId, string FromDate=null, string ToDate=null,string phone=null, string Option=null) //DateTime fromdate,DateTime todate
        {
            //get Start (paging start index) and length (page size for paging)



            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value           
            //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = ProcedureFunctionCalled.GetInvoiceInfo("", ConstantMessage.InvoiceTypeSale, "", "", null, null);


            //if (FromDate.HasValue)
            //    v = v.Where(t => t.InvoiceDate.Value >= FromDate.Value).ToList();

            //if (ToDate.HasValue)
            //    v = v.Where(t => t.InvoiceDate.Value <= ToDate.Value).ToList();


            if (FromDate != null && !string.IsNullOrEmpty(FromDate))
            {
                DateTime? fromdate = Convert.ToDateTime(FromDate);
                DateTime? myDate = DateTime.ParseExact(fromdate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                v = v.Where(t => t.InvoiceDate.Value >= myDate.Value).ToList();
            }

            if (ToDate != null && !string.IsNullOrEmpty(ToDate))
            {
                
                DateTime? todate = Convert.ToDateTime(ToDate);
                DateTime? _ToDate = DateTime.ParseExact(todate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                v = v.Where(t => t.InvoiceDate.Value <= _ToDate.Value).ToList();
            }

            if (CustomerId != null && !string.IsNullOrEmpty(CustomerId))
                v = v.Where(t => t.Customer_Id == CustomerId).ToList();

            if(phone !=null && !string.IsNullOrEmpty(phone))
                v = v.Where(t => t.Phone.Contains(phone)).ToList();

            if (InvoiceNo != null && !string.IsNullOrEmpty(InvoiceNo))
                v = v.Where(t => t.InvoiceNo.Contains(InvoiceNo)).ToList();



            totalRecords = v.Count();

            if (v == null)
                totalRecords = 0;
            else
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList(); //
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
                       
        }
        // GET: /InvSale/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /InvSale/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.InvoiceNo = CustomCode.GeneratenewRandom("INV-S-");

                ViewBag.Customer_Id = new SelectList(db.Users.Where(t => t.UserType == ConstantMessage.UserTypeCustomer).Select(t => new UserIdNameModel
                {
                    Id = t.Id,
                    UserName = t.UserName + "(" + t.Name + ")"

                }), "Id", "UserName");  //

                ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");

                SaleViewModel model = new SaleViewModel();
                model.Sale = new List<BulkProduct> { new  BulkProduct { MainCatagoryId = "", BuyRate = 0, BuyTotal = 0, Product_Id = "", Qty = 0, SaleRate = 0, SaleTotal = 0} };
                //Invoice_Id working here Product Main Catagory and Product_Id is product sub catagory
                ViewBag.Status = CustomCode.SaleStatusList(); 
            

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");

        }

        //
        // POST: /InvSale/Create
        [HttpPost]
        public ActionResult Create(SaleViewModel model)
        {
           
                List<Sale> salableproduct = new List<Sale>();
                variable.Message = null;
                         
                if(ModelState.IsValid)
                {                   
                    try
                    {

                        ApplicationUser Customer = new ApplicationUser();
                        Customer = db.Users.Where(t => t.Id == model.CustomerId).SingleOrDefault();

                        //get total amount
                        variable.TotalAmount = model.Sale.Where(t=>t.Status == ConstantMessage.StatusApproved).Sum(t => t.SaleTotal);
                        //genarate Invoice

                        Invoice invoice = new Invoice();

                        invoice.Id           = Guid.NewGuid().ToString();
                        invoice.InvoiceNo    = model.Invoice;
                        invoice.InvoiceType  = ConstantMessage.InvoiceTypeSale;

                        invoice.Customer_Id  = model.CustomerId;
                        invoice.CustomerName = model.CustomerName;
                        invoice.Phone        = model.CustomerPhone;
                        invoice.Address      = Customer.PresentAddress;

                        invoice.Total        = variable.TotalAmount;
                        invoice.Discount     = 0;
                        invoice.Payable      = variable.TotalAmount - invoice.Discount;
                        invoice.Paid         = 0;
                        invoice.Due          = invoice.Payable - invoice.Paid;

                        invoice.Status       = ConstantMessage.StatusPending;

                        invoice.InvoiceDate  = model.InvoiceDate;
                        invoice.CreatedDate  = DateTime.Now;
                        invoice.CreatedBy    = User.Identity.Name;

                        db.Invoices.Add(invoice);

                        variable.Count = 0;

                        foreach (var item in model.Sale)
                        {
                            variable.Count++;

                            if (item.Status != ConstantMessage.StatusReject && item.Qty > 0)
                            {
                                variable.Count--; 
                                var product  = new Product();
                                product      = db.Products.Where(t => t.Id == item.Product_Id).SingleOrDefault();

                                //var ProductInStock = new Stock();
                                //ProductInStock     = db.Stocks.Where(t => t.Product_Id == item.Product_Id).SingleOrDefault();

                                Sale newSale = new Sale();

                                newSale.Id = Guid.NewGuid().ToString();

                                newSale.MainCatagory_Id = item.MainCatagoryId;
                                newSale.SubCatagory_Id  = item.SubCatagoryId;

                                newSale.BuyRate     = item.BuyRate;
                                newSale.BuyTotal    = item.BuyTotal;
                                newSale.InvoiceDate = model.InvoiceDate;

                                newSale.Stock       = item.Stock; // how many remaining now

                                newSale.Qty         = item.Qty;
                                newSale.SaleRate    = item.SaleRate;
                                newSale.SaleTotal   = item.SaleTotal;
                                newSale.Status      = item.Status;
                               
                                newSale.CreatedDate = DateTime.Now;
                                newSale.CreatedBy   = User.Identity.Name;

                                newSale.Product         = product;
                                newSale.ApplicationUser = Customer;
                                
                                newSale.Invoice     = invoice;

                                db.Sales.Add(newSale);
                                
                            }
                        }

                        if (model.Sale.Count() != variable.Count)
                        {
                            db.SaveChanges();

                            //Notify to all Client
                            CustomerHub.BroadcastData();

                             TempData["Invoice"] = invoice;
                             // go to Invoice page 
                             return RedirectToAction("SaleInvoice", "InvInvoice", new { invoiceId = invoice.Id });
                        }
                        else
                        {
                            TempData["message"] = "<span class=\"font-red\">Save Unsuccessful.Either All Product in Stock not found nighter nor Status is rejected!!</span>";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        //transaction.Rollback();
                        ModelState.AddModelError("", "Transaction failed due to- " + ex.Message);
                    }


                }// if model validation failed

                #region ReturnIfSaveFailed
                ViewBag.InvoiceNo = CustomCode.GeneratenewRandom("INV-S-");
                ViewBag.Customer_Id = new SelectList(db.Users.Where(t => t.UserType == ConstantMessage.UserTypeCustomer).Select(t => new UserIdNameModel
                {
                    Id = t.Id,
                    UserName = t.UserName + "(" + t.Name + ")"

                }), "Id", "UserName");  //

                ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");
                model.Sale.Clear();
                model.Sale = new List<BulkProduct> { new BulkProduct { MainCatagoryId = "", BuyRate = 0, BuyTotal = 0, Product_Id = "", Qty = 0, SaleRate = 0, SaleTotal = 0 } };
              
              //  model.Sale = new List<Sale> { new Sale { Id = "", Invoice_Id = "", BuyRate = 0, BuyTotal = 0, Product_Id = "", Qty = 0, SaleRate = 0, SaleTotal = 0 } };               
                //Invoice_Id working here Product Main Catagory and Product_Id is product sub catagory
                ViewBag.Status = CustomCode.SaleStatusList();
                #endregion

                return View(model);
                       
        }

        //
        // GET: /InvSale/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /InvSale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /InvSale/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /InvSale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult SaleReturn(string invoiceId)
        {
            try
            {

                Invoice invObj = new Invoice();
                invObj = db.Invoices.Include(t => t.Sales).Include(t => t.Transactions).Where(t => t.Id == invoiceId).SingleOrDefault();

                SaleViewModel model = new SaleViewModel();
                model.CustomerId = invObj.Customer_Id;
                model.CustomerName = invObj.CustomerName;
                model.CustomerPhone = invObj.Phone;
                model.InvoiceDate = invObj.InvoiceDate;
                model.Invoice = invObj.InvoiceNo;
                model.CustomerId = invObj.Customer_Id;

                ViewBag.Customer_Id = new SelectList(db.Users.Where(t => t.UserType == ConstantMessage.UserTypeCustomer).Select(t => new UserIdNameModel
                {
                    Id = t.Id,
                    UserName = t.UserName + "(" + t.Name + ")"

                }), "Id", "UserName");  //

                foreach (var item in invObj.Sales)
                {

                    
                    model.Sale.Add(new BulkProduct {
                       

                        SubCatagoryId = item.SubCatagory_Id,
                        MainCatagoryId = item.MainCatagory_Id,
                        Stock = item.Stock,
                        BuyRate = 0,
                        BuyTotal = 0,
                        Product_Id = item.Product_Id,
                        Qty = item.Qty,
                        SaleRate = item.SaleRate,
                        SaleTotal = item.SaleTotal,
                        Discount = 0,
                        Status = item.Status

                      

                       }
                    );
                }

                ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList());
                ViewBag.Status = CustomCode.SaleStatusList(); 
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
        }
    }
}
