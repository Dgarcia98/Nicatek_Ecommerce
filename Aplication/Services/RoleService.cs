using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        private RoleDTO ToDTO(Roles item) => new RoleDTO
        {
            RoleId = item.RoleId,
            RoleName = item.RoleName,
            RoleDescription = item.RoleDescription,
            RoleCreatorId = item.RoleCreatorId,
            RoleCreationDate = item.RoleCreationDate,
            RoleModificatorId = item.RoleModificatorId,
            RoleModificationDate = item.RoleModificationDate,
            RoleStatusId = item.RoleStatusId
        };

        public async Task<IEnumerable<RoleDTO>> ListarRoles()
        {
            var lista = await _repository.ListarRolesAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<RoleDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<RoleDTO>();

            var lista = await _repository.ListarRolesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<RoleDTO?> ObtenerRolePorId(int id)
        {
            var lista = await _repository.ListarRolesFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.RoleId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoRole(RoleDTO dto)
        {
            await _repository.NuevoRoleAsync(new Roles
            {
                RoleName = dto.RoleName,
                RoleDescription = dto.RoleDescription,
                RoleCreatorId = dto.RoleCreatorId
            });
        }

        public async Task EditarRole(RoleDTO dto)
        {
            await _repository.EditarRoleAsync(new Roles
            {
                RoleId = dto.RoleId,
                RoleName = dto.RoleName,
                RoleDescription = dto.RoleDescription,
                RoleModificatorId = dto.RoleModificatorId ?? dto.RoleCreatorId
            });
        }

        public async Task EliminarRole(int id, int idModificador)
        {
            await _repository.EliminarRoleAsync(id, idModificador);
        }
    }
}