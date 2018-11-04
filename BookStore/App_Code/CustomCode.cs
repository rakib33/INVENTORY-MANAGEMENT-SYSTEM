using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using BookStore.Models.Infrastructure;

namespace BookStore.App_Code
{
    public class CustomCode
    {
        public static string GeneratenewRandom(string Prev)
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                GeneratenewRandom(Prev);
            }
            return Prev+r+DateTime.Now.ToString("ss");
        }

        public static List<SelectListItem> TransactionType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem
            //{
            //    Text = ConstantMessage.InvoiceTypeSalary,
            //    Value = ConstantMessage.InvoiceTypeSalary
            //});

            items.Add(new SelectListItem
            {
                Text = ConstantMessage.InvoiceTypeProfitExpense,
                Value = ConstantMessage.InvoiceTypeProfitExpense
                //Selected = true
            });

            //items.Add(new SelectListItem
            //{
            //    Text = ConstantMessage.InvoiceTypeExpense,
            //    Value = ConstantMessage.InvoiceTypeExpense
            //});          

            return items;
        }



        public static List<SelectListItem> TransactionTypeList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = ConstantMessage.InvoiceTypeSalary,
                Value = ConstantMessage.InvoiceTypeSalary
            });

            items.Add(new SelectListItem
            {
                Text = ConstantMessage.InvoiceTypeProfitExpense,
                Value = ConstantMessage.InvoiceTypeProfitExpense
                //Selected = true
            });         

            items.Add(new SelectListItem
            {
                Text = ConstantMessage.InvoiceTypePurchase,
                Value = ConstantMessage.InvoiceTypePurchase
            });

            items.Add(new SelectListItem
            {
                Text = ConstantMessage.InvoiceTypeSale,
                Value = ConstantMessage.InvoiceTypeSale
            });

            return items;
        }


        public static List<SelectListItem> StatusList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = ConstantMessage.StatusApproved,
                Value = ConstantMessage.StatusApproved
            });

            //items.Add(new SelectListItem
            //{
            //    Text = ConstantMessage.StatusPending,
            //    Value = ConstantMessage.StatusPending,
            //    Selected = true
            //});

            items.Add(new SelectListItem
            {
                Text = ConstantMessage.StatusReject,
                Value = ConstantMessage.StatusReject
            });

            return items;
        }

        public static List<SelectListItem> SaleStatusList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = ConstantMessage.StatusApproved,
                Value = ConstantMessage.StatusApproved
            });

            //items.Add(new SelectListItem
            //{
            //    Text = ConstantMessage.StatusPending,
            //    Value = ConstantMessage.StatusPending,
            //    Selected = true
            //});

            items.Add(new SelectListItem
            {
                Text = ConstantMessage.StatusReject,
                Value = ConstantMessage.StatusReject
            });

            return items;
        }
        public static string GenarateUserName(string Name)
        {
            var _Name=Name;
            if (Name.Length > 4)
            {
                //Client code will be 4 digit Uper Case latter
                _Name = Name.Substring(0, 4);
            }

            return _Name.ToUpper()+DateTime.Now.ToString("mm-ss")+"@gmail.com";

        }


        //public string getUniqueCharacter(string text)
        //{

        //    if (text.Length > 5)
        //        var.CodeNumber = text.Substring(0, 5);

        //    // now make it unique by adding dateTime
        //    var.CodeNumber = var.CodeNumber + DateTime.Now.ToString("yyyyMMddHHmmss");

        //    return var.CodeNumber;


        //}

    }
}