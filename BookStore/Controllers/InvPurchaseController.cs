using BookStore.Hubs;
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
using System.Globalization;
namespace BookStore.Controllers
{
    [Authorize]
    public class InvPurchaseController : Controller
    {
        //IF CAST(DateField1 AS DATE) = CAST(DateField2 AS DATE)


        private ApplicationDbContext db = new ApplicationDbContext();
        private Variable variable = new Variable(); 
        // GET: /InvPurchase/
        public ActionResult Index(string Option)
        {
            //http://demo.dotnetawesome.com/jquery-datatable-server-side-pagination-sorting
            //http://www.c-sharpcorner.com/article/Asp-Net-mvc5-datatables-plugin-server-side-integration/
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                ViewBag.Status = CustomCode.StatusList();

                var Supplier =
                    db.Users.Where(t => t.UserType == ConstantMessage.UserTypeSupplier).Select(t => new UserIdNameModel
                    {
                        Id = t.Id,
                        UserName = t.UserName + "(" + t.Name + ")"
                    }).ToList();

                ViewBag.Supplier = new SelectList(Supplier, "Id", "UserName");
                ViewBag.Option = Option;

                return View();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return RedirectToAction("notFound", "Error", new { message = ex.Message });
            }
         
        }

      
        /// <param name="id">id= Invoice Id</param>
        /// <returns></returns>
        public ActionResult Purchase(string id)
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                ViewBag.Status = CustomCode.StatusList();

                var Supplier =
                    db.Users.Where(t => t.UserType == ConstantMessage.UserTypeSupplier).Select(t => new UserIdNameModel
                    {
                        Id = t.Id,
                        UserName = t.UserName + "(" + t.Name + ")"
                    }).ToList();

                ViewBag.Supplier = new SelectList(Supplier, "Id", "UserName");

