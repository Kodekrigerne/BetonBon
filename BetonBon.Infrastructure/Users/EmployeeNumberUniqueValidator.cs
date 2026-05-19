using BetonBon.Domain.Users;
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
            return await _db.Users.AsNoTracking().AnyAsync(u => u.EmployeeNumber == employeeNumber);
        }
    }
}
