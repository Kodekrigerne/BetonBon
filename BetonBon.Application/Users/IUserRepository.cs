using BetonBon.Domain.Users;

namespace BetonBon.Application.Users
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        void Delete(User user);
        Task<User> GetByIdAsync(Guid id);
        Task<bool> UsernameExistsAsync(string username);
        void Update(User user, uint rowVersion);
        Task SaveChangesAsync();
    }
}
