using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Context
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=monster;Database=ShopDb;Integrated Security=true;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(x => new { x.CategoryId, x.ProductId });  //tablonun primary keyini iki id içinde vermek için bu yapılır.
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
