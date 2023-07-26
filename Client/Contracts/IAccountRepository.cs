using API.DTOs.Accounts;
using API.Utilities;
using Client.Repositories;
using Client.ViewModel;

namespace Client.Contracts
{
    public interface IAccountRepository : IRepository<RegisterAccountDto, string>
    {
        public Task<ResponseHandler<AccountRepository>> Register(RegisterAccountDto entity);
        public Task<ResponseHandler<string>> Login(LoginAccountDto entity);
    }
}
