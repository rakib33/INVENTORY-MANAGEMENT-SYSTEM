using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using BookStore.App_Code;

namespace BookStore.App_Code
{
    public  class  ConstantMessage
    {
        public const string MessageSuccess = "success";

        public const string  UserTypeEmployee ="Employee";
        public const string UserTypeCustomer = "Customer";
        public const string UserTypeSupplier = "Supplier";  

        public const string StatusApproved = "Approved";
        public const string StatusPending = "Pending";
        public const string StatusReject = "Reject";
        public const string StatusAccept = "Accept";
        public const string StatusSuccess = "Success";
        public const string StatusSold = "Sold";

        public const string StatusPaid = "Paid";

       // Purchase,Sale,Salary
        public const string InvoiceTypePurchase = "Purchase";
        public const string InvoiceTypeSale = "Sale";
        public const string InvoiceTypeSalary = "Salary";
        public const string InvoiceTypeExpense = "Expense"; // extra cost of the company
        public const string InvoiceTypeProfitExpense = "ProfitExpense"; //Extra Profit of this Compney

        public const string InvoicePurchaseCode = "INV-P-";
        public const string InvoiceSaleCode = "INV-S-";
        public const string InvoiceSalaryCode = "INV-Sl-";

        public const string InvoiceProfitExpenseCode = "INV-PE-";

        public const string TransactionCode = "TRAN-";
        public const string TransactionSalaryCode = "TRAN-Sl-";
        public const string TransactionProfitExpenseCode = "TRAN-PE-";


        #region OthersMessage

        public static string CONST_FTPUrl = @"ftp://162.217.144.39/app.ingenstudio.com/OrderFileUpload/";

        public static string PAYPAL_ReturnUrl_FROM_INGEN_app_ingenstudio = "http://app.ingenstudio.com/Paypal/PaymentWithPaypal?";

        public static string PAYPAL_ReturnUrl_FROM_BOSL_SERVER = "http://95.138.162.122/app.ingenstudio.com/Paypal/PaymentWithPayPal?";  //for BOSL server;

        public static string PAYPAL_ReturnUrl_FROM_INGEN_orderpro = "http://orderpro.ingenstudio.com/Paypal/PaymentWithPaypal?";

        public static string PAYPAL_ReturnUrl_FROM_LOCALHOST = "http://localhost:81/app.ingenstudio.com/Paypal/PaymentWithPaypal?";

        public static string CONST_UserName = "soft2016";
        public static string CONST_Pass = "jk$878no";


        #region IngenMailAddressAndSubjectAndBody

        public static string MailAddressInfo = "info@risoftbd.com";          // "support@ingenstudio.com";
        public static string MailAddressInfoPassword = "Info@09033";                // "Support!@34";

        public static string MailAddressRegister = "register@risoftbd.com";              // "info.ingenstudio@gmail.com";         //
        public static string MailAddressRegisterPassword = "c9Rgc04!";                // "info2016$%^&*";              // 


        public static string MailAddressSales = "sales@risoftbd.com";                // "info.ingenstudio@gmail.com";           // 
        public static string IMailAddressSalesPassword = "oJt63v@6";                   // "info2016$%^&*";                 // 

        public static string MailAddressSupport = "support@risoftbd.com";                //"info.ingenstudio@gmail.com";             //
        public static string MailAddressSupportPassword = "Yex9m3#5";                     // "info2016$%^&*";                 // 



        public static string IngenRegistrationSubject = "Activate your Account.";
        public static string IngenOrderPlacedSubject = "Confirming your Order No: ";
        public static string IngenTransactionSubject = "Payment Confirmation of your Order No: ";


        #endregion


        public static Stream ftpStream = null;
        public static int bufferSize = 2048;

        public static string CONST_FileToUploadDirectoryName = "OrderFileUpload";

        #region UserStatus

        public static string CONST_StatusPending = "Pending";  //Active/Suspended
        public static string CONST_StatusConfirmed = "Confirmed";
        public static string CONST_StatusSuspended = "Suspended";
        public static string CONST_StatusPaid = "Paid";

        public static string CONST_User_AccountTypePrepaidMsg = "Prepaid";
        public static string CONST_User_AccountTypePostPaidMsg = "PostPaid";

        #endregion

        public static string CONST_MainServiceE_Commerce = "E-Commerce";
        public static string CONST_Mainservice_AddOns = "Add-ons";
        public static string CONST_MainService_ReTouching = "Re-Touching";

        public static string CONST_Active = "Active";

        public static string CONST_PaymentType = "Paypal";

        public static string CONST_CurrencyUSD = "USD";

        public static string CONST_CustomerExistsMsg = "User already exists.Please Login.";

        public static string CONST_AccountCatagoryRegular = "Regular";
        public static string CONST_AccountCatagoryCorporate = "Corporate";
    //    public static List<FtpsFile> UploadedFileList = new List<FtpsFile>();


        #region GetPaymentFuncVariable

        public static string CONST_Header_GrandTotal = "Grand Total:";
        public static string CONST_Header_Discount = "Discount";
        public static string CONST_Header_NetAmount = "Net Amount:";

        public static string CONST_PaypalTransactionFailedMsg = "Paypal said 'Transaction Failed'";
        #endregion

        public static string CONST_ServiceErrorMsg = "This Service is not available in your region. Please contact with us.";
        public static string CONST_OrderReferenceErrorMsg = "Can not find Order Reference!";

        #region DeclareGlobalVariable

        public static string GlobalDirectoryPath = "";
        public static string ClientCode = "";
        public static string OrderNumber = "";
        public static string DirectoryPath = "";
        public static bool LoginFromAccount = false;
        #endregion


        #region ExceptionMessage
        public static string CONST_DatabaseInitialize_EXP = "An exception occurred while initializing the database";
        public static string CONST_DatabaseInitialize_EXP_ReturnMsg = "Ex001: Respond failed.Please try again.";
        //You must call the "WebSecurity.InitializeDatabaseConnection" method before you call any other method of the "WebSecurity" class. This call should be placed in an _AppStart.cshtml file in the root of your site

        public static string CONST_WebSecurityDbInitializeEXP = "WebSecurity.InitializeDatabaseConnection";
        public static string CONST_WebSecurityDbInitializeEXP_ReturnMsg = "Ex002: Respond failed.Please try again.";

        //Attempt to SignIn/SignUp
        //An error occurred while getting provider information from the database. This can be caused by Entity Framework 
        //using an incorrect connection string. Check the inner exceptions for details and ensure that the connection string is correct
        public static string CONST_DBConnectionExp = "An error occurred while getting provider information from the database";
        public static string CONST_DBConnectionExp_ReturnMsg = "Connection time out.Please try again.";
        #endregion
        #endregion
    }
}