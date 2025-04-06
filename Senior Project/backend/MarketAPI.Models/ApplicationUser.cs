namespace MarketAPI.Models
{
    public enum UserGoal
    {
        KiloAlmak = 1,
        SporYapmak = 2,
        KiloVermek = 3
    }

    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "user";

        public int Age { get; set; }
        public double Weight { get; set; }  // kg
        public double Height { get; set; }  // cm

        public UserGoal Goal { get; set; }

        public double Budget { get; set; }  
    }
}
