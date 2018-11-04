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
    public class InvCatagoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /InvCatagory/
        public ActionResult Index()
        {
            try
            {
                ViewBag.message =Convert.ToString(TempData["message"]);
                return View(db.Catagories.Where(t=>t.Parent_Id == "0").OrderBy(t=>t.Name).ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
        }


        public ActionResult SubCatagory(string id)
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                List<Catagory> catList = db.Catagories.Where(t => t.Parent_Id == id).OrderBy(t => t.Name).ToList();
                ViewBag.parentId = id;
                return View("Index",catList);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("Index", "InvCatagory");
            }
        }

        // GET: /InvCatagory/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("notFound", "Error");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagory catagory = db.Catagories.Find(id);
            if (catagory == null)
            {
                return RedirectToAction("notFound", "Error");
               // return HttpNotFound();
            }
            return View(catagory);
        }

        // GET: /InvCatagory/Create
        public ActionResult Create()
        {
            try
            {
                //get all parent catagory     
                List<SelectListItem> selectList = null;
                selectList = (from s in db.Catagories.Where(t=>t.Parent_Id=="0")                              
                              select new SelectListItem
                                {
                                    //Selected = (s.id == model.Item.Item.Status),
                                    Text = s.Name,
                                    Value = s.Id
                                }).ToList();

                var newItem = new SelectListItem { Text = "Main Catagory", Value = "0", Selected = true };
                selectList.Add(newItem);

                ViewBag.ParentCatagory = selectList;
                
                return View();
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">"+ ex.Message +"</span>";
            }

            return RedirectToAction("Index");
        }

        // POST: /InvCatagory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Catagory catagory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    catagory.Id = Guid.NewGuid().ToString();
                    catagory.CreatedBy = User.Identity.Name;
                    catagory.CreatedDate = DateTime.Now;

                    db.Catagories.Add(catagory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Can not save due to- " + ex.Message);
                }
            }

            return View(catagory);
        }

        // GET: /InvCatagory/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                TempData["message"] = "<span class=\"font-red\">Records not Found.</span>";
                return RedirectToAction("Index");
            }
           
            try
            {

                Catagory model = db.Catagories.Find(id);
                if (model == null)
                {
                    TempData["message"] = "<span class=\"font-red\">Records not Found.</span>";
                }
                else
                {
                    List<SelectListItem> selectList = null;
                    selectList = (from s in db.Catagories.Where(t => t.Parent_Id == "0")
                                  select new SelectListItem
                                  {                                     
                                      Text = s.Name,
                                      Value = s.Id,
                                      Selected=(s.Id== model.Id)
                                  }).ToList();

                    var newItem = new SelectListItem { Text = "Main Catagory", Value = "0"};
                    selectList.Add(newItem);

                    ViewBag.ParentCatagory = selectList; 
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
        }

        // POST: /InvCatagory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Catagory catagory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    catagory.UpdateDate = DateTime.Now;
                    catagory.UpdateBy = User.Identity.Name;

                    db.Entry(catagory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
                   // ModelState.AddModelError("", "Can not save edited data due to- " + ex.Message);
                }
            }

            return RedirectToAction("Index");
           // return View(catagory);
        }

        // GET: /InvCatagory/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagory catagory = db.Catagories.Find(id);
            if (catagory == null)
            {
                return HttpNotFound();
            }
            return View(catagory);
        }

        // POST: /InvCatagory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Catagory catagory = db.Catagories.Find(id);
            db.Catagories.Remove(catagory);
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
