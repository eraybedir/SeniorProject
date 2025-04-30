namespace MarketAPI.MarketAPI.Models
{
    public class ProductInputModel
    {
        public string Name { get; set; }
        public string Market { get; set; }
        public string Price { get; set; }

        public double? CaloriesPer100g { get; set; }
        public double? ProteinPer100g { get; set; }
        public double? CarbsPer100g { get; set; }
        public double? FatPer100g { get; set; }

        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }

    }

}
