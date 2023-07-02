using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employees
{
    public class NewEmployeeDto
    {
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

        [EmailAddress]
        [EmployeeDuplicateProperty("string", "Email")]
        public string Email { get; set; }

        [Phone]
        [EmployeeDuplicateProperty("string", "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
