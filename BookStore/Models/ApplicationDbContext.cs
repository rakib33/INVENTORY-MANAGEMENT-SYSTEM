using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using BookStore.Models.bookStore;
using BookStore.Models.Infrastructure;
using BookStore.Models.Inventory;
using System.Data.SqlClient;

namespace BookStore.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
         
        }

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

        public IDbSet<Author> Authors { get; set; }
        public IDbSet<UserImage> Images { get; set; }
        public IDbSet<Book> Books { get; set; }
        public IDbSet<Employee> Employees { get; set; }


        #region Inventory

        public IDbSet<Brand> Brands { get; set; }
        public IDbSet<Catagory> Catagories { get; set; }  
        public IDbSet<Product> Products { get; set; }
        public IDbSet<ProductDetail> ProductDetails { get; set; }
        public IDbSet<Purchase> Purchases { get; set; }
        public IDbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public IDbSet<Sale> Sales { get; set; }
        public IDbSet<SaleBlackCopy> SaleBlackCopies { get; set; }
        public IDbSet<Stock> Stocks { get; set; }
        public IDbSet<Invoice> Invoices { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }

        public IDbSet<DbBackUp> DbBackUps { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                       
            modelBuilder.Entity<UserImage>().HasKey(t => t.Id).ToTable("UserImage");
            modelBuilder.Entity<Author>().HasKey(t => t.Id).ToTable("Author");
            modelBuilder.Entity<Book>().HasKey(t => t.Id).ToTable("Book");
            modelBuilder.Entity<Employee>().HasKey(t => t.Id).ToTable("Employee");

            modelBuilder.Entity<Brand>().HasKey(t => t.Id).ToTable("Brand");
            modelBuilder.Entity<Catagory>().HasKey(t => t.Id).ToTable("Catagory");
            modelBuilder.Entity<Product>().HasKey(t => t.Id).ToTable("Product");
            modelBuilder.Entity<ProductDetail>().HasKey(t => t.Id).ToTable("ProductDetail");
            modelBuilder.Entity<Purchase>().HasKey(t => t.Id).ToTable("Purchase");
            modelBuilder.Entity<PurchaseReturn>().HasKey(t => t.Id).ToTable("PurchaseReturn");
            modelBuilder.Entity<Sale>().HasKey(t => t.Id).ToTable("Sale");
            modelBuilder.Entity<SaleBlackCopy>().HasKey(t => t.Id).ToTable("SaleBlackCopy");
            modelBuilder.Entity<Stock>().HasKey(t => t.Id).ToTable("Stock");
            modelBuilder.Entity<Invoice>().HasKey(t => t.Id).ToTable("Invoice");
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id).ToTable("Transaction");

            modelBuilder.Entity<DbBackUp>().HasKey(t => t.Id).ToTable("DbBackUp");

            base.OnModelCreating(modelBuilder);

            //For Identity version 1
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<IdentityUser>().ToTable("User");

            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");

            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            
        }

         

    }
}