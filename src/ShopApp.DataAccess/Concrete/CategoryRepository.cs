using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
    }
}
