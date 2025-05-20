using MarketAPI.Data;
using MarketAPI.Interfaces;
using MarketAPI.MarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public ProductService(IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<IEnumerable<object>> GetProductsAsync()
        {
            var products = await _productRepository.GetAllWithCategoryAsync();

            return products.Select(p => new
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
                Category = p.Category?.Name,
                p.CreatedAt
            });
        }

        public async Task<int> AddOrUpdateProductsAsync(List<ProductInputModel> products)
        {
            var productEntities = new List<Product>();

            foreach (var item in products)
            {
                // Kategori kontrolü
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == item.CategoryName.ToLower());

                if (category == null)
                {
                    category = new Category { Name = item.CategoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                }

                // Var olan ürünü getir
                var existingProduct = await _productRepository.GetByNameAndMarketAsync(item.Name, item.Market);

                if (existingProduct != null)
                {
                    existingProduct.ImageUrl = item.ImageUrl;
                    existingProduct.CaloriesPer100g = item.CaloriesPer100g;
                    existingProduct.ProteinPer100g = item.ProteinPer100g;
                    existingProduct.CarbsPer100g = item.CarbsPer100g;
                    existingProduct.FatPer100g = item.FatPer100g;
                    existingProduct.CategoryId = category.Id;

                    _productRepository.Update(existingProduct);
                    continue;
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
                    ImageUrl = item.ImageUrl,
                    CreatedAt = DateTime.Now
                };

                await _productRepository.AddAsync(newProduct);
            }

            await _productRepository.SaveChangesAsync();
            return products.Count;
        }
    }
}
