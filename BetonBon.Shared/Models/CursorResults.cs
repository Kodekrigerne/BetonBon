namespace BetonBon.Shared.Models
{
    public class CursorResults<T>
    {
        public string? Cursor { get; init; }
        public IReadOnlyList<T>? Items { get; init; }
    }
}