                var InvoiceNo = db.Invoices.Find(id).InvoiceNo;
                ViewBag.InvoiceNo = InvoiceNo;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return RedirectToAction("notFound", "Error", new { message = ex.Message });
            }
            return View();
        }

        //Write action for return json data
        [HttpPost]
        public ActionResult LoadData(string InvoiceNo,string FromDate, string ToDate,string phone, string SuplierId,string Option) 
       {         
         
           // DateTime _FromDate;
           // DateTime _ToDate; 

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            
            //Get Sort columns value           
            //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

           

            if (Option == "PurchaseInvoice")
            {
                var p = ProcedureFunctionCalled.GetInvoiceInfo("", ConstantMessage.InvoiceTypePurchase, "", "", null, null);
                //Filter
                if (p == null)
                    totalRecords = 0;
                else
                {
                    //fileter by date and supplier
                    //if (FromDate.HasValue)
                    //    p = p.Where(t => t.InvoiceDate.Value >= FromDate.Value).ToList();

                    //if (ToDate.HasValue)
                    //    p = p.Where(t => t.InvoiceDate.Value <= ToDate.Value).ToList();


                    if (FromDate != null && !string.IsNullOrEmpty(FromDate))
                    {
                        DateTime? fromdate = Convert.ToDateTime(FromDate);
                        DateTime? myDate = DateTime.ParseExact(fromdate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                        p = p.Where(t => t.InvoiceDate.Value.Date >= myDate.Value.Date).ToList();
                    }

                    if (ToDate != null && !string.IsNullOrEmpty(ToDate))
                    {
                        DateTime? todate = Convert.ToDateTime(ToDate);
                        DateTime? _ToDate = DateTime.ParseExact(todate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                        p = p.Where(t => t.InvoiceDate.Value.Date <= _ToDate.Value.Date).ToList();
                    }

                    if(SuplierId !=null && !string.IsNullOrEmpty(SuplierId))
                        p = p.Where(t => t.Customer_Id == SuplierId).ToList();

                    if (phone != null && !string.IsNullOrEmpty(phone))
                        p = p.Where(t => t.Phone.Contains(phone)).ToList();

                    if (InvoiceNo != null && !string.IsNullOrEmpty(InvoiceNo))
                        p = p.Where(t => t.InvoiceNo.Contains(InvoiceNo)).ToList();
                    totalRecords = p.Count();
                }
                var data = p.Skip(skip).Take(pageSize).ToList(); //
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            }
           else
            {
                var v = ProcedureFunctionCalled.GetPurchaseDisplayInfo("", "", null, null);
                //filter
                if (v == null)
                    totalRecords = 0;
                else
                {
                    //fileter by date and supplier
                    
                    totalRecords = v.Count();
                }
                var data = v.Skip(skip).Take(pageSize).ToList(); //
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            }
           
            
        }



        public ActionResult PurchseList()
        {           
            ViewBag.Status = CustomCode.StatusList();

            var Supplier =
                db.Users.Where(t => t.UserType == ConstantMessage.UserTypeSupplier).Select(t => new UserIdNameModel
                {
                    Id = t.Id,
                    UserName = t.UserName + "(" + t.Name + ")"
                }).ToList();

            ViewBag.Supplier = new SelectList(Supplier, "Id", "UserName");
            return View();
        }
        // GET: /InvPurchase/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Include(t => t.ApplicationUser).Include(t => t.Product).Where(t => t.Id == id).SingleOrDefault();
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: /InvPurchase/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.InvoiceNo = CustomCode.GeneratenewRandom(ConstantMessage.InvoicePurchaseCode);
                ViewBag.Supplier_Id = new SelectList(db.Users.Where(t => t.UserType == ConstantMessage.UserTypeSupplier).Select(t => new UserIdNameModel { 
                Id=t.Id,
                UserName = t.UserName +"("+t.Name+")"
                
                }), "Id", "UserName");  //

                ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");
                PurchaseViewModel model = new PurchaseViewModel();
                model.Purchase = new List<BulkProduct> { new BulkProduct { MainCatagoryId = "", SubCatagoryId = "", BuyRate = 0, BuyTotal = 0, Product_Id = "", Qty = 0, SaleRate = 0, SaleTotal = 0, Status = "" } };
                //Invoice_Id working here Product Main Catagory and Product_Id is product sub catagory

                ViewBag.Status = CustomCode.StatusList();   //items;

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");

          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseViewModel model)
        {
            if (model.Invoice !=null && !string.IsNullOrEmpty(model.Invoice) && model.Purchase.Count>0 && model.InvoiceDate !=null &&model.SupplierId!=null && !string.IsNullOrEmpty(model.SupplierId))
            {
                variable.Result = ConstantMessage.StatusPending;
              
                     try
                        {

                          ApplicationUser Supplier = new ApplicationUser();
                          Supplier = db.Users.Where(t => t.Id == model.SupplierId).SingleOrDefault();

                         //get TotalAmount from 

                          variable.TotalAmount = model.Purchase.Where(t=>t.Status == ConstantMessage.StatusApproved).Sum(t=>t.BuyTotal);

                         //genarate Invoice
                          Invoice invoice = new Invoice();

                          invoice.Id          = Guid.NewGuid().ToString();
                          invoice.InvoiceNo   = model.Invoice;
                          invoice.InvoiceType = ConstantMessage.InvoiceTypePurchase;

                          invoice.Customer_Id   = Supplier.Id;
                          invoice.CustomerName  = Supplier.UserName + "(" + Supplier.Name + ")";
                          invoice.Phone         = Supplier.Phone;
                          invoice.Address       = Supplier.PresentAddress;

                          invoice.Total         = variable.TotalAmount;
                          invoice.Discount      = 0;
                          invoice.Payable       = variable.TotalAmount - invoice.Discount;
                          invoice.Paid          = 0;
                          invoice.Due           = invoice.Payable - invoice.Paid;
                         
                          invoice.Status        = ConstantMessage.StatusPending;

                          invoice.InvoiceDate   = model.InvoiceDate;
                          invoice.CreatedDate   = DateTime.Now;
                          invoice.CreatedBy     = User.Identity.Name;

                          db.Invoices.Add(invoice);

                          variable.Count = 0;
                         
                          foreach (var item in model.Purchase)
                            {
                                variable.Count++;
                                if (item.Status != ConstantMessage.StatusReject && item.Qty > 0)
                                {

                                    variable.Count--; 

                                    var product  = new Product();
                                    product      = db.Products.Where(t => t.Id == item.Product_Id).SingleOrDefault();

                                    var ProductInStock = new Stock();
                                    ProductInStock     = db.Stocks.Where(t => t.Product_Id == item.Product_Id).SingleOrDefault();

                                    Purchase newPurchase  = new Purchase();
                                    newPurchase.Id        = Guid.NewGuid().ToString();


                                    newPurchase.InvoiceNo  = model.Invoice;
                                    newPurchase.BuyRate    = item.BuyRate;
                                    newPurchase.BuyTotal   = item.BuyTotal;

                                    newPurchase.FK_Supplier_Id = Supplier.Id;
                                    newPurchase.Product_Id     = item.Product_Id;

                                    newPurchase.Qty          = item.Qty;
                                    newPurchase.SaleRate     = item.SaleRate;
                                    newPurchase.SaleTotal    = item.SaleTotal;
                                    newPurchase.Status       = item.Status;
                                    newPurchase.InvoiceDate  = model.InvoiceDate;
                                    newPurchase.CreatedDate  = DateTime.Now;
                                    newPurchase.CreatedBy    = User.Identity.Name;

                                    newPurchase.Invoice      = invoice;

                                    //if update by is null then updateDate will be created date
                                    db.Purchases.Add(newPurchase);
                                  

                                   // variable.TotalAmount += newPurchase.BuyTotal;

                                    #region PreviousCode
                                    ////Check Status if Approved add the Qty in Stock table
                                    ////Only availale Quantity from Stock table is elegibale for Sale
                                    ////first find out the product in Stock if found then update Qty else Add new One
                                    //if (item.Status == ConstantMessage.StatusApproved)
                                    //{

                                    //    if (ProductInStock == null)
                                    //    {
                                    //        //add new product in stock
                                    //        Stock addProductInStock = new Stock();

                                    //        addProductInStock.Id = Guid.NewGuid().ToString();
                                    //        addProductInStock.ProductInStock = item.Qty;
                                    //        addProductInStock.CreatedDate = DateTime.Now;
                                    //        addProductInStock.CreatedBy = User.Identity.Name;
                                    //        addProductInStock.Product_Id = item.Product_Id;

                                    //        db.Stocks.Add(addProductInStock);


                                    //    }
                                    //    else
                                    //    { //edit existing product Qty 


                                    //        ProductInStock.ProductInStock = ProductInStock.ProductInStock + item.Qty;

                                    //        ProductInStock.UpdateDate = DateTime.Now;
                                    //        ProductInStock.UpdateBy = User.Identity.Name;

                                    //        db.Entry(ProductInStock).State = EntityState.Modified;


                                    //    }
                                    //}//
                                    #endregion
                                }
                              
                            }

                          if (model.Purchase.Count() != variable.Count)
                          {
                              db.SaveChanges();
                              
                              //Broadcast to all client
                              CustomerHub.BroadcastData();

                              TempData["Invoice"] = invoice;
                              return RedirectToAction("PurchaseInvoice", "InvInvoice", new { invoiceId = invoice.Id });
                          }
                          else
                              TempData["message"] = "<span class=\"font-red\">Records Save Un Successful.Because all product status is rejected!!</span>";                  
                          
                           //TempData["message"] = "<span class=\"font-green\">Records Save Successful.</span>";
                           return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            //transaction.Rollback();
                            ModelState.AddModelError("", "Transaction failed due to- " + ex.Message);
                        }
                       
                
            }// if model validation failed
           

           // model.Purchase.Clear();
           // model.Purchase = new List<Purchase> { new Purchase { Id = "", Invoice_Id = "", BuyRate = 0, BuyTotal = 0, Product_Id = "", Qty = 0, SaleRate = 0, SaleTotal = 0, Status = "" } };
           
            ViewBag.InvoiceNo = CustomCode.GeneratenewRandom(ConstantMessage.InvoicePurchaseCode);

            ViewBag.Supplier_Id = new SelectList(db.Users.Where(t => t.UserType == ConstantMessage.UserTypeSupplier).Select(t => new UserIdNameModel { 
             Id = t.Id,
             UserName = t.UserName +"("+t.Name+")"
            }), "Id", "UserName");
            
            ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");
            ViewBag.Status = CustomCode.StatusList();

            return View(model);
        }

        // GET: /InvPurchase/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {

                Purchase model = db.Purchases.Include(t=>t.ApplicationUser).Where(t=>t.Id == id).SingleOrDefault();   //.Find(id);
                if (model == null)
                {
                    TempData["message"] = "<span class=\"font-red\">Records not Found.</span>";
                }
                else
                {

                    //Check Status if Approved don't allow Edit permission

                    if (model.Status == ConstantMessage.StatusApproved)
                    {
                        ViewBag.message = "<span class=\"font-red\">This product is already Approved.</span>";
                        ViewBag.Option = "Approved";
                    }
                   

                        var Product = db.Products.Where(t => t.Id == model.Product_Id).SingleOrDefault();
                        //take this product Catagory_Id

                        ViewBag.Product = new SelectList(db.Products.Where(t => t.Catagory_Id == Product.Catagory_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", Product.Id);
                        //This Catagory_Id from Catagory Table will be the SubCatagory in Dropdown

                        var SubCatagory = db.Catagories.Where(t => t.Id == Product.Catagory_Id).SingleOrDefault();
                        //one row come contain its ParentId.Get all Catagory whose ParentId = SubCatagory.ParentId

                        ViewBag.SubCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == SubCatagory.Parent_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Id);
                        ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == null).OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Parent_Id);


                        ViewBag.Status = CustomCode.StatusList();

                        ViewBag.Supplier = new SelectList(db.Users.Where(t => t.UserType == ConstantMessage.UserTypeSupplier).Select(t => new UserIdNameModel { 
                         Id=t.Id,
                         UserName = t.UserName+"("+t.Name+")"
                        }), "Id", "UserName", model.FK_Supplier_Id);


                        return View(model);
                    
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
           
        }

        // POST: /InvPurchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Purchase purchase,FormCollection Collect)
        {              
            try
            {
                Purchase model = db.Purchases.Include(t => t.ApplicationUser).Include(t=>t.Invoice).Where(t => t.Id == purchase.Id).SingleOrDefault();
                Invoice invoice = new Invoice();
                //Purchase model = db.Purchases.Find(purchase.Id);
                if (model == null)
                {
                    TempData["message"] = "<span class=\"font-red\">Records not Found.</span>";
                }
                else
                {                   
                    
                             try
                                {

                                model.Status = purchase.Status;                               

                                model.UpdateDate = DateTime.Now;
                                model.UpdateBy = User.Identity.Name;

                                //Check Status if Approved add the Qty in Stock table
                                //Only availale Quantity from Stock table is elegibale for Sale
                                //first find out the product in Stock if found then update Qty else Add new One

                                var ProductInStock = new Stock();
                                ProductInStock = db.Stocks.Where(t => t.Product_Id == purchase.Product_Id).SingleOrDefault();

                                if (model.Status == ConstantMessage.StatusApproved)
                                {
                                    //parent child update test purpose
                                    model.Invoice.Total += model.BuyTotal;

                                    model.Invoice.Discount += 0;
                                    model.Invoice.Payable = model.Invoice.Total - model.Invoice.Discount;
                                    model.Invoice.Paid = 0;
                                    model.Invoice.Due = model.Invoice.Payable - model.Invoice.Paid;

                                    ///////

                                    if (ProductInStock == null)
                                    {
                                        //add new product in stock
                                        Stock addProductInStock = new Stock();

                                        addProductInStock.Id = Guid.NewGuid().ToString();
                                        addProductInStock.ProductInStock = model.Qty;            //purchase.Qty;
                                        addProductInStock.CreatedDate = DateTime.Now;
                                        addProductInStock.CreatedBy = User.Identity.Name;
                                        addProductInStock.Product_Id = purchase.Product_Id;

                                        db.Stocks.Add(addProductInStock);


                                    }
                                    else
                                    { //edit existing product Qty 


                                        ProductInStock.ProductInStock = ProductInStock.ProductInStock + model.Qty;  // purchase.Qty;

                                        ProductInStock.UpdateDate = DateTime.Now;
                                        ProductInStock.UpdateBy = User.Identity.Name;

                                        db.Entry(ProductInStock).State = EntityState.Modified;


                                    }
                                }//
                                //now update purchase
                                if (model.Status == ConstantMessage.StatusReject)
                                {
                                    db.Entry(model).State = EntityState.Deleted;
                                }
                                else
                                {
                                    db.Entry(model).State = EntityState.Modified;
                                }


                                db.SaveChanges();

                                //Broadcast to all client
                                CustomerHub.BroadcastData();
                               // transaction.Commit();

                                return RedirectToAction("Index");
                            }
                             catch (Exception ex)
                             {
                                 //transaction.Rollback();
                                 ModelState.AddModelError("", "Transaction failed due to- " + ex.Message);
                             }
                    //    }
                    //}
                    
                    var Product = db.Products.Where(t => t.Id == model.Product_Id).SingleOrDefault();
                    //take this product Catagory_Id

                    ViewBag.Product = new SelectList(db.Products.Where(t => t.Catagory_Id == Product.Catagory_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", Product.Id);
                    //This Catagory_Id from Catagory Table will be the SubCatagory in Dropdown

                    var SubCatagory = db.Catagories.Where(t => t.Id == Product.Catagory_Id).SingleOrDefault();
                    //one row come contain its ParentId.Get all Catagory whose ParentId = SubCatagory.ParentId

                    ViewBag.SubCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == SubCatagory.Parent_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Id);
                    ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Parent_Id);


                    ViewBag.Status = CustomCode.StatusList();
                    ViewBag.Supplier = new SelectList(db.Users.Where(t => t.UserType ==ConstantMessage.UserTypeSupplier).Select(t => new UserIdNameModel
                    {
                        Id = t.Id,
                        UserName = t.UserName + "(" + t.Name + ")"
                    }), "Id", "UserName");

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
           
        }

        // GET: /InvPurchase/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: /InvPurchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Purchase purchase = db.Purchases.Find(id);
            db.Purchases.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
