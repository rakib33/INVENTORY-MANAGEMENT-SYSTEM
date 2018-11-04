using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{

    //https://stackoverflow.com/questions/40527349/pass-a-list-of-objects-to-mvc-action-method-using-angularjs-post
    public class AngularTestController : Controller
    {
        //
        // GET: /AngularTest/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
	}
}