namespace MarketAPI.Models
{
    public enum UserGoal
    {
        gainingWeight = 1,
        doingSports = 2,
        loosingWeight = 3,
        beinghealthy = 4
    }

    public enum ActivityLevel
    {
        Sedentary,         // Çok az aktif
        LightlyActive,
        ModeratelyActive,
        VeryActive,
        ExtraActive
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class ApplicationUser
    {
        public int Id { get; set; }

        public string FirebaseUid { get; set; } // Firebase Authentication UID
        public string Email { get; set; }

        public int Age { get; set; }
        public Gender Gender {  get; set; }

        public double Weight { get; set; }
        public double Height { get; set; }

        public UserGoal Goal { get; set; }
        public ActivityLevel ActivityLevel { get; set; }

        public double Budget { get; set; }
    }
}
