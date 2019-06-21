using System.Collections.Generic;
using ShopApp.Entities;

namespace ShopApp.UI.Models.Product
{
    public class ProductDetailViewModel
    {
        public Entities.Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
