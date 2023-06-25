using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/account-roles")]
    public class AccountRoleController : GeneralController<AccountRole>
    {
        public AccountRoleController(IAccountRoleRepository repository) : base(repository)
        {
        }
    }
}
