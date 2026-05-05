using BetonBon.Application.RepositoryInterfaces;
using BetonBon.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.Infrastructure.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly BetonBonDbContext _dbContext;

        public UserRepository(BetonBonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task IUserRepository.AddUserAsync(User user)
        {
            await _dbContext.AddAsync(user);
        }

        async Task<bool> IUserRepository.UsernameExistsAsync(string username)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Username == username);
        }

        async Task IUserRepository.SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
