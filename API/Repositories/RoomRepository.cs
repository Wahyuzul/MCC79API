using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class RoomRepository : GeneralRepository<Room>, IRoomRepository
    {
        public RoomRepository(BookingDbContext context) : base(context){ }

        public IEnumerable<Room> GetByName(string name)
        {
            return _context.Set<Room>().Where(room => room.Name.Contains(name));
        }
    }
}
