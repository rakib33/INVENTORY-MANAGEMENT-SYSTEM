using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using Microsoft.Reporting.WebForms;

using System.Globalization;
using BookStore.Models;
using BookStore.Models.Inventory;
using System.ComponentModel;


using System.Data.Entity;
using BookStore.App_Code;


namespace BookStore.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       
        // GET: /Report/
        public ActionResult Index()
        {
            try
            {
                ViewBag.Product = new SelectList(db.Products.ToList(), "Id", "Name");

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
        }


        [HttpPost]
        public ActionResult ProductInStock(DateTime PToDate, string ProductId)
        {
            try
            {

                List<StockViewModel> list = new List<StockViewModel>();

                list = ProcedureFunctionCalled.GetProductInStock(ProductId,PToDate);


                LocalReport lr = new LocalReport();            

                ReportDataSource rd = new ReportDataSource();

                DataTable dtFDRStatement = ConvertToDataTable(list.ToList());
                rd.Name = "ProductInStock";
                rd.Value = dtFDRStatement;
                
                ReportParameter[] parameters = new ReportParameter[] 
                           {                            
                             new ReportParameter("ToDate",  PToDate.ToString("dd-MMM-yy HH:mm:ss.fff", CultureInfo.InvariantCulture))                            
                           };

                lr.ReportPath = Server.MapPath("~/Reports/ProductInStock.rdlc");

                lr.SetParameters(parameters);
                lr.DataSources.Add(rd);

                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =

                "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.75in</MarginLeft>" +
                    "  <MarginRight>0.5in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;


                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);



                renderedBytes = lr.Render(reportType);

                string reportName = "ProductInStock-" + PToDate.ToString("dd-MMM-yy") + ".pdf";

                return File(renderedBytes, mimeType, reportName);
            }
            catch (Exception ex)
            {
              
                return RedirectToAction("notFound", "Error", new { message = ex.Message });

            }
        
        }
        #region ProductWiseReport

        public ActionResult ProductWiseLedger(DateTime FromDate, DateTime ToDate, string ProductId)
        {
            try
            {

                List<ProductWiseViewModel> list = new List<ProductWiseViewModel>();

                list = ProcedureFunctionCalled.GetProductWiseLedger(ProductId, FromDate, ToDate);


                LocalReport lr = new LocalReport();

                //string report_title = "FDR Main Statement";;

                ReportDataSource rd = new ReportDataSource();


                DataTable dtFDRStatement = ConvertToDataTable(list.ToList());
                rd.Name = "ProductWiseLedger";
                rd.Value = dtFDRStatement;



                ReportParameter[] parameters = new ReportParameter[] 
                           {
                             new ReportParameter("FromDate", FromDate.ToString("dd-MMM-yy")),
                             new ReportParameter("ToDate",  ToDate.ToString("dd-MMM-yy"))                                
                           };


                lr.ReportPath = Server.MapPath("~/Reports/ProductWiseLeadger.rdlc");

                lr.SetParameters(parameters);
                lr.DataSources.Add(rd);

                //lr.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);



                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =

                "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.75in</MarginLeft>" +
                    "  <MarginRight>0.5in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;


                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);



                renderedBytes = lr.Render(reportType);

                string reportName = "ProductWiseLedger-" + DateTime.Now.ToString("dd-MMM-yy") + ".pdf";

                return File(renderedBytes, mimeType, reportName);
            }
            catch (Exception ex)
            {
                return RedirectToAction("notFound", "Error", new { message = ex.Message });

            }

        }

        [HttpPost]
        public ActionResult ProductBalanceSheet(DateTime PToDate, string ProductId)
        {
            try
            {

                List<ProductWiseViewModel> list = new List<ProductWiseViewModel>();

                list = ProcedureFunctionCalled.GetProductBalanceSheet(ProductId,PToDate);


                LocalReport lr = new LocalReport();            

                ReportDataSource rd = new ReportDataSource();

                DataTable dtFDRStatement = ConvertToDataTable(list.ToList());
                rd.Name = "ProductBalanceSheet";
                rd.Value = dtFDRStatement;
                
                ReportParameter[] parameters = new ReportParameter[] 
                           {                            
                             new ReportParameter("ToDate",  PToDate.ToString("dd-MMM-yy"))                                
                           };
                
                lr.ReportPath = Server.MapPath("~/Reports/ProductBalanceSheet.rdlc");

                lr.SetParameters(parameters);
                lr.DataSources.Add(rd);

                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =

                "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.75in</MarginLeft>" +
                    "  <MarginRight>0.5in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;


                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);



                renderedBytes = lr.Render(reportType);

                string reportName = "ProductBalanceSheet-" + PToDate.ToString("dd-MMM-yy") + ".pdf";

                return File(renderedBytes, mimeType, reportName);
            }
            catch (Exception ex)
            {
                return RedirectToAction("notFound", "Error", new { message = ex.Message });

            }
        
        }




        public ActionResult InventoryGainLossByDate(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {

                List<ProductWiseViewModel> list = new List<ProductWiseViewModel>();

                list = ProcedureFunctionCalled.GetInventoryGainLossByDate(FromDate, ToDate);


                LocalReport lr = new LocalReport();

                //string report_title = "FDR Main Statement";;

                ReportDataSource rd = new ReportDataSource();


                DataTable dtFDRStatement = ConvertToDataTable(list.ToList());
                rd.Name = "ProductWiseLedger";
                rd.Value = dtFDRStatement;



                ReportParameter[] parameters = new ReportParameter[] 
                           {
                             new ReportParameter("FromDate", FromDate.HasValue ? FromDate.Value.ToString("dd-MMM-yy"):null),
                             new ReportParameter("ToDate",  ToDate.Value.ToString("dd-MMM-yy"))                                
                           };


                lr.ReportPath = Server.MapPath("~/Reports/InventoryGailnLossByDate.rdlc");

                lr.SetParameters(parameters);
                lr.DataSources.Add(rd);

                //lr.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);



                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =

                "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.75in</MarginLeft>" +
                    "  <MarginRight>0.5in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;


                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);



                renderedBytes = lr.Render(reportType);

                string reportName = "ProductWiseLedger-" + DateTime.Now.ToString("dd-MMM-yy") + ".pdf";

                return File(renderedBytes, mimeType, reportName);
            }
            catch (Exception ex)
            {
                return RedirectToAction("notFound", "Error", new { message = ex.Message });

            }

        }
        #endregion

        [HttpPost]
        public ActionResult Generate()
        {
            LocalReport lr = new LocalReport();

            lr.ReportPath = Server.MapPath("~/Reports/TestReport.rdlc");
            //lr.SetParameters(parameters);
            //lr.DataSources.Add(rd);
            //lr.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);


            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>1in</MarginLeft>" +
                "  <MarginRight>1in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;


            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);



            renderedBytes = lr.Render(reportType);

            string reportName = "PurchaseNote.pdf";

            return File(renderedBytes, mimeType, reportName);



        }

        public ActionResult GetCollectedProduct()
        {
            try
            {
                var list = (from e in db.Products.ToList()
                            join c in db.Catagories.ToList() on e.Catagory_Id equals c.Id
                            join b in db.Brands.ToList() on e.Brand_Id equals b.Id
                            select new Product
                            {
                                Id = e.Id,
                                BarCode = e.BarCode,
                                Brand_Id = b.Name,
                                Catagory_Id = c.Name,
                                Name = e.Name,
                                CostPrice = e.CostPrice,
                                CreatedBy = e.CreatedBy,
                                CreatedDate = e.CreatedDate,
                                Description = e.Description,
                                ParentId = e.ParentId,
                                SalePrice = e.SalePrice,
                                UpdateBy = e.UpdateBy,
                                UpdateDate = e.UpdateDate
                            }).OrderBy(t => t.Catagory_Id).ThenBy(t => t.Name).ToList();

              

                LocalReport lr = new LocalReport();
                
                //string report_title = "FDR Main Statement";;
                
                ReportDataSource rd = new ReportDataSource();
              

                DataTable dtFDRStatement =ConvertToDataTable(list.ToList());
                rd.Name = "ProductList";
                rd.Value = dtFDRStatement;



                //ReportParameter[] parameters = new ReportParameter[] 
                //           {
                //             new ReportParameter("FromDate", fromDateFS.Value.ToString("dd-MMM-yy")),
                //             new ReportParameter("ToDate",  toDateFS.Value.ToString("dd-MMM-yy"))                                
                //           };


                lr.ReportPath = Server.MapPath("~/Reports/CollectedProduct.rdlc");

               // lr.SetParameters(parameters);
                lr.DataSources.Add(rd);
                //lr.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);



                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =

                "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.75in</MarginLeft>" +
                    "  <MarginRight>0.5in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;


                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);



                renderedBytes = lr.Render(reportType);

                string reportName = "CollectedProduct-" + DateTime.Now.ToString("dd-MMM-yy") + ".pdf";

                return File(renderedBytes, mimeType, reportName);
            }
            catch (Exception ex)
            { 
             return RedirectToAction("notFound", "Error", new { message = ex.Message });
           
            }
        
        }


        public ActionResult GetInvoice(string InvoiceId,string InvoiceType)
        {
            try
            {

                List<Invoice> Invoice = new List<Invoice>();
                List<Purchase> InvoiceDetails = new List<Purchase>();
              

                if (InvoiceType == ConstantMessage.InvoiceTypePurchase)
                {
                    Invoice = db.Invoices.Include(t => t.Purchases).AsNoTracking().Where(t => t.Id == InvoiceId).ToList();
               
                    InvoiceDetails = Invoice[0].Purchases.Select(t => new Purchase
                    {
                        Id = t.Id,
                        Product_Id = db.Products.Find(t.Product_Id).Name,
                        Qty = t.Qty,
                        BuyRate = t.BuyRate,
                        BuyTotal = t.BuyTotal,
                        SaleRate = 0,
                        SaleTotal = 0,
                        Status = t.Status

                    }).ToList();
                }
                else {
                    Invoice = db.Invoices.Include(t => t.Sales).Where(t => t.Id == InvoiceId).ToList();
                    InvoiceDetails = Invoice[0].Sales.Select(t => new Purchase
                    {
                        Id = t.Id,
                        Product_Id = db.Products.Find(t.Product_Id).Name,
                        Qty = t.Qty,
                        BuyRate = 0,
                        BuyTotal = 0,
                        SaleRate = t.SaleRate,
                        SaleTotal = t.SaleTotal,
                        Status = t.Status

                    }).ToList();
                }

                

                LocalReport lr = new LocalReport();             

                ReportDataSource rd = new ReportDataSource();
                ReportDataSource dd = new ReportDataSource();

                DataTable dtInvoice = ConvertToDataTable(Invoice.ToList());
                rd.Name = "Invoice";
                rd.Value = dtInvoice;

                dd.Name = "InvoiceDetails";
                dd.Value = ConvertToDataTable(InvoiceDetails.ToList());


                lr.ReportPath = Server.MapPath("~/Reports/Invoice.rdlc");

              
                lr.DataSources.Add(rd);
                lr.DataSources.Add(dd);


                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =

                "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.75in</MarginLeft>" +
                    "  <MarginRight>0.5in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;


                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);



                renderedBytes = lr.Render(reportType);

                string reportName =InvoiceType+" Invoice-" + DateTime.Now.ToString("dd-MMM-yy") + ".pdf";

                return File(renderedBytes, mimeType, reportName);
            }
            catch (Exception ex)
            {
                return RedirectToAction("notFound", "Error", new { message = ex.Message });

            }

        }

      


        //public ActionResult InvoiceWiseLedger(DateTime? FromDate, DateTime? ToDate, string InvoiceNo,string IsExcel)
        //{
        //    try
        //    {

        //        List<VM_InvoiceWiseLedger> list = new List<VM_InvoiceWiseLedger>();

        //        list = ProcedureFunctionCalled.GetInvoiceWiseLedger(InvoiceNo, FromDate.Value , ToDate.Value);


        //        LocalReport lr = new LocalReport();

        //        //string report_title = "FDR Main Statement";;

        //        ReportDataSource rd = new ReportDataSource();


        //        DataTable dtFDRStatement = ConvertToDataTable(list.ToList());
        //        rd.Name = "InvoiceWiseLedger";
        //        rd.Value = dtFDRStatement;



        //        ReportParameter[] parameters = new ReportParameter[] 
        //                   {
        //                     new ReportParameter("FromDate", FromDate.Value.ToString("dd-MMM-yy")),
        //                     new ReportParameter("ToDate",  ToDate.Value.ToString("dd-MMM-yy"))                                
        //                   };


        //        lr.ReportPath = Server.MapPath("~/Reports/InvoiceWiseLedger.rdlc");

        //        lr.SetParameters(parameters);
        //        lr.DataSources.Add(rd);

        //        //lr.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);

        //        string reportName = "InvoiceWiseLedger-" + DateTime.Now.ToString("dd-MMM-yy") + ".pdf";

        //        string reportType = "PDF";
        //        string mimeType;
        //        string encoding;
        //        string fileNameExtension;

        //        if (IsExcel == "true")
        //        {
        //            reportType = "excel";
        //            reportName = "InvoiceWiseLedger-" + DateTime.Now.ToString("dd-MMM-yy") + ".{0}";
        //        }
        //        string deviceInfo =

        //        "<DeviceInfo>" +
        //            "  <OutputFormat>'"+ reportType+"'</OutputFormat>" +
        //            "  <PageWidth>8.27in</PageWidth>" +
        //            "  <PageHeight>11.69in</PageHeight>" +
        //            "  <MarginTop>0.5in</MarginTop>" +
        //            "  <MarginLeft>0.75in</MarginLeft>" +
        //            "  <MarginRight>0.5in</MarginRight>" +
        //            "  <MarginBottom>0.5in</MarginBottom>" +
        //            "</DeviceInfo>";

        //        Warning[] warnings;
        //        string[] streams;
        //        byte[] renderedBytes;


        //        renderedBytes = lr.Render(
        //            reportType,
        //            deviceInfo,
        //            out mimeType,
        //            out encoding,
        //            out fileNameExtension,
        //            out streams,
        //            out warnings);
                
        //        renderedBytes = lr.Render(reportType);

        //        if (IsExcel == "true")
        //        {
        //            return File(renderedBytes, mimeType, string.Format(reportName, fileNameExtension));
        //        }
        //        else
        //        {
        //            return File(renderedBytes, mimeType, reportName);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("notFound", "Error", new { message = ex.Message });

        //    }

        //}



        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    catch { }
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}