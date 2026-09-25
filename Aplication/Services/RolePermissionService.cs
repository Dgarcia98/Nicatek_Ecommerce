using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _repository;

        public RolePermissionService(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        private RolePermissionDTO ToDTO(RolePermission item) => new RolePermissionDTO
        {
            RolePermissionId = item.RolePermissionId,
            RolePermissionRoleId = item.RolePermissionRoleId,
            RoleName = item.RoleName,
            RolePermissionPermissionId = item.RolePermissionPermissionId,
            PermissionName = item.PermissionName,
            PermissionModule = item.PermissionModule,
            RolePermissionCreatorId = item.RolePermissionCreatorId,
            RolePermissionCreationDate = item.RolePermissionCreationDate,
            RolePermissionModificatorId = item.RolePermissionModificatorId,
            RolePermissionModificationDate = item.RolePermissionModificationDate,
            RolePermissionStatusId = item.RolePermissionStatusId
        };

        public async Task<IEnumerable<RolePermissionDTO>> ListarPorRol(int roleId)
        {
            var lista = await _repository.ListarPorRolAsync(roleId);
            return lista.Select(ToDTO);
        }

        public async Task AsignarPermiso(RolePermissionCreateDTO dto)
        {
            await _repository.AsignarPermisoAsync(new RolePermission
            {
                RolePermissionRoleId = dto.RolePermissionRoleId,
                RolePermissionPermissionId = dto.RolePermissionPermissionId,
                RolePermissionCreatorId = dto.RolePermissionCreatorId
            });
        }

        public async Task EliminarAsignacion(int id, int idModificador)
        {
            await _repository.EliminarAsignacionAsync(id, idModificador);
        }
    }
}