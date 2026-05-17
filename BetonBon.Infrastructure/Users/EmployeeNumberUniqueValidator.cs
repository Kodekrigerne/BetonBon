using BetonBon.Domain.Users;

namespace BetonBon.Infrastructure.Users
{
    public class EmployeeNumberUniqueValidator : IEmployeeNumberUniqueValidator
    {
        private readonly BetonBonDbContext _db;

        public EmployeeNumberUniqueValidator(BetonBonDbContext db)
        {
            _db = db;
        }

        public bool ValidateUniqueEmployeeNumber(int employeeNumber)
        {
            return _db.Users.Any(u => u.EmployeeNumber == employeeNumber);
        }
    }
}
