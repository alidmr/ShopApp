using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Context;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ShopContext db) : base(db)
        {
        }
    }
}
