using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService
    {
        Category GetById(int id);
        List<Category> GetAll();
        void Add(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
    }
}
