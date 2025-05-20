namespace MarketAPI.Models
{
    public class ProductInputModel
    {
        public required string Name { get; set; }
        public required string Market { get; set; }
        public required decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? StockQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Brand { get; set; }
        public string? Barcode { get; set; }
        public string? Unit { get; set; }
        public string? Weight { get; set; }
        public string? Dimensions { get; set; }
        public required string CategoryName { get; set; }
    }
}
