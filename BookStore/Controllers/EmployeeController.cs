using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookStore.Models.bookStore;
using BookStore.Models.Infrastructure;
using BookStore.Repository;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class EmployeeController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET api/employee  
        [ActionName("get"), HttpGet]
        public IEnumerable<Employee> Emps()
        {
            return db.Employees.ToList();
        }
        // GET api/employee/5  
        public Employee Get(int id)
        {
            return db.Employees.Find(id);
        }
        // POST api/employee  
        public HttpResponseMessage Post(Employee model)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(model);
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        // PUT api/employee/5  
        public HttpResponseMessage Put(Employee model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        // DELETE api/employee/5  
        public HttpResponseMessage Delete(int id)
        {
            Employee emp = db.Employees.Find(id);
            if (emp == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            db.Employees.Remove(emp);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, emp);
        }  
    }
}
