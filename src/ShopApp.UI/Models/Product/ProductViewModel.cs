using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.UI.Models.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        //[Required,StringLength(60,ErrorMessage = "Ürün adı max. 60 karakter olmalıdır."),MinLength(5)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required, StringLength(100000, ErrorMessage = "Ürün açıklaması min. 20 karakter max. 100 karakter olmalıdır."), MinLength(20)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat belirtiniz.")]
        [Range(1,10000)]
        public decimal? Price { get; set; }

        public List<Entities.Category> SelectedCategories { get; set; }
    }
}
