using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;


using System.Data.Entity;
using BookStore.Models;
using BookStore.Models.Infrastructure;

namespace BookStore.App_Code
{
    public class UserRoleManager
    {
         public UserRoleManager()
      :  this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

      public UserRoleManager(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;

            //To allow email as UserName for avoid error: User name ***@gmail.com is invalid, can only contain letters or digits.
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

         public UserRoleManager(string a)
            : this(new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext())))
        {
        }

        public UserRoleManager(RoleManager<ApplicationRole> roleManager)
        {
            RoleManager = roleManager;
        }


        public UserManager<ApplicationUser> UserManager { get; set; } //private

        public RoleManager<ApplicationRole> RoleManager { get; private set; }

        public ApplicationUser SaveUser(ApplicationUser user,UserImage UserImage,string CreatedBy) 
        {

            UserImage.Id = Guid.NewGuid().ToString();
            UserImage.CreatedBy = CreatedBy;
            UserImage.CreatedDate = DateTime.Now;
            UserImage.FK_User_Id = user.Id;
            UserImage.ApplicationUser = user;

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Users.Add(user);
                    db.Images.Add(UserImage);
                    db.SaveChanges();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


       

    }
}