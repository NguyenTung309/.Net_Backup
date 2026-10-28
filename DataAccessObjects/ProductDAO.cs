using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ProductDAO
    {
        private readonly MyStoreContext _context = new MyStoreContext();

        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using var db = new MyStoreContext();
                listProducts = db.Products.Include(f => f.Category).ToList();
            }
            catch (Exception e) { }
            return listProducts;
        }

        public static Product? GetProductById(int id)
        {
            using var db = new MyStoreContext();
            return db.Products.FirstOrDefault(c => c.ProductId.Equals(id));
        }

        public static void SaveProduct(Product p)
        {
            try
            {
                using var context = new MyStoreContext();
                context.Products.Add(p);
                context.SaveChanges();
            }
            catch (Exception e) 
            { 
                throw new Exception(e.Message);
            }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using var context = new MyStoreContext();
                context.Entry<Product>(p).State
                = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public static void DeleteProduct(Product p)
        {
            try
            {
                using var context = new MyStoreContext();
                var p1 = context.Products.SingleOrDefault(c => c.ProductId == p.ProductId);
                context.Products.Remove(p1);
                context.SaveChanges();
            }
            catch (Exception e) 
            {
            throw new Exception(e.Message);
            }
        }

        public static List<Product> SearchProducts(string keyword)
        {
            var listProducts = new List<Product>();
            try
            {
                using var db = new MyStoreContext();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    // Nếu người dùng không nhập từ khóa, trả về toàn bộ sản phẩm
                    listProducts = db.Products.Include(p => p.Category).ToList();
                }
                else
                {
                    // Chuyển keyword về dạng thường để tránh phân biệt hoa/thường
                    keyword = keyword.Trim().ToLower();

                    listProducts = db.Products
                        .Include(p => p.Category)
                        .Where(p =>
                            p.ProductName.ToLower().Contains(keyword) || // tìm theo tên
                            (p.Category != null && p.Category.CategoryName.ToLower().Contains(keyword)) // tìm theo tên danh mục
                        )
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return listProducts;
        }
    }
}