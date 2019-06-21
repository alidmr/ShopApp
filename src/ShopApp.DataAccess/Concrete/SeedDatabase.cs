using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Context;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete
{
    public static class SeedDatabase
    {
        //public static void Seed()
        //{
        //    var db = new ShopContext();
        //    if (db.Database.GetPendingMigrations().Count() == 0)
        //    {
        //        if (db.Categories.Count() == 0)
        //        {
        //            db.Categories.AddRange(Categories);
        //        }

        //        if (db.Products.Count() == 0)
        //        {
        //            db.Products.AddRange(Products);
        //        }

        //        db.SaveChanges();
        //    }
        //}

        private static Category[] Categories =
        {
            new Category() {Name = "Telefon"},
            new Category() {Name = "Bilgisayar"}
        };

        private static Product[] Products =
        {
            new Product() {Name = "Samsung s5", Price = 2000, ImageUrl = "1.jpg"},
            new Product() {Name = "Samsung s6", Price = 3000, ImageUrl = "2.jpg"},
            new Product() {Name = "Samsung s7", Price = 4000, ImageUrl = "3.jpg"},
            new Product() {Name = "Samsung s8", Price = 5000, ImageUrl = "4.jpg"},
            new Product() {Name = "Samsung s9", Price = 6000, ImageUrl = "5.jpg"},
            new Product() {Name = "Samsung s10", Price = 7000, ImageUrl = "6.jpg"},
            new Product() {Name = "İphone 5s", Price = 8000, ImageUrl = "7.jpg"},
            new Product() {Name = "İphone 6s", Price = 9000, ImageUrl = "8.jpg"},
            new Product() {Name = "İphone 7s", Price = 4000, ImageUrl = "9.jpg"},
            new Product() {Name = "İphone 8s", Price = 5000, ImageUrl = "1.jpg"}
        };
    }
}
