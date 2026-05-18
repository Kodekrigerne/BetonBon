namespace BetonBon.Shared.Models
{
    public sealed record Employee
    {
        public required int Number { get; init; }
        public required string Name { get; init; }
        public required int GroupNumber { get; init; }
        public required bool IsBarred { get; init; }
        public string? Phone { get; init; }
        public string? Email { get; init; }
    }
}