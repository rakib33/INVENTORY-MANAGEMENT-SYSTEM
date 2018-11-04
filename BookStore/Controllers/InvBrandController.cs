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

namespace BookStore.Controllers
{
    [Authorize]
    public class InvBrandController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /InvBrand/
        //This is data table parent child tree view 
        //http://www.jqueryscript.net/demo/Minimal-Tree-Table-jQuery-Plugin-For-Bootstrap-TreeTable/
        public ActionResult Index()
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                return View(db.Brands.Include(t=>t.Catagory).OrderBy(t=>t.Catagory.Name).ThenBy(t=>t.Name).ToList());
            }         
           catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
            
        }

        // GET: /InvBrand/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
               // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("notFound", "Error");
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
               // return HttpNotFound();
                return RedirectToAction("notFound", "Error");
            }
            return View(brand);
        }

        // GET: /InvBrand/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");

                BrandViewModel model = new BrandViewModel();
                model.Brand = new List<Brand> { new Brand { Id="", Name="", ISIN="", Description="" } };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
        }

       
    

 
        /// <summary>
        /// I am using EntityFramework Transaction roll back here with SQL Server database.This is help link
        /// http://www.c-sharpcorner.com/UploadFile/ff2f08/working-with-transaction-in-entity-framework-6-0/
        /// </summary>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BrandViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {

                                foreach(var item in model.Brand)
                                {
                                    Brand newBrand = new Brand();
                                    newBrand.Id = Guid.NewGuid().ToString();
                                    newBrand.Catagory_Id = model.Catagory_Id;

                                    newBrand.ISIN = item.ISIN;
                                    newBrand.Name = item.Name;
                                    newBrand.Description = item.Description;

                                    newBrand.CreatedBy = User.Identity.Name;
                                    newBrand.CreatedDate = DateTime.Now;

                                    context.Brands.Add(newBrand);
                                    context.SaveChanges();
                                
                                }
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                ModelState.AddModelError("", "Transaction failed due to- " + ex.Message);
                            }
                        }
                    }  
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Can not save due to- " + ex.Message);
            }

            ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");
    
            return View(model);
        }

        // GET: /InvBrand/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
               
                Brand model = db.Brands.Find(id);
                if (model == null)
                {
                    TempData["message"] = "<span class=\"font-red\">Records not Found.</span>";
                }
                else
                {
                    var SubCatagory = db.Catagories.Where(t=>t.Id == model.Catagory_Id).SingleOrDefault();
                    var MainCatagory = db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList();

                    //In Edit mode ViewBag name and DropdownList("<Name>" .. must not be same in edit mode.because selected value not work
                    ViewBag.MainCatagoryId = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Parent_Id);
                    ViewBag.CatagoryId = new SelectList(db.Catagories.Where(t => t.Parent_Id == SubCatagory.Parent_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Id);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
           
        }

        // POST: /InvBrand/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.UpdateBy = User.Identity.Name;
                    model.UpdateDate = DateTime.Now;

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Can not save edited data due to- " + ex.Message);
            }
            // If model failed
            var SubCatagory = db.Catagories.Where(t => t.Id == model.Catagory_Id).SingleOrDefault();
            ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Parent_Id);
            ViewBag.Catagory_Id = new SelectList(db.Catagories.Where(t => t.Parent_Id == SubCatagory.Parent_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Id);
   
            return View(model);
        }

        // GET: /InvBrand/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: /InvBrand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region Helper
       
        public JsonResult getSubCatagory(string id)
        {

            List<Catagory> MainCatagory = db.Catagories.Where(t => t.Parent_Id == id).ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--Select Sub Catagory--", Value = null });

            foreach (var row in MainCatagory)
            {

                list.Add(new SelectListItem { Text = row.Name, Value = row.Id });

            }

            return Json(list, JsonRequestBehavior.AllowGet);
            //return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));

        }

        
        public JsonResult getBrand(string id)
        {

            List<Brand> brand = db.Brands.Where(t => t.Catagory_Id == id).ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--Select Brand--", Value = null });

            foreach (var row in brand)
            {

                list.Add(new SelectListItem { Text = row.Name, Value = row.Id });

            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));

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
