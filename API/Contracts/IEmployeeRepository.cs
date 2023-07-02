using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        public Employee? GetByEmailAndPhoneNumber(string data);
        Employee? GetEmail(string email);
    }
}
