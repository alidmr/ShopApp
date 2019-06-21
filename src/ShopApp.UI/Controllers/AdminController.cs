using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.UI.Models.Product;

namespace ShopApp.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductViewModel model)
        {
            var entity = new Product()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl
            };
            _productService.Add(entity);
            return Redirect("Index");
        }
    }
}