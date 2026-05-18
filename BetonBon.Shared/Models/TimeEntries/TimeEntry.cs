namespace BetonBon.Shared.Models.TimeEntries
{
    public sealed record TimeEntry
    {
        public required int ProjectNumber { get; init; }
        public required int ActivityNumber { get; init; }
        public required int EmployeeNumber { get; init; }
        public required DateTimeOffset Date { get; init; }
        public required string Text { get; init; }
        public required double NumberOfHours { get; init; }
    }
}
