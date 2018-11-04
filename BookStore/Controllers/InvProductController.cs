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
using BookStore.Repository;
using BookStore.App_Code;

namespace BookStore.Controllers
{
    [Authorize]
    public class InvProductController : Controller
    {
         private IImage ImageRepository = null;

        public InvProductController()
        {
            ImageRepository = new ImageRepository();
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /InvProduct/
        public ActionResult Index()
        {
          try{
                ViewBag.message = Convert.ToString(TempData["message"]);

                List<Product> list = new List<Product>();
                list = ProcedureFunctionCalled.GetProductInfo();
                ViewBag.message = "";
                return View(list);
             }         
           catch (Exception ex)
            {
                string ms = ex.Message;
                return RedirectToAction("notFound", "Error");
            }
        }


        public ActionResult PriceChart()
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);

                List<Product> list = new List<Product>();
                list = ProcedureFunctionCalled.GetProductInfo();
                ViewBag.message = "";
                return View(list);
            }
            catch (Exception ex)
            {
                string ms = ex.Message;
                return RedirectToAction("notFound", "Error");
            }
        
        }

        // GET: /InvProduct/Details/5
        public ActionResult Details(string id)
        {
          
            if (id == null)
            {
                TempData["message"] = "<span class=\"font-red\">Record not found.</span>";
                
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                TempData["message"] = "<span class=\"font-red\">Record not found.</span>";
               // return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // GET: /InvProduct/Create
        public ActionResult Create()
        {
             try
            {
            ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");

            ViewBag.message = "";
            return View();
            }
             catch (Exception ex)
             {
                 TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
             }

             return RedirectToAction("Index");
        }

        // POST: /InvProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            
            
            if (ModelState.IsValid)
            {
                UserImage ImageObj = new UserImage();
                if (file != null)
                {
                    ImageObj = ImageRepository.SetImage(file);
                }
                try
                {
                    var Exists = db.Products.Where(t=>t.Name.ToUpper() == product.Name.ToUpper() || t.BarCode == product.BarCode ).SingleOrDefault();

                    if (file != null && ImageObj == null)
                    {
                        ModelState.AddModelError("", "Uploaded Image get Null Value.");
                    }
                    else if(Exists !=null)
                    {
                        ModelState.AddModelError("", "Prduct " + Exists.Name +" and Product code "+Exists.BarCode+" already Exists!");                    
                    }
                    else
                    {
                        product.Id = Guid.NewGuid().ToString();
                        product.CreatedBy = User.Identity.Name;
                        product.CreatedDate = DateTime.Now;

                        if (product.DisplayCostPrice < 1 || product.DisplayCostPrice == null)
                            product.DisplayCostPrice = product.CostPrice;

                        ImageObj.Id = Guid.NewGuid().ToString();

                        ImageObj.CreatedBy = User.Identity.Name;
                        ImageObj.CreatedDate = DateTime.Now;

                        ImageObj.FK_Product_Id = product.Id;
                        ImageObj.Product = product;

                        db.Products.Add(product);
                        db.Images.Add(ImageObj);

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }


                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Can not save due to- " + ex.Message);
                }

            }


            try
            {
                ViewBag.MainCatagory = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name");
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: /InvProduct/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {

                Product model = db.Products.Find(id);
                if (model == null)
                {
                    TempData["message"] = "<span class=\"font-red\">Records not Found.</span>";
                }
                else
                {
                    var SubCatagory = db.Catagories.Where(t => t.Id == model.Catagory_Id).SingleOrDefault();
                    var MainCatagory = db.Catagories.Where(t => t.Parent_Id == null).OrderBy(t => t.Name).ToList();

                    //In Edit mode ViewBag name and DropdownList("<Name>" .. must not be same in edit mode.because selected value not work
                    ViewBag.MainCatagoryId = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Parent_Id);
                    ViewBag.CatagoryId = new SelectList(db.Catagories.Where(t => t.Parent_Id == SubCatagory.Parent_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Id);

                    ViewBag.BrandId = new SelectList(db.Brands.Where(t => t.Catagory_Id == model.Catagory_Id).OrderBy(t => t.Name).ToList(), "Id", "Name",model.Brand_Id);
                    ViewBag.message = "";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
        }

        // POST: /InvProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //var Exists = db.Products.Where(t => t.Name.ToUpper() == model.Name.ToUpper() || t.BarCode == model.BarCode).SingleOrDefault();

                    //if (Exists != null)
                    //{
                    //    ModelState.AddModelError("", "Prduct " + Exists.Name + " and Barcode " + Exists.BarCode + " already Exists!");
                    //}
                    //else
                    //{

                        Product ProductToEdited = db.Products.Where(t => t.Id == model.Id).SingleOrDefault();

                        ProductToEdited.DisplayCostPrice = model.DisplayCostPrice;

                        ProductToEdited.Name = model.Name;
                        ProductToEdited.BarCode = model.BarCode;
                        ProductToEdited.Catagory_Id = model.Catagory_Id;
                        ProductToEdited.Brand_Id = model.Brand_Id;
                        ProductToEdited.CostPrice = model.CostPrice;
                        ProductToEdited.SalePrice = model.SalePrice;
                        ProductToEdited.ParentId = model.ParentId; // not necessary this concept is not apply in Product table parent-child menu
                        ProductToEdited.Description = model.Description;

                        ProductToEdited.UpdateBy = User.Identity.Name;
                        ProductToEdited.UpdateDate = DateTime.Now;

                        UserImage ImageEdit = db.Images.Include(t => t.Product).Where(t => t.FK_Product_Id == model.Id).SingleOrDefault();
                        var ImageObj = new UserImage();
                        if (file != null)
                        {
                            ImageObj = ImageRepository.SetImage(file);
                        }


                        db.Entry(ProductToEdited).State = EntityState.Modified;

                        if (ImageEdit == null)
                        {
                            ImageObj.Id = Guid.NewGuid().ToString();

                            ImageObj.CreatedBy = User.Identity.Name;
                            ImageObj.CreatedDate = DateTime.Now;

                            ImageObj.FK_Product_Id = ProductToEdited.Id;
                            ImageObj.Product = ProductToEdited;


                            db.Images.Add(ImageObj);
                        }
                        else
                        {
                            if (file != null)
                            {
                                ImageEdit.ImageStream = ImageObj.ImageStream;
                            }

                            ImageEdit.UpdateBy = User.Identity.Name;
                            ImageEdit.UpdateDate = DateTime.Now;
                            ImageEdit.Product = ProductToEdited;

                            db.Entry(ImageEdit).State = EntityState.Modified;
                        }


                        db.SaveChanges();

                        TempData["message"] = "<span class=\"font-green\">Product Edit Successful!</span>";
                        return RedirectToAction("Index");
                   // }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Can not save edited data due to- " + ex.Message);
            }

            try
            {
                var SubCatagory = db.Catagories.Where(t => t.Id == model.Catagory_Id).SingleOrDefault();
                var MainCatagory = db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList();

                //In Edit mode ViewBag name and DropdownList("<Name>" .. must not be same in edit mode.because selected value not work
                ViewBag.MainCatagoryId = new SelectList(db.Catagories.Where(t => t.Parent_Id == "0").OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Parent_Id);
                ViewBag.CatagoryId = new SelectList(db.Catagories.Where(t => t.Parent_Id == SubCatagory.Parent_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", SubCatagory.Id);

                ViewBag.BrandId = new SelectList(db.Brands.Where(t => t.Catagory_Id == model.Catagory_Id).OrderBy(t => t.Name).ToList(), "Id", "Name", model.Brand_Id);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /InvProduct/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /InvProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region Helper
    

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
