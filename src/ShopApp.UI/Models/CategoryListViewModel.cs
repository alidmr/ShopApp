using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Entities;

namespace ShopApp.UI.Models
{
    public class CategoryListViewModel
    {
        public List<Category> Category { get; set; }
        public string SelectedCategory { get; set; }
    }
}
