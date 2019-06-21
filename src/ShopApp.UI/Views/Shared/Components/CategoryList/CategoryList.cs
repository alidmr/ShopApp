using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.UI.Models.Category;

namespace ShopApp.UI.Views.Shared.Components.CategoryList
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            return View(new CategoryListViewModel()
            {
                Category = _categoryService.GetAll(),
                SelectedCategory = RouteData.Values["category"]?.ToString()
            });
        }
    }
}
