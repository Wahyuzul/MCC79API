using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingDbContext context) : base(context) { }

    public IEnumerable<University> GetByName(string name)
    {
        return _context.Set<University>().Where(university => university.Name.Contains(name));
    }

    public University? GetByCodeandName(string code, string name)
    {
        return _context.Set<University>().FirstOrDefault(University => University.Code.ToLower() == code.ToLower() && University.Name.ToLower() == name.ToLower());
    }
}