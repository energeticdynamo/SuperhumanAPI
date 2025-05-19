namespace SuperhumanAPI.Models
{
    public class Mutant
    {
        public int MutantId { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? MutantName { get; set; } = string.Empty;
        public string PrimaryPower { get; set; } = string.Empty;
        public string? SecondaryPower { get; set; } = string.Empty;
        public string TeamAffiliation { get; set; } = string.Empty;
        public string? Classification { get; set; }
    }
}
