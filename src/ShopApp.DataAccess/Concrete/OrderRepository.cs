using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
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

        public List<Order> GetOrders(string userId)
        {
            var orders = Db.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                orders = orders.Where(x => x.UserId == userId);
            }

            return orders.ToList();
        }
    }
}
