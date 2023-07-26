using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Client.ViewModel
{
    public class RegisterVM
    {

        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "0 = Female, 1 = Male")]
        public GenderEnum Gender { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        [Required]
        [EmailAddress]
        [EmployeeDuplicateProperty("string", "Email")]
        public string Email { get; set; }
        [Required]
        [Phone]
        [EmployeeDuplicateProperty("string", "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Major { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        [Range(0, 4, ErrorMessage = "GPA must betwen 0 - 4")]
        public Double Gpa { get; set; }
        [Required]
        public string UniversityCode { get; set; }
        [Required]
        public string UniversityName { get; set; }
        [Required]
        [PasswordPolicy]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
