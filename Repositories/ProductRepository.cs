using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _productDAO = new ProductDAO();

        public List<Product> GetProducts() => ProductDAO.GetProducts();
        public Product? GetProductById(int id) => ProductDAO.GetProductById(id);
        public void SaveProduct(Product p) => ProductDAO.SaveProduct(p);
        public void UpdateProduct(Product p) => ProductDAO.UpdateProduct(p);
        public void DeleteProduct(Product p) => ProductDAO.DeleteProduct(p);
        //public List<Product> SearchProducts(string keyword) => ProductDAO.SearchProducts(keyword);
    }
}
