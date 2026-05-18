namespace BetonBon.API.Extensions
{
    public static class ConnectionStringBuilder
    {
        extension(IConfiguration config)
        {
            public string GetConnectionString()
            {
                var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
                var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
                var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
                var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
                var dbPass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

                return $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};Trust Server Certificate=true;";
            }
        }
    }
}
