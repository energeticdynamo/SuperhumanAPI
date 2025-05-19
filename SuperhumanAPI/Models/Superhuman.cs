namespace SuperhumanAPI.Models
{
    public class Superhuman
    {
        public int SuperhumanId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? CodeName { get; set; } = string.Empty;
        public string PrimaryPower { get; set; } = string.Empty;
        public string? SecondaryPower { get; set; } = string.Empty;
        public int? Ranking { get; set; }
        public int? TeamId { get; set; }
        public string? TeamName { get; set; } = string.Empty;
    }
}
