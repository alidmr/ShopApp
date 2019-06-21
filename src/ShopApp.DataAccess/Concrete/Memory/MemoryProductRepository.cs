using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.Memory
{
    public class MemoryProductRepository //: IProductRepository
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetOne(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            var products = new List<Product>()
            {
                new Product(){Id = 1,Name = "Samsung 1",ImageUrl = "1.jpg",Price = 1000},
                new Product(){Id = 2,Name = "Samsung 2",ImageUrl = "1.jpg",Price = 2000},
                new Product(){Id = 3,Name = "Samsung 3",ImageUrl = "1.jpg",Price = 3000},
            };
            return products.ToList();
        }

        public void Add(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetPopularProducts()
        {
            throw new NotImplementedException();
        }

        public Product GetProductDetails(int id)
        {
            throw new NotImplementedException();
        }
    }
}
