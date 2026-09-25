using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;

        public PermissionService(IPermissionRepository repository)
        {
            _repository = repository;
        }

        private PermissionDTO ToDTO(Permissions item) => new PermissionDTO
        {
            PermissionId = item.PermissionId,
            PermissionName = item.PermissionName,
            PermissionDescription = item.PermissionDescription,
            PermissionModule = item.PermissionModule,
            PermissionCreatorId = item.PermissionCreatorId,
            PermissionCreationDate = item.PermissionCreationDate,
            PermissionModificatorId = item.PermissionModificatorId,
            PermissionModificationDate = item.PermissionModificationDate,
            PermissionStatusId = item.PermissionStatusId
        };

        public async Task<IEnumerable<PermissionDTO>> ListarPermissions()
        {
            var lista = await _repository.ListarPermissionsAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<PermissionDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<PermissionDTO>();
            var lista = await _repository.ListarPermissionsFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<PermissionDTO?> ObtenerPermissionPorId(int id)
        {
            var lista = await _repository.ListarPermissionsFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.PermissionId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoPermission(PermissionDTO dto)
        {
            await _repository.NuevoPermissionAsync(new Permissions
            {
                PermissionName = dto.PermissionName,
                PermissionDescription = dto.PermissionDescription,
                PermissionModule = dto.PermissionModule,
                PermissionCreatorId = dto.PermissionCreatorId
            });
        }

        public async Task EditarPermission(PermissionDTO dto)
        {
            await _repository.EditarPermissionAsync(new Permissions
            {
                PermissionId = dto.PermissionId,
                PermissionName = dto.PermissionName,
                PermissionDescription = dto.PermissionDescription,
                PermissionModule = dto.PermissionModule,
                PermissionModificatorId = dto.PermissionModificatorId ?? dto.PermissionCreatorId
            });
        }

        public async Task EliminarPermission(int id, int idModificador)
        {
            await _repository.EliminarPermissionAsync(id, idModificador);
        }
    }
}