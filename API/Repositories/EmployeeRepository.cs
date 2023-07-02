using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BookingDbContext context) : base(context)
        {
        }

        public Employee? GetEmail(string email)
        {
            return _context.Set<Employee>().SingleOrDefault(e => e.Email == email);
        }

        public Employee? GetByEmailAndPhoneNumber(string data)
        {
            return _context.Set<Employee>().FirstOrDefault(e => e.PhoneNumber == data || e.Email == data);
        }
    }
}
