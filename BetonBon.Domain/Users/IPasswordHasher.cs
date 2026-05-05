namespace BetonBon.Domain.Users
{
    public interface IPasswordHasher
    {
        PasswordHash HashPassword(string password);
        bool VerifyPassword(string password, PasswordHash hashedPassword);
    }

    public record PasswordHash(string Hash, string Salt);
}