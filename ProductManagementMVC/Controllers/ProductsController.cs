using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Services;

namespace ProductManagementMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _contextProduct;
        private readonly ICategoryService _contextCategory;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService contextProduct, ICategoryService contextCategory, ILogger<ProductsController> logger)
        {
            _contextProduct = contextProduct;
            _contextCategory = contextCategory;
            _logger = logger;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Accessing Session data
            string userId = HttpContext.Session.GetString("UserId");
            string username = HttpContext.Session.GetString("Username");

            // You can use session data as needed
            ViewBag.Username = username;
            ViewBag.UserId = userId;

            var myStoreContext = _contextProduct.GetProducts();
            return View(myStoreContext.ToList());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId,UnitsInStock,UnitPrice")] Product product)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation("Create product POST method called");
            _logger.LogInformation("Product data - Name: {Name}, CategoryId: {CategoryId}, Stock: {Stock}, Price: {Price}",
                product.ProductName, product.CategoryId, product.UnitsInStock, product.UnitPrice);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid, attempting to save product");

                try
                {
                    _contextProduct.SaveProduct(product);
                    _logger.LogInformation("Product saved successfully, redirecting to Index");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating product");
                    ModelState.AddModelError("", "Error creating product. Please try again.");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId,UnitsInStock,UnitPrice")] Product product)
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contextProduct.UpdateProduct(product);
                }
                catch (Exception)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _contextProduct.GetProductById(id);
            if (product != null)
            {
                _contextProduct.DeleteProduct(product);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            var tmp = _contextProduct.GetProductById(id);
            return (tmp != null) ? true : false;
        }

        // === THÊM CHỨC NĂNG VIEW Ở ĐÂY ===

        // GET: Products/View
        public IActionResult ViewProducts()
        {
            // Session Access - Check if user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation("ViewProducts page accessed");

            var products = _contextProduct.GetProducts();

            // Calculate total units in stock - SỬA LỖI Ở ĐÂY
            int totalItems = 0;
            if (products != null && products.Any())
            {
                totalItems = (int)products.Sum(p => p.UnitsInStock);
            }
            ViewBag.TotalItems = totalItems;

            return View(products.ToList());
        }
    }
}