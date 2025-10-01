using System.ComponentModel.DataAnnotations;

namespace SuperhumanAPI.Models
{
    public record CosmicBeing : IEquatable<CosmicBeing>
    {
        [Key]
        public int CosmicId { get; init; }
        public string Name { get; init; }
        public string? Role { get; init; }
        public string? Powers { get; init; }
    }
}
