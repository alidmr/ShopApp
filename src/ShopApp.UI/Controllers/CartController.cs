using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IyzipayCore;
using IyzipayCore.Model;
using IyzipayCore.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.UI.Identity;
using ShopApp.UI.Models.Cart;
using ShopApp.UI.Models.Order;

namespace ShopApp.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            var model = new CartViewModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(x => new CartItemViewModel()
                {
                    CartItemId = x.Id,
                    Name = x.Product.Name,
                    Price = (decimal)x.Product.Price,
                    ImageUrl = x.Product.ImageUrl,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            _cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            _cartService.DeleteFromCart(_userManager.GetUserId(User), productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderViewModel();

            orderModel.CartViewModel = new CartViewModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(x => new CartItemViewModel()
                {
                    CartItemId = x.Id,
                    Name = x.Product.Name,
                    Price = (decimal)x.Product.Price,
                    ImageUrl = x.Product.ImageUrl,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            return View(orderModel);
        }
        [HttpPost]
        public IActionResult Checkout(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartViewModel = new CartViewModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(x => new CartItemViewModel()
                    {
                        CartItemId = x.Id,
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        Price = (decimal)x.Product.Price,
                        ImageUrl = x.Product.ImageUrl,
                        Quantity = x.Quantity
                    }).ToList()
                };

                //ödeme 
                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(cart.Id);

                    return View("Success");
                }
                //SaveOrder(model, payment, userId); //TODO:payment status olarak fail dönüyor deneme amaçlı buraya yazıldı. bu metodun if içierisinde olması gerekiyor.
                //ClearCart(cart.Id);  //TODO:payment status olarak fail dönüyor deneme amaçlı buraya yazıldı. bu metodun if içierisinde olması gerekiyor.
            }

            //sipariş
            return View(model);
        }

        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private void SaveOrder(OrderViewModel model, Payment payment, string userId)
        {
            var order = new Order();
            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.Completed;
            order.PaymentTypes = EnumPaymentTypes.CreditCart;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = DateTime.Now;
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.Email = model.Email;
            order.Phone = model.Phone;
            order.Address = model.Address;
            order.City = model.City;
            order.UserId = userId;

            foreach (var item in model.CartViewModel.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    OderId = 1
                };
                order.OrderItems.Add(orderItem);
            }

            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderViewModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-5W8uj6fGyOQFO5P7pJiDDRtmjgbq6bCT";
            options.SecretKey = "sandbox-bOjKcSelsm9w3otNNVqxeUtH9NEVBy3m";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = model.CartViewModel.TotalPrice().ToString().Split(",")[0];
            request.PaidPrice = model.CartViewModel.TotalPrice().ToString().Split(",")[0];
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = model.CartViewModel.CartId.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvv;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;


            //PaymentCard paymentCard = new PaymentCard();
            //paymentCard.CardHolderName = "John Doe";
            //paymentCard.CardNumber = "5528790000000008";
            //paymentCard.ExpireMonth = "12";
            //paymentCard.ExpireYear = "2030";
            //paymentCard.Cvc = "123";
            //paymentCard.RegisterCard = 0;
            //request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;
            foreach (var item in model.CartViewModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Telefon";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = item.Price.ToString().Split(",")[0];

                basketItems.Add(basketItem);
            }

            request.BasketItems = basketItems;

            var result = Payment.Create(request, options);

            return result;
        }

        public IActionResult GetOrders()
        {
            var orders = _orderService.GetOrders(_userManager.GetUserId(User));

            var orderListModel = new List<OrderListViewModel>();
            OrderListViewModel orderModel;

            foreach (var order in orders)
            {
                orderModel = new OrderListViewModel();
                orderModel.Id = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.OrderNote = order.OrderNote;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;

                orderModel.OrderItems = order.OrderItems.Select(x => new OrderItemModel()
                {
                    OrderItemId = x.Id,
                    Name = x.Product.Name,
                    ImageUrl = x.Product.ImageUrl,
                    Price = x.Price,
                    Quantity = x.Quantity
                }).ToList();
                orderListModel.Add(orderModel);
            }
            return View(orderListModel);
        }
    }
}