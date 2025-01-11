using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Models;

public class ProductsController : Controller
{
    private readonly ShopDbContext _context;

    public ProductsController(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Include(p => p.Department).ToListAsync();
        return View(products);
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(string productName, decimal price, int departmentId)
    {
        if (string.IsNullOrEmpty(productName) || price <= 0 || departmentId <= 0)
        {
            ViewData["Error"] = "Все поля должны быть заполнены корректно.";
            return View();
        }

        var product = new Product
        {
            Name = productName,
            Price = price,
            DepartmentId = departmentId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ProductCount(int departmentId)
    {
        var productCount = await _context.Products
            .Where(p => p.DepartmentId == departmentId)
            .CountAsync();

        ViewData["ProductCount"] = productCount;
        return View();
    }

    public async Task<IActionResult> ProductsByDepartment(int departmentId)
    {
        var products = await _context.Products
            .Where(p => p.DepartmentId == departmentId)
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> CheckProductExists(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            ViewData["Error"] = "Товар не найден.";
        }
        else
        {
            ViewData["Message"] = $"Товар найден: {product.Name}, Цена: {product.Price}";
        }

        return View();
    }
}

