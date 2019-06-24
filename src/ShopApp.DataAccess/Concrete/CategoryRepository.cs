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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopContext db) : base(db)
        {
        }

        public Category GetByIdWithProducts(int id)
        {
            return Db.Categories
                .Where(x => x.Id == id)
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Product)
                .FirstOrDefault();
        }

        public void DeleteFromCategory(int categoryId, int productId)
        {
            var cmd = @"delete from ProductCategory where ProductId=@p0 and CategoryId=@p1";
            Db.Database.ExecuteSqlCommand(cmd, productId, categoryId);

        }
    }
}
