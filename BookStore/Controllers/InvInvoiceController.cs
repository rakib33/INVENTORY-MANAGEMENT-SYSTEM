
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

namespace BookStore.Controllers
{
    [Authorize]
    public class InvInvoiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /InvInvoice/
        public ActionResult Index()
        {
            return View(db.Invoices.ToList());
        }

      
        public ActionResult SaleInvoice(string invoiceId) //string invoiceId
        {
            try
            {
                var inv = TempData["Invoice"]; 

                Invoice Invoice = db.Invoices.Include(t => t.Sales).AsNoTracking().Where(t => t.Id == invoiceId).SingleOrDefault();

                foreach (var item in Invoice.Sales)
                {
                    item.Product = db.Products.Where(t => t.Id == item.Product_Id).SingleOrDefault();                
                }
                return View(Invoice);                      
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
                          
            }

            return RedirectToAction("Index","InvSale");
        }

        [HttpPost]
        public ActionResult SaleInvoice(Invoice model)
        {
            Invoice EditInv = new Invoice();
            Transaction NewTransaction = new Transaction();
            try
            {

                EditInv = db.Invoices.Include(t=>t.Sales).Include(t=>t.Transactions).Where(t => t.Id == model.Id).SingleOrDefault();

                EditInv.Paid = model.Paid;
                EditInv.Due = model.Payable - model.Paid;


                EditInv.Status = EditInv.Payable == model.Paid ? ConstantMessage.StatusPaid : ConstantMessage.StatusAccept; //that mean some Due remain

                EditInv.UpdateDate = DateTime.Now;
                EditInv.UpdateBy = User.Identity.Name;

                db.Entry(EditInv).State = EntityState.Modified;


                NewTransaction.Id = Guid.NewGuid().ToString();
                NewTransaction.Invoice = EditInv;
                NewTransaction.InvoiceNo = EditInv.InvoiceNo;
                NewTransaction.TransactionType = ConstantMessage.InvoiceTypeSale;
                NewTransaction.TransactionNo = CustomCode.GeneratenewRandom(ConstantMessage.TransactionCode);
                NewTransaction.Phone = EditInv.Phone;
                NewTransaction.Customer_Id = EditInv.Customer_Id;
                NewTransaction.ReceiveFrom = EditInv.CustomerName;

                NewTransaction.PaidAmount = model.Paid;
                NewTransaction.Remarks = "";

                NewTransaction.CreatedDate = DateTime.Now;
                NewTransaction.CreatedBy = User.Identity.Name;

                db.Transactions.Add(NewTransaction);

                db.SaveChanges();
                //Notify to all Client
                CustomerHub.BroadcastData();
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">Sale Invoice Save Unsuccessful.Exp :" + ex.Message + "</span>";            
            }
        
        return RedirectToAction("Index","InvSale");
        }



        public ActionResult PurchaseInvoice(string invoiceId)
        {
            try
            {
                Invoice Invoice = db.Invoices.Include(t => t.Purchases).AsNoTracking().Where(t => t.Id == invoiceId).SingleOrDefault();

                foreach (var item in Invoice.Purchases)
                {
                    item.Product = db.Products.Where(t => t.Id == item.Product_Id).SingleOrDefault();
                }

                return View(Invoice);
            }
            catch (Exception ex)
            {
                return RedirectToAction("notFound", "Error", new { message = ex.Message });
            }
        }



        [HttpPost]
        public ActionResult PurchaseInvoice(Invoice model)
        {
            Invoice EditInv = new Invoice();
            Transaction NewTransaction = new Transaction();
            try
            {

                EditInv = db.Invoices.Include(t => t.Purchases).Include(t => t.Transactions).Where(t => t.Id == model.Id).SingleOrDefault();

                EditInv.Paid = model.Paid;
                EditInv.Due = model.Payable - model.Paid;
                EditInv.Status = EditInv.Payable == model.Paid ? ConstantMessage.StatusPaid : ConstantMessage.StatusAccept; //that mean some Due remain

                EditInv.UpdateDate = DateTime.Now;
                EditInv.UpdateBy = User.Identity.Name;

                db.Entry(EditInv).State = EntityState.Modified;


                NewTransaction.Id = Guid.NewGuid().ToString();
                NewTransaction.Invoice = EditInv;
                NewTransaction.InvoiceNo = EditInv.InvoiceNo;
                NewTransaction.TransactionType = ConstantMessage.InvoiceTypePurchase;
                NewTransaction.TransactionNo = CustomCode.GeneratenewRandom(ConstantMessage.TransactionCode);
                NewTransaction.Phone = EditInv.Phone;
                NewTransaction.Customer_Id = EditInv.Customer_Id;
                NewTransaction.ReceiveFrom = EditInv.CustomerName;

                NewTransaction.PaidAmount = model.Paid;
                NewTransaction.Remarks = "";

                NewTransaction.CreatedDate = DateTime.Now;
                NewTransaction.CreatedBy = User.Identity.Name;

                db.Transactions.Add(NewTransaction);

                db.SaveChanges();

                //Notify to all Client
                CustomerHub.BroadcastData();

            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">Purchase Invoice Save Unsuccessful.Exp :" + ex.Message + "</span>";

            }
            return RedirectToAction("Index", "InvPurchase");
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
