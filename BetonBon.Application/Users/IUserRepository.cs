using BetonBon.Domain.Users;

namespace BetonBon.Application.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
        Task SaveChangesAsync();
    }
}
