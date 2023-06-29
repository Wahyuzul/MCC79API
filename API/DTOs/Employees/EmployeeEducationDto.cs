using API.Utilities;

namespace API.DTOs.Employees
{
    public class EmployeeEducationDto
    {
        public Guid Guid { get; set; }
        public string NIK { get; set; }
        public string FullName {  get; set; }
        public DateTime BirthDate { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime HiringDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Major { get; set; }
        public string Degree { get; set; }
        public double GPA { get; set; }
        public string UniversityName { get; set; }
    }
}
