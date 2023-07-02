using System.Security.Claims;

namespace API.Contracts
{
    public interface ITokenHandler
    {
        public string GenerateToken(IEnumerable<Claim> claims);
    }
}
