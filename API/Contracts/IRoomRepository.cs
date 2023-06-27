using API.Models;

namespace API.Contracts
{
    public interface IRoomRepository : IGeneralRepository<Room>
    {
        IEnumerable<Room> GetByName(string name);
    }
}
