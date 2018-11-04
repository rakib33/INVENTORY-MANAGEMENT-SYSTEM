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
using System.Globalization;

namespace BookStore.Controllers
{
    [Authorize]
    public class InvTransactionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public UserRoleManager UserObj = new UserRoleManager();
        // GET: /InvTransactionn/
        public ActionResult Index()
        {
            try
            {
                //var transactions = db.Transactions.Include(t => t.Invoice);
                //return View(transactions.ToList());
                ViewBag.TransactionType = CustomCode.TransactionTypeList();
                return View();
            }

            catch (Exception ex)
            {
                return RedirectToAction("notFound", "Error", new { message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult LoadData(string InvoiceNo, string InvoiceType, string FromDate, string ToDate) //DateTime fromdate,DateTime todate
        {
            //get Start (paging start index) and length (page size for paging)


            DateTime _FromDate;
            DateTime _ToDate;

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            if (FromDate != null && !string.IsNullOrEmpty(FromDate))
                _FromDate = Convert.ToDateTime(FromDate);
            else
                _FromDate = DateTime.Today.AddDays(-30);

            if (ToDate != null && !string.IsNullOrEmpty(ToDate))
                _ToDate = Convert.ToDateTime(ToDate);
            else
                _ToDate = DateTime.Today;

            var v = ProcedureFunctionCalled.GetTransactionInfo(InvoiceType, _FromDate, _ToDate);

            if (InvoiceNo != null && !string.IsNullOrEmpty(InvoiceNo))
                v = v.Where(t => t.InvoiceNo == InvoiceNo).ToList();

            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList(); //
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);


        }

        //this is Invoice Id
        public ActionResult DuePaid() //string Id
        {
            var InvoiceId = Convert.ToString(TempData["InvoiceId"]);
            var InvoiceType = Convert.ToString(TempData["InvoiceType"]);
            try
            {
                Invoice GetInvoice = new Invoice();

                if (InvoiceType == ConstantMessage.InvoiceTypePurchase)
                {
                    GetInvoice = db.Invoices.Include(t => t.Purchases).Where(t => t.Id == InvoiceId).SingleOrDefault();
                }
                else if (InvoiceType == ConstantMessage.InvoiceTypeSale)
                {
                    GetInvoice = db.Invoices.Include(t => t.Sales).Where(t => t.Id == InvoiceId).SingleOrDefault();
                }
                else
                {
                    GetInvoice = db.Invoices.Where(t => t.Id == InvoiceId).SingleOrDefault();
                }

                return View(GetInvoice);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            if (InvoiceType == ConstantMessage.InvoiceTypePurchase)
            {
                return RedirectToAction("Index", "InvPurchase");
            }
            else if (InvoiceType == ConstantMessage.InvoiceTypeSale)
            {
                return RedirectToAction("Index", "InvSale");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Purchase and Sale
        /// </summary>
        [HttpPost]
        public ActionResult Transaction(Invoice model, string PaidAmount)
        {
            Invoice invoice = new Invoice();
            try
            {
                var InvoiceId = Convert.ToString(TempData["InvoiceId"]);
                var InvoiceType = Convert.ToString(TempData["InvoiceType"]);

                invoice = db.Invoices.Where(t => t.Id == model.Id).SingleOrDefault();
                Transaction NewTransaction = new Transaction();

                var DuePaid = Convert.ToDecimal(PaidAmount);

                if (invoice.Due > 0)
                    invoice.Due = invoice.Due - DuePaid;

                invoice.Paid += DuePaid;

                if (invoice.Paid == invoice.Payable)
                    invoice.Status = ConstantMessage.CONST_StatusPaid;

                invoice.UpdateBy = User.Identity.Name;
                invoice.UpdateDate = DateTime.Now;

                NewTransaction.Id = Guid.NewGuid().ToString();
                NewTransaction.Invoice = invoice;
                NewTransaction.InvoiceNo = invoice.InvoiceNo;
                NewTransaction.TransactionType = invoice.InvoiceType;
                NewTransaction.TransactionNo = CustomCode.GeneratenewRandom(ConstantMessage.TransactionCode);
                NewTransaction.Phone = invoice.Phone;
                NewTransaction.Customer_Id = invoice.Customer_Id;
                NewTransaction.ReceiveFrom = invoice.CustomerName;

                NewTransaction.PaidAmount = DuePaid;  // model.Paid;
                NewTransaction.Remarks = "";

                NewTransaction.CreatedDate = DateTime.Now;
                NewTransaction.CreatedBy = User.Identity.Name;

                db.Transactions.Add(NewTransaction);

                db.Entry(invoice).State = EntityState.Modified;

                db.SaveChanges();

                TempData["message"] = "<span class=\"font-green\">Transaction Success</span>";
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">Sale Invoice Save Unsuccessful.Exp :" + ex.Message + "</span>";
            }

            if (invoice.InvoiceType == ConstantMessage.InvoiceTypePurchase)
                return RedirectToAction("Index", "InvPurchase");
            if (invoice.InvoiceType == ConstantMessage.InvoiceTypeSale)
                return RedirectToAction("Index", "InvSale");
            else
                return RedirectToAction("NotFound");

        }

        public ActionResult AddTransaction(string Option)
        {
            if (Option == null || string.IsNullOrEmpty(Option))
                Option = Convert.ToString(TempData["Option"]);
            ViewBag.Option = Option;

            var Employee =
                 db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
                 {
                     Id = t.Id,
                     UserName = t.UserName + "(" + t.Name + ")"
                 }).ToList();

            ViewBag.EmployeeList = new SelectList(Employee, "Id", "UserName");

            ViewBag.TranType = CustomCode.TransactionType();
            return View();
        }


        [HttpPost]
        public ActionResult Transaction1(Invoice model)
        {

            if (model.Payable > 0 && model.Paid > 0 && model.InvoiceType != null && model.InvoiceDate != null && model.Customer_Id != null)
            {
                Transaction newTransaction = new Transaction();

                newTransaction.Id = Guid.NewGuid().ToString();

                newTransaction.TransactionType = ConstantMessage.InvoiceTypeSalary;
                newTransaction.TransactionNo = CustomCode.GeneratenewRandom(ConstantMessage.TransactionSalaryCode);
                newTransaction.Customer_Id = model.Customer_Id;

                newTransaction.PaidAmount = model.Paid;

                newTransaction.CreatedDate = model.InvoiceDate;
                newTransaction.CreatedBy = User.Identity.Name;

                db.Transactions.Add(newTransaction);
                db.SaveChanges();

            }
            var Employee =
                 db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
                 {
                     Id = t.Id,
                     UserName = t.UserName + "(" + t.Name + ")"
                 }).ToList();

            ViewBag.EmployeeList = new SelectList(Employee, "Id", "UserName");

            ViewBag.TranType = CustomCode.TransactionType();
            return View();
        }

        #region EmployeeSalaryExtraExpenseAction


        public ActionResult SalaryList()
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                var Employee =
                  db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
                  {
                      Id = t.Id,
                      UserName = t.UserName + "(" + t.Name + ")"
                  }).ToList();

                ViewBag.Employee = new SelectList(Employee, "Id", "UserName");

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }


        }


        public ActionResult AddSalary()
        {

            try
            {
                var Employee =
                     db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
                     {
                         Id = t.Id,
                         UserName = t.UserName + "(" + t.Name + ")"
                     }).ToList();

                ViewBag.EmployeeList = new SelectList(Employee, "Id", "UserName");

                ViewBag.Option = "Salary";
                return View();
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("SalaryList");
            }

        }


