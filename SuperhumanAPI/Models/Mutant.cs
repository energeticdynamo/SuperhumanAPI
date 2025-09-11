using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SuperhumanAPI.Models
{
    public record Mutant : IEquatable<Mutant>, IEntity
    {
        [Key]
        public int Id { get; set; } // Changed from 'init' to 'set' to satisfy IEntity
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? MutantName { get; init; }
        public required string PrimaryPower { get; init; }
        public string? SecondaryPower { get; init; }
        public string? Classification { get; init; }
        public int? TeamId { get; init; }
    }
}
