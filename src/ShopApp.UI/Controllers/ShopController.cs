using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.UI.Models.Product;

namespace ShopApp.UI.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProductDetails(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailViewModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(x => x.Category).ToList()
            });
        }
        public IActionResult List(string category, int page = 1)
        {
            const int pageSize = 3;
            var model = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category, page, pageSize)
            };
            return View(model);
        }
    }
}