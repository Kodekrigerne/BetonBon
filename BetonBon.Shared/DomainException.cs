namespace BetonBon.Shared
{
    public class DomainException : Exception
    {
        public string? ParamName { get; }

        public DomainException() : base() { }
        public DomainException(string message) : base(message) { }


        public DomainException(string message, string paramName) : base(message)
        {
            ParamName = paramName;
        }

        public DomainException(string message, Exception innerException) : base(message, innerException) { }
    }
}
