using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Context;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ShopContext db) : base(db)
        {
        }

        public IEnumerable<Product> GetPopularProducts()
        {
            throw new NotImplementedException();
        }

        public Product GetProductDetails(int id)
        {
            return Db.Products.Where(x => x.Id == id)
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                .FirstOrDefault();
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            var products = Db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Include(x => x.ProductCategories)
                    .ThenInclude(x => x.Category)
                    .Where(x => x.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
            }

            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetCountByCategory(string category)
        {
            var products = Db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Include(x => x.ProductCategories)
                    .ThenInclude(x => x.Category)
                    .Where(x => x.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
            }

            return products.Count();
        }

        public Product GetByIdWithCategories(int id)
        {
            return Db.Products
                .Where(x => x.Id == id)
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                .FirstOrDefault();
        }

        public void Update(Product entity, int[] categoryIds)
        {
            var product = Db.Products
                .Include(x => x.ProductCategories)
                .FirstOrDefault(x => x.Id == entity.Id);

            if (product == null) return;

            product.Name = entity.Name;
            product.Description = entity.Description;
            product.ImageUrl = entity.ImageUrl;
            product.Price = entity.Price;

            product.ProductCategories = categoryIds.Select(catId => new ProductCategory()
            {
                CategoryId = catId,
                ProductId = entity.Id
            }).ToList();

            Db.SaveChanges();
        }
    }
}
