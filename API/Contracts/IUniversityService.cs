using API.DTOs.Universities;

namespace API.Contracts
{
    public interface IUniversityService
    {
        IEnumerable<GetUniversityDto> GetUniversity();
    }
}
