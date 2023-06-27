using API.Models;

namespace API.Contracts
{
    public interface IRoleRepository : IGeneralRepository<Role>
    {
        IEnumerable<Role> GetByName(string name);
    }
}
