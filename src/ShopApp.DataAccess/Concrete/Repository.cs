using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Context;

namespace ShopApp.DataAccess.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ShopContext Db;
        private readonly DbSet<T> _dbSet;
        public Repository(ShopContext db)
        {
            Db = db;
            _dbSet = Db.Set<T>();
        }
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual T GetOne(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter).SingleOrDefault();
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? _dbSet.ToList() : _dbSet.Where(filter).ToList();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
            SaveChanges();
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
