using API.Utilities;
using System.Security.Cryptography.X509Certificates;

namespace API.DTOs.Bookings
{
    public class BookedRoomDto
    {
        public Guid Guid { get; set; }
        public string RoomName { get; set; }
        public string BookedBy { get; set; }
        public StatusLevel Status { get; set; }
        public int Floor { get; set; }

    }
}
