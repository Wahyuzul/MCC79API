using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employees
{
    public class UpdateEmployeeDto
    {
        [Required]
        public Guid Guid { get; set; }
        [Required]
        public string NIK { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "0 = Female, 1 = Male ")]
        public GenderEnum Gender { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
