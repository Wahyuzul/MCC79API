using API.Models;

namespace API.Contracts
{
    public interface IAccountRoleRepository
    {
        ICollection<AccountRole> GetAll();
        AccountRole? GetByGuid(Guid guid);
        AccountRole Create(AccountRole accountRole);
        bool Update(AccountRole accountRole);
        bool Delete(Guid guid);
    }
}
