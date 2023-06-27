using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Universities
{
    public class UpdateUniversityDto
    {
        [Required]
        public Guid Guid { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
