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
                    // Eğer yoksa yeni bir kategori oluştur 
                    category = new Category { Name = item.CategoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync(); // ID üretmesi için
                }

                // Aynı isim ve markette ürün zaten var mı?
                var existingProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == item.Name.ToLower()
                                           && p.Market.ToLower() == item.Market.ToLower());

                if (existingProduct != null)
                {
                    // Var olan ürünü güncelle
                    existingProduct.ImageUrl = item.ImageUrl;
                    existingProduct.CaloriesPer100g = item.CaloriesPer100g;
                    existingProduct.ProteinPer100g = item.ProteinPer100g;
                    existingProduct.CarbsPer100g = item.CarbsPer100g;
                    existingProduct.FatPer100g = item.FatPer100g;
                    existingProduct.CategoryId = category.Id;
                    continue;
                }

                // Yeni ürün ekle
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
                    ImageUrl = item.ImageUrl,
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
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ImageUrl != null && p.CaloriesPer100g != null) // 🔍 filtreleme
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Market,
                    p.Price,
                    p.CaloriesPer100g,
                    p.ProteinPer100g,
                    p.CarbsPer100g,
                    p.FatPer100g,
                    p.ImageUrl,
                    Category = p.Category.Name,
                    p.CreatedAt
                })
                .ToListAsync();

            return Ok(products);
        }

    }
}
