﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.UI.Models.Cart;

namespace ShopApp.UI.Models.Order
{
    public class OrderViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvv { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public CartViewModel CartViewModel { get; set; }
    }
}
