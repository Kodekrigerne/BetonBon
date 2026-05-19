using BetonBon.Domain.Users;
using BetonBon.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.Infrastructure.Users
{
    public class EmployeeNumberUniqueValidator : IEmployeeNumberUniqueValidator
    {
        private readonly BetonBonDbContext _db;

        public EmployeeNumberUniqueValidator(BetonBonDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ValidateUniqueEmployeeNumberAsync(int employeeNumber)
        {
            return await _db.Users.AsNoTracking().Where(u => u.Role != UserRole.Admin).AnyAsync(u => u.EmployeeNumber == employeeNumber);
        }
    }
}
