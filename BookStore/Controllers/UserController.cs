using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BookStore.App_Code;
using BookStore.Models;
using BookStore.Models.Inventory;
using BookStore.Models.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using BookStore.Repository;
using System.Data.Entity.Validation;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private IImage ImageRepository = null;

        public UserController()
        {
            ImageRepository = new ImageRepository();
        }

        public UserRoleManager newUser = new UserRoleManager();
        private ApplicationDbContext db = new ApplicationDbContext();
        //[Route("Supplier")]
        public ActionResult Supplier()
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                var supplierList = db.Users.Where(t => t.UserType == ConstantMessage.UserTypeSupplier).ToList();

                ViewBag.type = ConstantMessage.UserTypeSupplier;

                return View(supplierList);
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
        }

       
        [HttpGet]
        public ActionResult Customer()
        {
            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                //var CustomerList = db.Users.Where(t => t.UserType == ConstantMessage.UserTypeCustomer).ToList();

                ViewBag.type = ConstantMessage.UserTypeSupplier;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
        }

     
        [HttpGet]
        public ActionResult Employee()
        {

            try
            {
                ViewBag.message = Convert.ToString(TempData["message"]);
                var EmployeeList = db.Users.Where(t => t.UserType == ConstantMessage.UserTypeEmployee).ToList();

                ViewBag.type = ConstantMessage.UserTypeSupplier;

                return View(EmployeeList);
            }
            catch (Exception)
            {
                return RedirectToAction("notFound", "Error");
            }
        }

        [HttpGet]
        public ActionResult Create(string type)
        {
            ViewBag.Type = type;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model, HttpPostedFileBase file)
        {
            model.UserType =Convert.ToString(TempData["UserType"]);

            if (model.UserType !=null && !string.IsNullOrEmpty(model.UserType))
            {

                if (model.UserType != ConstantMessage.UserTypeEmployee)
                    model.DateOfBirth = null;

                var user = new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    BloodGroup = model.BloodGroup,
                    UserType = model.UserType,
                    Country = model.Country,

                    DateOfBirth = model.DateOfBirth,

                    FatherName = model.FatherName,
                    MotherName = model.MotherName,
                    Description = model.Description,
                    NID = model.NID,
                    PermanentAddress = model.PermanentAddress,
                    PresentAddress = model.PresentAddress,
                    Salary=model.Salary,
                    ReferenceInfo= model.ReferenceInfo,
                    Phone= model.Phone, 
   
                    UserName =   model.UserName,  //CustomCode.GenarateUserName(model.Name),  
                    
                    CreatedDate = DateTime.Now,

                    CreatedBy = User.Identity.Name,

                    JoiningDate = DateTime.Now,
                    ExpairedDate = DateTime.Now,
                    
                    
                    AllAccess = false,
                    IsActive = model.IsActive

                };

                UserImage ImageObj = new UserImage();
                if (file != null)
                {
                   ImageObj = ImageRepository.SetImage(file);
                }
                try
                {
                    if (file != null && ImageObj ==null)
                    {
                        ModelState.AddModelError("", "Uploaded Image get Null Value.");
                    }                    
                    else
                    {
                        var result = newUser.SaveUser(user, ImageObj, User.Identity.Name);               //newUser.UserManager.Create(user, model.Password);
                        if (result !=null)  // if (result.Succeeded)
                        {                       
    

                            if (model.UserType == ConstantMessage.UserTypeSupplier)
                                return RedirectToAction("Supplier");
                            if (model.UserType == ConstantMessage.UserTypeCustomer)
                                return RedirectToAction("Customer");
                            if (model.UserType == ConstantMessage.UserTypeEmployee)
                                return RedirectToAction("Employee");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to create user due to internal error.");
                            //AddErrors(result);
                        }
                    }
                }

                catch (DbEntityValidationException ex)
                {
                    foreach (var failure in ex.EntityValidationErrors)
                    {
                       // string validationErrors = "";

                        foreach (var error in failure.ValidationErrors)
                        {
                            //validationErrors += error.PropertyName + "  " + error.ErrorMessage;
                            ModelState.AddModelError("", error.PropertyName + "  " + error.ErrorMessage);
                        }
                    }
                }
                
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    
                }
               
            }

            ViewBag.Type = model.UserType;
            // If we got this far, something failed, redisplay form
            return View(model);
          
        }


        [HttpGet]
        public ActionResult Edit(string type,string id)
        {
            ViewBag.Type = type;
            try
            {

                ApplicationUser model = db.Users.Find(id);
                var user = new UserViewModel
                                {
                                    Id = model.Id,
                                    Name = model.Name,
                                    BloodGroup = model.BloodGroup,
                                    UserType = model.UserType,
                                    Country = model.Country,

                                    DateOfBirth = model.DateOfBirth,

                                    FatherName = model.FatherName,
                                    MotherName = model.MotherName,
                                    Description = model.Description,
                                    NID = model.NID,
                                    PermanentAddress = model.PermanentAddress,
                                    PresentAddress = model.PresentAddress,
                                    Salary = model.Salary.Value,
                                    ReferenceInfo = model.ReferenceInfo,
                                    Phone = model.Phone,
                                    UserName = model.UserName,

                                    CreatedDate = model.CreatedDate.Value,
                                    CreatedBy = model.CreatedBy,

                                    JoiningDate = model.JoiningDate.Value,
                                    ExpairedDate = model.ExpairedDate.Value,

                                    IsActive = model.IsActive
                                };

                if (model == null)
                {
                    TempData["message"] = "<span class=\"font-red\">Records not Found.</span>";
                }
                else
                {
                  return View(user);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"font-red\">" + ex.Message + "</span>";
            }

            if (type == ConstantMessage.UserTypeSupplier)
                return RedirectToAction("Supplier");
            if (type == ConstantMessage.UserTypeCustomer)
                return RedirectToAction("Customer");
            if (type == ConstantMessage.UserTypeEmployee)
                return RedirectToAction("Employee");
            return RedirectToAction("Index");
           
     
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string UserType, UserViewModel model, HttpPostedFileBase file)
        {
            ViewBag.Type = UserType;
            if (model.UserType != null && !string.IsNullOrEmpty(model.UserType))  //ModelState.IsValid && 
            {

                if (model.UserType != ConstantMessage.UserTypeEmployee)
                    model.DateOfBirth = null;


                try
                {
                ApplicationUser user = db.Users.Find(model.Id);


                   user.Name = model.Name;
                   user.BloodGroup = model.BloodGroup;
                   user.UserType = UserType;
                   user.Country = model.Country;

                
                   user.DateOfBirth = model.DateOfBirth;

                   user.FatherName = model.FatherName;
                   user.MotherName = model.MotherName;
                   user.Description = model.Description;
                   user.NID = model.NID;
                   user.PermanentAddress = model.PermanentAddress;
                   user.PresentAddress = model.PresentAddress;
                   user.Salary = model.Salary;
                   user.ReferenceInfo = model.ReferenceInfo;
                   user.Phone = model.Phone;

                  // user.UserName = model.UserName;    //model.Name + DateTime.Now.ToString("mm-ss") + "@gmail.com";

                   user.IsActive = model.IsActive;
                   user.UpdateBy = User.Identity.Name;
                   user.UpdateDate = DateTime.Now;

                   UserImage ImageEdit = db.Images.Include(t => t.ApplicationUser).Where(t => t.FK_User_Id == model.Id).SingleOrDefault();
                   var ImageObj = new UserImage();
                   if (file != null)
                   {
                       ImageObj = ImageRepository.SetImage(file);
                   }
                   db.Entry(user).State = EntityState.Modified;

                   if (ImageEdit == null)
                   {
                       ImageObj.Id = Guid.NewGuid().ToString();

                       ImageObj.CreatedBy = User.Identity.Name;
                       ImageObj.CreatedDate = DateTime.Now;

                       ImageObj.FK_User_Id = user.Id;
                       ImageObj.ApplicationUser = user;

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

                       ImageObj.ApplicationUser = user;

                       db.Entry(ImageEdit).State = EntityState.Modified;
                   }


                   db.SaveChanges();
                
                        if (model.UserType == ConstantMessage.UserTypeSupplier)
                            return RedirectToAction("Supplier");
                        if (model.UserType == ConstantMessage.UserTypeCustomer)
                            return RedirectToAction("Customer");
                        if (model.UserType == ConstantMessage.UserTypeEmployee)
                            return RedirectToAction("Employee");
                  
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        // string validationErrors = "";

                        foreach (var error in failure.ValidationErrors)
                        {
                            //validationErrors += error.PropertyName + "  " + error.ErrorMessage;
                            ModelState.AddModelError("", error.PropertyName + "  " + error.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Type = model.UserType;
            // If we got this far, something failed, redisplay form
            return View(model);
          

        }


        [HttpGet]
        public ActionResult Details(string id)
        {
            //string type = Convert.ToString(TempData["type"]);

            var details = db.Users.Find(id);
            ViewBag.Type = details.UserType;
           
            return View(details);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
        public ActionResult LoadData(string UserType) //, string UserId, decimal? FromDate, decimal? ToDate
        {
            //get Start (paging start index) and length (page size for paging)


            //DateTime _FromDate;
            //DateTime _ToDate;

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value           
            //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            //if (FromDate != null )
            //_FromDate = Convert.ToDateTime(FromDate);

            //if (ToDate != null)
            //_ToDate = Convert.ToDateTime(ToDate);

            var v = ProcedureFunctionCalled.GetUserInfo(UserType);

            if (v == null)
                totalRecords = 0;
            else
                totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList(); //
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

     
	}
    
}