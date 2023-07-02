using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Rooms
{
    public class UnusedRoomDto
    {
        [Required]
        public Guid Guid { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 10)]
        public int Floor { get; set; }
        [Required]
        [Range(0, 5)]
        public int Capacity { get; set; }
    }
}
