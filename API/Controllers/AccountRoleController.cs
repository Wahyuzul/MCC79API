using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/account-role")]
    public class AccountRoleController : ControllerBase
    {
        private readonly IAccountRoleRepository _repository;

        public AccountRoleController(IAccountRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var accountRole = _repository.GetAll();

            if (!accountRole.Any())
            {
                return NotFound();
            }
            return Ok(accountRole);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var accountRole = _repository.GetByGuid(guid);
            if (accountRole is null)
            {
                return NotFound();
            }
            return Ok(accountRole);
        }

        [HttpPost]
        public IActionResult Create(AccountRole accountRole)
        {
            var createdAccountRole = _repository.Create(accountRole);
            return Ok(createdAccountRole);
        }

        [HttpPut]
        public IActionResult Update(AccountRole accountRole)
        {
            var isUpdated = _repository.Update(accountRole);
            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok(isUpdated);
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var isDeleted = _repository.Delete(guid);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
