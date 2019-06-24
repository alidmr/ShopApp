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
            var model = new ProductListViewModel()
            {
                Products = _productService.GetAll()
            };
            return View(model);
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
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = _productService.GetById((int)id);
            if (result==null)
            {
                return NotFound();
            }
            var model = new ProductViewModel()
            {
                Id = result.Id,
                Name = result.Name,
                Price = result.Price,
                Description = result.Description,
                ImageUrl = result.ImageUrl
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            var entity = _productService.GetById(model.Id);
            if (entity==null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.ImageUrl = model.ImageUrl;
            entity.Price = model.Price;
            _productService.Update(entity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            return RedirectToAction("Index");
        }
    }
}