using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.Business.Concrete
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public void InitializeCart(string userId)
        {
            _cartRepository.Add(new Cart() { UserId = userId });
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                var index = cart.CartItems.FindIndex(x => x.ProductId == productId);
                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id,
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }
                _cartRepository.Update(cart);
            }
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart !=null)
            {
                _cartRepository.DeleteFromCart(cart.Id, productId);
            }
        }

        public void ClearCart(int cartId)
        {
            _cartRepository.ClearCart(cartId);
        }
    }
}
