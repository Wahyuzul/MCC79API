using API.Contracts;
using API.DTOs.Roles;
using API.DTOs.Rooms;
using API.Models;
using API.Repositories;
using System.Data;

namespace API.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public IEnumerable<GetRoleDto>? GetRole()
        {
            var roles = _roleRepository.GetAll();
            if (!roles.Any())
            {
                return Enumerable.Empty<GetRoleDto>(); // No roles found
            }

            var toDto = roles.Select(role =>
                new GetRoleDto
                {
                    Guid = role.Guid,
                    Name = role.Name
                }).ToList();

            return toDto; // Roles found
        }

        /*public IEnumerable<GetRoleDto>? GetRole(string name)
        {
            var roles = _roleRepository.GetByName(name);
            if (!roles.Any())
            {
                return Enumerable.Empty<GetRoleDto>(); // No rooms found
            }

            var toDto = roles.Select(role =>
                 new GetRoleDto
                 {
                     Guid = role.Guid,
                     Name = role.Name
                 }).ToList();

            return toDto; // Rooms found
        }*/

        public GetRoleDto? GetRole(Guid guid)
        {
            var role = _roleRepository.GetByGuid(guid);
            if (role is null)
            {
                return null; // Role not found
            }

            var toDto = new GetRoleDto
            {
                Guid = role.Guid,
                Name = role.Name
            };

            return toDto; // Role found
        }

        public GetRoleDto? CreateRole(NewRoleDto newRoleDto)
        {
            var role = new Role
            {
                Name = newRoleDto.Name
            };

            var createdRole = _roleRepository.Create(role);

            var toDto = new GetRoleDto
            {
                Guid = createdRole.Guid,
                Name = createdRole.Name
            };

            return toDto; // Role created
        }

        public bool UpdateRole(UpdateRoleDto updateRoleDto)
        {
            var isExist = _roleRepository.IsExist(updateRoleDto.Guid);
            if (!isExist)
            {
                return false; // Role not found
            }

            var role = new Role
            {
                Guid = updateRoleDto.Guid,
                Name = updateRoleDto.Name
            };

            var isUpdate = _roleRepository.Update(role);
            return isUpdate; // Role updated or not
        }

        public bool DeleteRole(Guid guid)
        {
            var isExist = _roleRepository.IsExist(guid);
            if (!isExist)
            {
                return false; // Role not found
            }

            var role = _roleRepository.GetByGuid(guid);
            var isDelete = _roleRepository.Delete(role!);
            return isDelete; // Role deleted or not
        }
    }

}
