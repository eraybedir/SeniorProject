using MarketAPI.MarketAPI.Models;

namespace MarketAPI.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<object>> GetProductsAsync();
        Task<int> AddOrUpdateProductsAsync(List<ProductInputModel> products);
    }
}
