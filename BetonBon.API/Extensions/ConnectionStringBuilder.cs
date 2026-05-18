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
                var dbName = Environment.GetEnvironmentVariable("DB_NAME");
                var dbUser = Environment.GetEnvironmentVariable("DB_USER");
                var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

                return $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};Trust Server Certificate=true;";
            }
        }
    }
}
