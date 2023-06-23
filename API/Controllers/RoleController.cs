using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repository;

        public RoleController(IRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var role = _repository.GetAll();

            if (!role.Any())
            {
                return NotFound();  
            }
            return Ok(role);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid) 
        {
            var role = _repository.GetByGuid(guid);
            if (role is null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPost]
        public IActionResult Create(Role role)
        {
            var createdRole = _repository.Create(role);
            return Ok(createdRole);
        }

        [HttpPut]
        public IActionResult Update(Role role)
        {
            var isUpdated = _repository.Update(role);
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
