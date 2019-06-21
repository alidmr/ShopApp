using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ShopApp.DataAccess.Abstract
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T GetById(int id);
        T GetOne(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int SaveChanges();
    }
}
