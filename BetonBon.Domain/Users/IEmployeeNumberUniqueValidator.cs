namespace BetonBon.Domain.Users
{
    public interface IEmployeeNumberUniqueValidator
    {
        bool ValidateUniqueEmployeeNumber(int employeeNumber);
    }
}
