namespace BetonBon.Infrastructure
{
    public record JwtSettings
    {
        public const string SectionName = "JWT";
        public string Secret { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; }
    }
}