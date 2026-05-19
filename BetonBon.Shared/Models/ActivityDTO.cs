using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public sealed record ActivityDTO
    {
        public required int Number { get; init; }

        public required string Name { get; init; }
    }
}
