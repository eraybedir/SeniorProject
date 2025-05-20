using Microsoft.AspNetCore.Mvc;
using MarketAPI.Interfaces;
using MarketAPI.MarketAPI.Models;

namespace MarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> PostProducts([FromBody] List<ProductInputModel> products)
        {
            if (products == null || !products.Any())
                return BadRequest("Veri listesi boş.");

            int count = await _productService.AddOrUpdateProductsAsync(products);
            return Ok(new { message = "Ürünler başarıyla işlendi.", count });
        }
    }
}
