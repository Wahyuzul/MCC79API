using API.DTOs.Employees;
using Client.Contracts;

namespace Client.Repositories
{
    public class EmployeeRepository : GeneralRepository<GetEmployeeDto, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(string request = "employees/") : base(request)
        {
        }
    }
}
