using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MarketAPI.Data;
using MarketAPI.MarketAPI.Models;

namespace MarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> PostProducts([FromBody] List<ProductInputModel> products)
        {
            if (products == null || !products.Any())
                return BadRequest("Veri listesi boş.");

            var productEntities = new List<Product>();

            foreach (var item in products)
            {
                // CategoryName'e göre CategoryId bul
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == item.CategoryName.ToLower());

                if (category == null)
                {
                    // Eğer yoksa yeni bir kategori oluştur (istersen)
                    category = new Category { Name = item.CategoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync(); // ID üretmesi için
                }

                var newProduct = new Product
                {
                    Name = item.Name,
                    Market = item.Market,
                    Price = item.Price,
                    CaloriesPer100g = item.CaloriesPer100g,
                    ProteinPer100g = item.ProteinPer100g,
                    CarbsPer100g = item.CarbsPer100g,
                    FatPer100g = item.FatPer100g,
                    CategoryId = category.Id,
                    CreatedAt = DateTime.Now
                };

                productEntities.Add(newProduct);
            }

            await _context.Products.AddRangeAsync(productEntities);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ürünler başarıyla kaydedildi.", count = productEntities.Count });
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
    }
}
