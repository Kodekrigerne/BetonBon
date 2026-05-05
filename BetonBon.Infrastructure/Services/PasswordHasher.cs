using BetonBon.Domain.Users;
using System.Security.Cryptography;

namespace BetonBon.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        PasswordHash IPasswordHasher.HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: saltBytes,
                iterations: 600000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);

            return new PasswordHash(Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        bool IPasswordHasher.VerifyPassword(string password, PasswordHash hashedPassword)
        {
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(hashedPassword.Salt),
                iterations: 600000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);

            return CryptographicOperations.FixedTimeEquals(hashBytes, Convert.FromBase64String(hashedPassword.Hash));
        }
    }
}
