using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using System.Data;


using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

using BookStore.Models;
using BookStore.Models.Inventory;

namespace BookStore.Controllers
{
    [Authorize]
    public class DbBackUpController : Controller
    {
        //Incremental Back Up Merge with existing Data
        //Full Back Up
        #region HelpLink
        // https://www.codeproject.com/Tips/873677/SQL-Server-Database-Backup-and-Restore-in-Csharp
        // http://www.c-sharpcorner.com/UploadFile/rahul4_saxena/take-sql-server-data-base-backup-in-Asp-Net/
        #endregion

        ApplicationDbContext db = new ApplicationDbContext();

        SqlConnection con = new SqlConnection();
        SqlCommand sqlcmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();   
        // GET: /DbBackUp/

        [HttpPost]
        public ActionResult BackUp(DbBackUp model)
        {
            // Within the code body set your variable    
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            con.ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=InventoryDB;User ID=sa;Password=cse;Integrated Security=true;";

            string backupDIR = model.DriveName + ":\\" + model.DirectoryPath; // "E:\\BackUp";
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }
            try
            {
                con.Open();
                sqlcmd = new SqlCommand("backup database InventoryDB to disk='" + backupDIR + "\\" + DateTime.Now.ToString("dd-MMM-yy-HH-mm-ss") + ".Bak'", con);
                sqlcmd.ExecuteNonQuery();
                con.Close();
                TempData["message"] = "<span class=\"font-green\">Database Back Up Success.Path " + backupDIR + "</span>";
            }
            catch (Exception ex)
            {
                string ms = ex.Message;
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
               // lblError.Text = "Error Occured During DB backup process !<br>" + ex.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                var model = db.DbBackUps.SingleOrDefault();
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("notFound", "Error", new { message =ex.Message});
            }
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