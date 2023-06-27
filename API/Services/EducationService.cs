using API.Contracts;
using API.DTOs.Educations;
using API.Models;

namespace API.Services
{
    public class EducationService
    {
        private readonly IEducationRepository _educationRepository;
        public EducationService(IEducationRepository educationRepository)
        {
            _educationRepository = educationRepository;
        }

        public IEnumerable<GetEducationDto>? GetEducation()
        {
            var educations = _educationRepository.GetAll();
            if (!educations.Any())
            {
                return null; // No education  found
            }

            var toDto = educations.Select(education =>
                                                new GetEducationDto
                                                {
                                                    Guid = education.Guid,
                                                    Major = education.Major,
                                                    Degree = education.Degree,
                                                    GPA = education.GPA,
                                                    UniversityGuid = education.UniversityGuid
                                                }).ToList();

            return toDto; // education found
        }

        public GetEducationDto? GetEducation(Guid guid)
        {
            var education = _educationRepository.GetByGuid(guid);
            if (education is null)
            {
                return null; // education not found
            }

            var toDto = new GetEducationDto
            {
                Guid = education.Guid,
                Major = education.Major,
                Degree = education.Degree,
                GPA = education.GPA,
                UniversityGuid = education.UniversityGuid
            };
            return toDto; // educations found
        }

        public GetEducationDto? CreateEducation(NewEducationDto newEducationDto)
        {
            var education = new Education
            {
                Guid = newEducationDto.Guid,
                Major = newEducationDto.Major,
                Degree = newEducationDto.Degree,
                GPA = newEducationDto.GPA,
                UniversityGuid = newEducationDto.UniversityGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdEducation = _educationRepository.Create(education);
            if (createdEducation is null)
            {
                return null; // education not created
            }

            var toDto = new GetEducationDto
            {
                Guid = education.Guid,
                Major = education.Major,
                Degree = education.Degree,
                GPA = education.GPA,
                UniversityGuid = education.UniversityGuid
            };

            return toDto; // education created
        }

        public int UpdateBooking(UpdateEducationDto updateEducationDto)
        {
            var isExist = _educationRepository.IsExist(updateEducationDto.Guid);
            if (!isExist)
            {
                return -1; // education not found
            }

            var getBooking = _educationRepository.GetByGuid(updateEducationDto.Guid);

            var education = new Education
            {
                Guid = updateEducationDto.Guid,
                Major = updateEducationDto.Major,
                Degree = updateEducationDto.Degree,
                GPA = updateEducationDto.GPA,
                UniversityGuid = updateEducationDto.UniversityGuid,
                ModifiedDate = DateTime.Now,
                CreatedDate = getBooking!.CreatedDate
            };

            var isUpdate = _educationRepository.Update(education);
            if (!isUpdate)
            {
                return 0; // education not updated
            }

            return 1;
        }

        public int DeleteEducation(Guid guid)
        {
            var isExist = _educationRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // education not found
            }

            var education = _educationRepository.GetByGuid(guid);
            var isDelete = _educationRepository.Delete(education!);
            if (!isDelete)
            {
                return 0; // education not deleted
            }

            return 1;
        }
    }
}
