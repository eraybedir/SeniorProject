namespace MarketAPI.MarketAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Market { get; set; }
        public string Price { get; set; }

        public double? CaloriesPer100g { get; set; }
        public double? ProteinPer100g { get; set; }
        public double? CarbsPer100g { get; set;}
        public double? FatPer100g { get; set; }

        //foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
