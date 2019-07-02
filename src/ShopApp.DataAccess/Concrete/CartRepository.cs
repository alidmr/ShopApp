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
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ShopContext db) : base(db)
        {
        }

        public Cart GetByUserId(string userId)
        {
            var result = Db.Carts
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.UserId == userId);

            return result;
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            var cmd = @"delete from CartItems where CartId = @p0 and ProductId = @p1";
            Db.Database.ExecuteSqlCommand(cmd, cartId, productId);
        }

        public void ClearCart(int cartId)
        {
            var cmd = @"delete from CartItems where CartId=@p0";
            Db.Database.ExecuteSqlCommand(cmd, cartId);
        }

        public override void Update(Cart entity)
        {
            Db.Carts.Update(entity);
            Db.SaveChanges();
        }
    }
}
