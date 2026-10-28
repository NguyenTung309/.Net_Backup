using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProductRepository
    {
        void SaveProduct(Product p);
        void UpdateProduct(Product p);
        void DeleteProduct(Product p);
        List<Product> GetProducts();
        Product GetProductById(int id);
    }
}
