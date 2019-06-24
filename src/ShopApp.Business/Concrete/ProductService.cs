using System.Collections.Generic;
using System.Linq;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.Business.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetProductDetails(int id)
        {
            return _productRepository.GetProductDetails(id);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll().ToList();
        }

        public List<Product> GetPopularProducts()
        {
            return _productRepository.GetAll(x => x.Price > 2000).ToList();
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(category, page, pageSize);
        }

        public bool Add(Product entity)
        {
            if (Validate(entity))
            {
                _productRepository.Add(entity);
                return true;
            }

            return false;
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public void Update(Product entity, int[] categoryIds)
        {
            _productRepository.Update(entity, categoryIds);
        }

        public string ErrorMessage { get; set; }
        public bool Validate(Product entity)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Ürün ismi girmelisiniz...";
                isValid = false;
            }

            return isValid;
        }
    }
}
