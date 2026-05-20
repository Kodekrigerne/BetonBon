namespace BetonBon.Domain.Users
{
    public interface IEmployeeNumberUniqueValidator
    {
        Task<bool> ValidateUniqueEmployeeNumberAsync(int employeeNumber);
    }
}
