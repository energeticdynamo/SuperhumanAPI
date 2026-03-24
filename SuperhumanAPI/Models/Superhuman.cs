using System.ComponentModel.DataAnnotations;

namespace SuperhumanAPI.Models
{
    public class Superhuman
    {
        public int SuperhumanId { get; set; }

        [Required(ErrorMessage ="First Name is required")]
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? CodeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary Power is required")]
        public string PrimaryPower { get; set; } = string.Empty;
        public string? SecondaryPower { get; set; } = string.Empty;

        [Range(0, 10, ErrorMessage = "Ranking must be between 0 and 10")]
        public int? Ranking { get; set; }
        public int? TeamId { get; set; }
        public string? TeamName { get; set; } = string.Empty;
        public bool? IsTeamLeader { get; set; }
    }
}