        [HttpPost]
        public ActionResult AddSalary(Invoice model)
        {
            try
            {

                model.Paid = model.Payable; //added 08-12-17

                if (model.Payable > 0 && model.Paid > 0 && model.InvoiceType != null && model.InvoiceDate != null && model.Customer_Id != null)
                {


                    var Employee = db.Users.Find(model.Customer_Id);

                    //First Check Is This Month Salary Already Paid of this Employee
                    #region CheckSalary

                    List<Invoice> HasSalary = new List<Invoice>();

                    HasSalary = db.Invoices.Where(t => t.InvoiceType == ConstantMessage.InvoiceTypeSalary && t.Customer_Id == model.Customer_Id && t.InvoiceDate.Value.Month == model.InvoiceDate.Value.Month && t.InvoiceDate.Value.Year == model.InvoiceDate.Value.Year).ToList();



                    #endregion


                    if (HasSalary == null || HasSalary.Count == 0)
                    {
                        Invoice invoice = new Invoice();

                        invoice.Id = Guid.NewGuid().ToString();
                        invoice.InvoiceNo = CustomCode.GeneratenewRandom(ConstantMessage.InvoiceSalaryCode);
                        invoice.InvoiceType = ConstantMessage.InvoiceTypeSalary;

                        invoice.Customer_Id = model.Customer_Id;
                        invoice.CustomerName = Employee.UserName + "(" + Employee.Name + ")";
                        invoice.Phone = Employee.Phone;
                        invoice.Address = Employee.PresentAddress;

                        invoice.Total = model.Total;

                        invoice.Discount = model.Discount;
                        invoice.MobileBill = model.MobileBill;
                        invoice.BonusOrExtra = model.BonusOrExtra;
                        invoice.TransportBill = model.TransportBill;

                        invoice.Payable = model.Payable;
                        invoice.Paid = model.Paid;
                        invoice.Due = invoice.Payable - invoice.Paid;

                        invoice.Status = ConstantMessage.StatusPending;
                        invoice.InvoiceDate = model.InvoiceDate;

                        invoice.CreatedDate = DateTime.Now;
                        invoice.CreatedBy = User.Identity.Name;

                        db.Invoices.Add(invoice);

                        Transaction newTransaction = new Transaction();

                        newTransaction.Id = Guid.NewGuid().ToString();

                        newTransaction.Invoice = invoice;

                        newTransaction.TransactionType = ConstantMessage.InvoiceTypeSalary;
                        newTransaction.TransactionNo = CustomCode.GeneratenewRandom(ConstantMessage.TransactionSalaryCode);

                        newTransaction.Customer_Id = model.Customer_Id;
                        newTransaction.ReceiveFrom = invoice.CustomerName;

                        newTransaction.PaidAmount = model.Paid;
                        newTransaction.InvoiceNo = invoice.InvoiceNo;

                        newTransaction.CreatedDate = model.InvoiceDate;
                        newTransaction.CreatedBy = User.Identity.Name;

                        db.Transactions.Add(newTransaction);
                        db.SaveChanges();

                        return RedirectToAction("SalaryList");
                    }
                    else
                    {
                        ViewBag.message = "<span class=\"font-red\">Employee " + Employee.Name + " Salary Is Paid in month " + model.InvoiceDate.Value.ToString("MMM-yyyy") + "</span>";
                    }
                }


            }
            catch (Exception ex)
            {
                ViewBag.message = "<span class=\"font-red\">Sale Invoice Save Unsuccessful.Exp :" + ex.Message + "</span>";
            }

            var EmployeeList = db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
                               {
                                   Id = t.Id,
                                   UserName = t.UserName + "(" + t.Name + ")"
                               }).ToList();

