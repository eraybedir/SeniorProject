using MarketAPI.Data;
using MarketAPI.Interfaces;
using MarketAPI.MarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByNameAndMarketAsync(string name, string market)
        {
            return await _context.Products //Await asenkron çalışmasını sağlamak. Threadı bloklamadığı için, API'nin performansını arttırmak, Birden fazla request işlemek için kullanılır.
                .FirstOrDefaultAsync(p =>
                    p.Name.ToLower() == name.ToLower() &&
                    p.Market.ToLower() == market.ToLower());
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ImageUrl != null && p.CaloriesPer100g != null)
                .ToListAsync();
        }
    }
}
