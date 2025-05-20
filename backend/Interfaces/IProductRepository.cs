using MarketAPI.MarketAPI.Models;

namespace MarketAPI.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByNameAndMarketAsync(string name,string market);
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();

    }

}