            ViewBag.EmployeeList = new SelectList(EmployeeList, "Id", "UserName");
            ViewBag.Option = model.InvoiceType;

            return View();

        }


        [HttpGet]
        public ActionResult EditSalary(string invoiceId)
        {

            try
            {

                Invoice EditSalary = db.Invoices.Find(invoiceId);
                var Employee =
                     db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
                     {
                         Id = t.Id,
                         UserName = t.UserName + "(" + t.Name + ")"
                     }).ToList();

                ViewBag.EmployeeList = new SelectList(Employee, "Id", "UserName", EditSalary.Customer_Id);

                ViewBag.Option = "Salary";
                return View(EditSalary);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("SalaryList");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditSalary(Invoice model, string PaidAmount)
        {
            Invoice invoice = new Invoice();
            Transaction NewTransaction = new Transaction();
            try
            {


                //First Check Is This Month Salary Already Paid of this Employee
               

                List<Invoice> HasSalary = new List<Invoice>();
                //Salary has only one entry at Invoice and Transaction Table
                invoice = db.Invoices.Include(t => t.Transactions).Where(t => t.Id == model.Id).SingleOrDefault();


                    //var PaidAmt = Convert.ToDecimal(PaidAmount);

                    //if (invoice.Due > 0)
                    //    invoice.Due = invoice.Due - PaidAmt;
                    // invoice.Paid += PaidAmt;

                    invoice.InvoiceDate = model.InvoiceDate;
                    invoice.Paid = model.Payable; //added 8-12-17
                    invoice.UpdateBy = User.Identity.Name;
                    invoice.UpdateDate = DateTime.Now;

                    invoice.Total = model.Total;

                    invoice.Discount = model.Discount;
                    invoice.MobileBill = model.MobileBill;
                    invoice.BonusOrExtra = model.BonusOrExtra;
                    invoice.TransportBill = model.TransportBill;

                    invoice.Payable = model.Payable;                    
                    invoice.Due = invoice.Payable - invoice.Paid;

                    //NewTransaction.Id = Guid.NewGuid().ToString();
                    //NewTransaction.Invoice = invoice;
                    //NewTransaction.InvoiceNo = invoice.InvoiceNo;
                    //NewTransaction.TransactionType = invoice.InvoiceType;
                    //NewTransaction.TransactionNo = CustomCode.GeneratenewRandom(ConstantMessage.TransactionSalaryCode);
                    //NewTransaction.Phone = invoice.Phone;
                    //NewTransaction.Customer_Id = invoice.Customer_Id;
                    //NewTransaction.ReceiveFrom = invoice.CustomerName;
                    //NewTransaction.PaidAmount = model.Paid;      // PaidAmt;
                    //NewTransaction.Remarks = "";

                    //NewTransaction.CreatedDate = DateTime.Now;
                    //NewTransaction.CreatedBy = User.Identity.Name;
                    //db.Transactions.Add(NewTransaction);

                    NewTransaction = invoice.Transactions.SingleOrDefault();
                    NewTransaction.PaidAmount = model.Payable;

                    NewTransaction.CreatedDate = model.InvoiceDate;
                    NewTransaction.UpdateDate = DateTime.Now;
                    NewTransaction.UpdateBy = User.Identity.Name;

                    db.Entry(NewTransaction).State = EntityState.Modified;
                    db.Entry(invoice).State = EntityState.Modified;

                    db.SaveChanges();

                    TempData["message"] = "<span class=\"font-green\">Salary Transaction Success</span>";

                    return RedirectToAction("SalaryList");
               
            }
            catch (Exception ex)
            {
                ViewBag.message = "<span class=\"font-red\">Salary Transaction Save Unsuccessful.Exp :" + ex.Message + "</span>";
            }

            var EmployeeList = db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
            {
                Id = t.Id,
                UserName = t.UserName + "(" + t.Name + ")"
            }).ToList();

            ViewBag.EmployeeList = new SelectList(EmployeeList, "Id", "UserName");
            ViewBag.Option = model.InvoiceType;

            return View();

        }

        [HttpPost]
        public ActionResult LoadSalaryData(string InvoiceNo, string EmployeeId, string FromDate, string ToDate)
        {

            DateTime _FromDate;
            DateTime _ToDate;

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value           
            //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;




            var SalaryList = ProcedureFunctionCalled.GetInvoiceInfo("", ConstantMessage.InvoiceTypeSalary, "", "", null, null);

            if (FromDate != null && !string.IsNullOrEmpty(FromDate))
            {
                _FromDate = DateTime.ParseExact(FromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //Convert.ToDateTime(FromDate);
                SalaryList = SalaryList.Where(t => t.InvoiceDate >= _FromDate).ToList();

            }

            if (ToDate != null && !string.IsNullOrEmpty(ToDate))
            {

                _ToDate = DateTime.ParseExact(ToDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //Convert.ToDateTime(ToDate);
                SalaryList = SalaryList.Where(t => t.InvoiceDate <= _ToDate).ToList();
                //Filter
            }

            if (EmployeeId != null && !string.IsNullOrEmpty(EmployeeId))
            {
                SalaryList = SalaryList.Where(t => t.Customer_Id == EmployeeId).ToList();
            }

            totalRecords = SalaryList.Count();
            var data = SalaryList.Skip(skip).Take(pageSize).ToList(); //
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);


        }



        #endregion

        #region EmployeerExpenseAction

        public ActionResult ExpenseList()
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                var Employee =
                  db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).Select(t => new UserIdNameModel
                  {
                      Id = t.Id,
                      UserName = t.UserName + "(" + t.Name + ")"
                  }).ToList();

                ViewBag.Employee = new SelectList(Employee, "Id", "UserName");
                //ViewBag.message = "welcome Profit and Expense List.";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
        }

        public ActionResult AddProfitExpense()
        {
            try
            {
                ViewBag.InvoiceType = CustomCode.TransactionType();
                ViewBag.message = "";
                return View();
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;

            }
            return RedirectToAction("ExpenseList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddProfitExpense(Invoice model)
        {
            try
            {
                //Though There is no customer/Employee but for validation we need to add User Id and Name

                #region CheckSalary
                Invoice HasExpense = new Invoice();
                HasExpense = db.Invoices.Where(t => t.InvoiceType == ConstantMessage.InvoiceTypeProfitExpense && t.InvoiceDate.Value.Month == model.InvoiceDate.Value.Month && t.InvoiceDate.Value.Year == model.InvoiceDate.Value.Year).SingleOrDefault();

                var customer = UserObj.UserManager.FindByNameAsync(User.Identity.Name);
                #endregion

                if (HasExpense == null)
                {

                    ApplicationUser addUser = customer.Result;

                    Invoice invoice = new Invoice();

                    invoice.Id = Guid.NewGuid().ToString();
                    invoice.InvoiceNo = CustomCode.GeneratenewRandom(ConstantMessage.InvoiceProfitExpenseCode);
                    invoice.InvoiceType = ConstantMessage.InvoiceTypeProfitExpense;

                    invoice.Customer_Id = addUser.Id;

                    invoice.CustomerName = addUser.UserName;
                    invoice.Phone = "";
                    invoice.Address = "";

                    invoice.InvoiceDate = model.InvoiceDate;


                    //Profit or Bonus of this Month
                    invoice.BonusOrExtra = model.BonusOrExtra;

                    invoice.RentExpense = model.RentExpense;
                    invoice.ElectricityBill = model.ElectricityBill;
                    invoice.GassBill = model.GassBill;
                    invoice.OtherExpense = model.OtherExpense; //Utility Bill
                    invoice.WaterBill = model.WaterBill;


                    //below is for Salary not mendatory here
                    invoice.Discount = model.Discount;
                    invoice.MobileBill = model.MobileBill;
                    invoice.TransportBill = model.TransportBill;
                    //


                    invoice.Total = model.Total;
                    invoice.Payable = model.Payable;
                    invoice.Paid = model.Paid;
                    invoice.Due = invoice.Payable - invoice.Paid;

                    invoice.Status = ConstantMessage.StatusPending;


                    invoice.CreatedDate = DateTime.Now;
                    invoice.CreatedBy = User.Identity.Name;

                    db.Invoices.Add(invoice);

                    Transaction newTransaction = new Transaction();

                    newTransaction.Id = Guid.NewGuid().ToString();

                    newTransaction.Invoice = invoice;

                    newTransaction.TransactionType = ConstantMessage.InvoiceTypeProfitExpense;
                    newTransaction.TransactionNo = CustomCode.GeneratenewRandom(ConstantMessage.TransactionProfitExpenseCode);

                    newTransaction.Customer_Id = addUser.Id;
                    newTransaction.ReceiveFrom = invoice.CustomerName;

                    newTransaction.PaidAmount = model.Paid;
                    newTransaction.InvoiceNo = invoice.InvoiceNo;

                    newTransaction.CreatedDate = model.InvoiceDate;
                    newTransaction.CreatedBy = User.Identity.Name;

                    db.Transactions.Add(newTransaction);
                    db.SaveChanges();


                    TempData["message"] = ConstantMessage.MessageSuccess;
                    return RedirectToAction("ExpenseList");
                }
                else
                {
                    ViewBag.message = "Invoice already exists in this month " + model.InvoiceDate.Value.ToString("MMM-yy");
                }

            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;

            }

            // ViewBag.InvoiceType = CustomCode.TransactionType();
            return View();

        }

        [HttpGet]
        public ActionResult EditProfitExpense(string invoiceId)
        {

            try
            {
                Invoice EditProfitExpense = db.Invoices.Find(invoiceId);
                ViewBag.message = "";             
                return View(EditProfitExpense);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("ExpenseList");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditProfitExpense(Invoice model)
        {
            Invoice invoice = new Invoice();
            Transaction NewTransaction = new Transaction();
            try
            {

                //First Check Is This Month Salary Already Paid of this Employee
                List<Invoice> HasSalary = new List<Invoice>();
                //Salary has only one entry at Invoice and Transaction Table
                invoice = db.Invoices.Include(t => t.Transactions).Where(t => t.Id == model.Id).SingleOrDefault();


                //Profit or Bonus of this Month
                invoice.BonusOrExtra = model.BonusOrExtra;

                invoice.RentExpense = model.RentExpense;
                invoice.ElectricityBill = model.ElectricityBill;
                invoice.GassBill = model.GassBill;
                invoice.OtherExpense = model.OtherExpense; //Utility Bill
                invoice.WaterBill = model.WaterBill;


                //below is for Salary not mendatory here
                invoice.Discount = model.Discount;
                invoice.MobileBill = model.MobileBill;
                invoice.TransportBill = model.TransportBill;
                //
                
                invoice.Total = model.Total;
                invoice.Payable = model.Payable;
                invoice.Paid = model.Paid;
                invoice.Due = invoice.Payable - invoice.Paid;
                              

                NewTransaction = invoice.Transactions.SingleOrDefault();
                NewTransaction.PaidAmount = model.Payable;

                NewTransaction.UpdateDate = DateTime.Now;
                NewTransaction.UpdateBy = User.Identity.Name;

                db.Entry(NewTransaction).State = EntityState.Modified;
                db.Entry(invoice).State = EntityState.Modified;

                db.SaveChanges();

                TempData["message"] = "<span class=\"font-green\">Profit Expense Transaction Success</span>";
                return RedirectToAction("ExpenseList");

            }
            catch (Exception ex)
            {
                ViewBag.message = "<span class=\"font-red\">Profit Expense Transaction Save Unsuccessful.Exp :" + ex.Message + "</span>";
            }          

            return View();
        }


        [HttpPost]
        public ActionResult LoadExpenseData(string InvoiceNo, string EmployeeId, string FromDate, string ToDate)
        {

            DateTime _FromDate;
            DateTime _ToDate;

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value           
            //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;




            var ExpenseList = ProcedureFunctionCalled.GetInvoiceInfo("", ConstantMessage.InvoiceTypeProfitExpense, "", "", null, null);

            if (FromDate != null && !string.IsNullOrEmpty(FromDate))
            {
                _FromDate = DateTime.ParseExact(FromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //Convert.ToDateTime(FromDate);
                ExpenseList = ExpenseList.Where(t => t.InvoiceDate >=_FromDate).ToList();

            }

            if (ToDate != null && !string.IsNullOrEmpty(ToDate))
            {

                _ToDate = DateTime.ParseExact(ToDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //Convert.ToDateTime(ToDate);
                ExpenseList = ExpenseList.Where(t => t.InvoiceDate <=_ToDate).ToList();
                //Filter
            }

            if (EmployeeId != null && !string.IsNullOrEmpty(EmployeeId))
            {
                ExpenseList = ExpenseList.Where(t => t.Customer_Id == EmployeeId).ToList();
            }

            totalRecords = ExpenseList.Count();
            var data = ExpenseList.Skip(skip).Take(pageSize).ToList(); //
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);






        }
        #endregion
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
