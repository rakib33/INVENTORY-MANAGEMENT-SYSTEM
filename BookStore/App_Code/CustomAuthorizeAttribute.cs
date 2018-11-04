//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//using System.Web.Mvc;

//using System.Threading.Tasks;


//using BookStore.Models;
//using BookStore.Controllers;
//using System.Web.Routing;

////using System.Web.Http;

//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;


//namespace BookStore.App_Code
//{
  

    
//    public class CustomAuthorize:AuthorizeAttribute
//     {



//         private Variables variable = new Variables();

//         private CheckMsisdnUserStatus objMsisdnUserStatus = new CheckMsisdnUserStatus();

//         private IsSubscribedWithRemainingDate checkStatus;      

//         private AccountController objAccount = new AccountController();

      
//         public override void OnAuthorization(AuthorizationContext filterContext)  //
//        {



//             var rd = filterContext.HttpContext.Request.RequestContext.RouteData;
//             variable.ActionName = rd.GetRequiredString("action");
//             variable.ControllerName = rd.GetRequiredString("controller");

//             variable.Parameter = filterContext.HttpContext.Request["IsDevice"] as string;

//             if (variable.Parameter == null)
//             {
//                 variable.Parameter = rd.Values["IsDevice"] as string;
//             }

//             if (filterContext.HttpContext.User.Identity.IsAuthenticated)
//             {
//                 variable.UserPhoneNo = filterContext.HttpContext.User.Identity.Name;

//             }
//             else
//             {
//                 //get User No from Header 
//                 variable.UserPhoneNo = objMsisdnUserStatus.GetMSISDNHeader(filterContext);

//                // variable.UserPhoneNo = "8801865554868";
//             }

             

//         //   //check if user No is not found

//             if (variable.UserPhoneNo == null)
//             {
//                 //header not found redirect
//                 //if request does not come from device/apk redirect a page

//                 filterContext.Result = new RedirectToRouteResult(new
//                 RouteValueDictionary(new { controller = "Subscribe", action = "HeaderError", IsDevice = variable.Parameter }));

//             }
//         //        //if user exists
//             else
//             {
//                 //To Do check user is in Role

//                 //base.OnAuthorization(filterContext);

//                 //get this user status from SubscribeController at IsSubscribedWithRemainingDate(string userNo) method               

//                 checkStatus =objMsisdnUserStatus.checkUserStatus(variable.UserPhoneNo);

               

//                 //if status is subscribed statusId=1
//                 if (checkStatus.StatusID == ConstantValues.STATUS_SUBSCRIBE_ID)//1
//                 {
//                     //check is already authenticate then grant paermission if not try to login
//                     if (!filterContext.HttpContext.Request.IsAuthenticated)
//                     {
//                         //try to login 
//                         checkStatus.LoginResult = objAccount.DeviceAuthorizationLogin(variable.UserPhoneNo);
//                         //if login does not success

//                         if (checkStatus.LoginResult == ConstantValues.LOGIN_STATUS_FAILED_ID)  //0
//                         {
//                             //todo login error try again                            
//                             filterContext.Result = new RedirectToRouteResult(new
//                             RouteValueDictionary(new { controller = "Subscribe", action = "LoginFailed", IsDevice = variable.Parameter }));
//                         }
//                         else
//                         {
//                             //access granted nothing to do heare
//                         }
//                     }

//                 }

//                 else
//                 {
//                     //for Unsubscribe or New user do throw Subscribe page if request does not come from Device 

//                     if (variable.Parameter == ConstantValues.Device)
//                     {
//                         //request come from a Device/apk
//                         //Redirect Subscribe/DeviceSubscribe Action with parameter
//                         filterContext.Result = new RedirectToRouteResult(new
//                         RouteValueDictionary(new { controller = "Subscribe", action = "DeviceSubscribe", Option = checkStatus.StatusID }));
//                     }
//                     else
//                     {

//                         filterContext.Result = new RedirectToRouteResult(new
//                        RouteValueDictionary(new { controller = "Subscribe", action = "Subscribe", Option = checkStatus.StatusID }));
//                     }
//                 }

//             }
//         }

        

//    }

    
//}