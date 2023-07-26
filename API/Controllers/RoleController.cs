using API.Contracts;
using API.DTOs.Roles;
using API.DTOs.Rooms;
using API.Models;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    //[Authorize(Roles = $"{nameof(RoleLevel.admin)}")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;

        public RoleController(RoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetRole();

            if (!entities.Any())
            {
                return NotFound(new ResponseHandler<GetRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetRoleDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetById(Guid guid)
        {
            var role = _service.GetRole(guid);
            if (role is null)
            {
                return NotFound(new ResponseHandler<GetRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = role
            });
        }

        [HttpPost]
        public IActionResult Create(NewRoleDto newRoleDto)
        {
            var createdRole = _service.CreateRole(newRoleDto);
            if (createdRole is null)
            {
                return BadRequest(new ResponseHandler<GetRoleDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status201Created,
                Status = HttpStatusCode.Created.ToString(),
                Message = "Successfully created",
                Data = createdRole
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateRoleDto updateRoleDto)
        {
            var isUpdated = _service.UpdateRole(updateRoleDto);
            if (!isUpdated)
            {
                return NotFound(new ResponseHandler<UpdateRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Role not found"
                });
            }

            return Ok(new ResponseHandler<UpdateRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var isDeleted = _service.DeleteRole(guid);

            if (!isDeleted)
            {
                return NotFound(new ResponseHandler<GetRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Role not found"
                });
            }

            return Ok(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }

        /*[HttpGet("by-name/{name}")]
        public IActionResult GetByName(string name)
        {
            var roles = _service.GetRole(name);
            if (!roles.Any())
            {
                return NotFound(new ResponseHandler<GetRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No role found with the given name"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetRoleDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Roles found",
                Data = roles
            });
        }*/
    }
}
