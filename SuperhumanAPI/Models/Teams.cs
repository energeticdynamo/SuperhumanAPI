using System.ComponentModel.DataAnnotations;

namespace SuperhumanAPI.Models
{
    public class Teams
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string? BaseOfOperations { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? TeamLeader {  get; set; }
    }
}
