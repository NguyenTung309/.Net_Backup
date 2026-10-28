using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository iProductRepository;
        public ProductService()
        {
            iProductRepository = new ProductRepository();
        }
        public void DeleteProduct(Product p)
        {
            iProductRepository.DeleteProduct(p);
        }
        public Product GetProductById(int id)
        {
            return iProductRepository.GetProductById(id);
        }
        public List<Product> GetProducts()
        {
            return iProductRepository.GetProducts();
        }

        public int GetTotalUnitsInStock()
        {
            try
            {
                var products = GetProducts();
                int total = products.Sum(p => p.UnitsInStock);
                _logger.LogInformation("Total units in stock calculated: {Total}", total);
                return total;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total units in stock");
                return 0;
            }
        }

        public void SaveProduct(Product p)
        {
            iProductRepository.SaveProduct(p);
        }
        public void UpdateProduct(Product p)
        {
            iProductRepository.UpdateProduct(p);
        }
    }
}
