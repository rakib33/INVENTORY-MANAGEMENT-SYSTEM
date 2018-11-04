using BookStore.Models;
using BookStore.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BookStore.Models.Infrastructure;

namespace BookStore.App_Code
{
    public class ProcedureFunctionCalled
    {

        public static List<PurchaseDislayModel> GetPurchaseDisplayInfo(string Status,string SupplierId,DateTime? FromDate,DateTime? ToDate)
        {
            try
            {
                var OrderRef="";
                string query = "SP_GetPurchaseInfo @InvoiceNo";
                SqlParameter InvoiceNo = new SqlParameter("@InvoiceNo", OrderRef);
             
                List<PurchaseDislayModel> Result = new List<PurchaseDislayModel>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<PurchaseDislayModel>(query, InvoiceNo).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public static List<Invoice> GetInvoiceInfo(string InvoiceId,string InvoiceType, string CustomerID,string Phone, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {               
                string query = "SP_GetInvoiceInfo @InvoiceId,@PhoneNumber,@CustomerId,@From_date,@To_date,@InvoiceType";
                SqlParameter invoiceId = new SqlParameter("@InvoiceId", (object)DBNull.Value);
                SqlParameter phoneNumber = new SqlParameter("@PhoneNumber", (object)DBNull.Value);
                SqlParameter customerID = new SqlParameter("@CustomerId", (object)DBNull.Value);
                SqlParameter from_date = new SqlParameter("@From_date", (object)DBNull.Value);
                SqlParameter to_date = new SqlParameter("@To_date", (object)DBNull.Value);
                SqlParameter invoiceType = new SqlParameter("@InvoiceType", InvoiceType);

                List<Invoice> Result = new List<Invoice>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<Invoice>(query, invoiceId, phoneNumber, customerID, from_date, to_date, invoiceType).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }


        public static List<StockViewModel> GetProductInStock(string productid,DateTime ToDate)
        {
            try
            {

                string query = "select * from dbo.FN_ProductInStock('" + productid + "','','" + ToDate + "')";
                //"select * from dbo.FN_ProductBalanceSheet('" + ProductId + "','" + toDate + "')";

                List<StockViewModel> Result = new List<StockViewModel>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<StockViewModel>(query).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }

            
        }

        public static List<Transaction> GetTransactionInfo(string invoiceType,DateTime fromDate,DateTime toDate)
        {

            try
            {
                //var from = fromDate.ToString("yyyy-MM-dd");
               // var to = toDate.ToString("yyyy-MM-dd");
               
                string query = "SP_GetTransactionInfo @To_Date,@From_Date,@InvoiceType";

                SqlParameter From_Date = new SqlParameter("@From_Date", fromDate); //DateTime.Parse(from)
                            // From_Date.Value = fromDate;

                SqlParameter To_Date = new SqlParameter("@To_Date",toDate ); //DateTime.Parse(to)
                             //To_Date.Value = toDate;
                 SqlParameter InvoiceType;
                if(invoiceType ==null || invoiceType =="")
                   InvoiceType = new SqlParameter("@InvoiceType", (object)DBNull.Value);
                else
                    InvoiceType = new SqlParameter("@InvoiceType", invoiceType);


                List<Transaction> Result = new List<Transaction>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<Transaction>(query, From_Date, To_Date, InvoiceType).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }


        public static List<ProductWiseViewModel> GetProductWiseLedger(string ProductId, DateTime fromDate, DateTime toDate)
        {

            try
            {
          
                string query = "SP_ProductWiseLedger @Product_Id,@From_Date,@To_Date";

                SqlParameter From_Date = new SqlParameter("@From_Date", fromDate);          

                SqlParameter To_Date = new SqlParameter("@To_Date", toDate); 
         
                SqlParameter Product_Id = new SqlParameter("@Product_Id", ProductId);  
                     
                List<ProductWiseViewModel> Result = new List<ProductWiseViewModel>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<ProductWiseViewModel>(query, From_Date, To_Date, Product_Id).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }


        public static List<ProductWiseViewModel> GetInventoryGainLossDashBoard(DateTime? fromDate, DateTime? toDate)
        {

            try
            {

                string query = "Beta_SP_InvProfitGainLossDashboard @From_Date,@To_Date";


                SqlParameter From_Date;

                if (fromDate.HasValue)
                    From_Date = new SqlParameter("@From_Date", fromDate.Value);
                else
                    From_Date = new SqlParameter("@From_Date", (object)DBNull.Value);

                SqlParameter To_Date = new SqlParameter("@To_Date", toDate.Value);


                List<ProductWiseViewModel> Result = new List<ProductWiseViewModel>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<ProductWiseViewModel>(query, From_Date, To_Date).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }


        public static List<ProductWiseViewModel> GetInventoryGainLossByDate(DateTime? fromDate, DateTime? toDate)
        {

            try
            {

                string query = "Beta_SP_InventoryProfitGainLoss @From_Date,@To_Date";


                SqlParameter From_Date ;

                if(fromDate.HasValue)
                    From_Date = new SqlParameter("@From_Date", fromDate.Value);
                else
                    From_Date = new SqlParameter("@From_Date",(object)DBNull.Value);

                SqlParameter To_Date = new SqlParameter("@To_Date", toDate.Value);

              
                List<ProductWiseViewModel> Result = new List<ProductWiseViewModel>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<ProductWiseViewModel>(query, From_Date, To_Date).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public static List<ProductWiseViewModel> GetProductBalanceSheet(string ProductId,DateTime toDate)
        {

            try
            {

                string query = "select * from dbo.Beta_FN_ProductWisePortfolio('" + ProductId + "','" + toDate + "')";             

                List<ProductWiseViewModel> Result = new List<ProductWiseViewModel>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<ProductWiseViewModel>(query).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }


        public static List<VM_InvoiceWiseLedger> GetInvoiceWiseLedger(string InvoiceNo, DateTime? fromDate, DateTime? toDate)
        {

            try
            {

                string query = "select * from dbo.FN_InvoiceWiseBalanceSheet('"+InvoiceNo+"','"+fromDate.Value+"','"+toDate.Value+"')"; 

                List<VM_InvoiceWiseLedger> Result = new List<VM_InvoiceWiseLedger>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<VM_InvoiceWiseLedger>(query).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }



        public static List<ApplicationUser> GetUserInfo(string userType)
        {

            try
            {
                string query = "SP_GetUserInfo @UserType";
                SqlParameter UserType = new SqlParameter("@UserType", userType);

                List<ApplicationUser> Result = new List<ApplicationUser>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<ApplicationUser>(query, UserType).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }


        public static List<Product> GetProductInfo()
        {

            try
            {
                string query = "SP_GetProductInfo";               

                List<Product> Result = new List<Product>();
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    Result = NewContext.Database.SqlQuery<Product>(query).ToList();
                }

                return Result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}