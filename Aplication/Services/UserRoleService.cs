using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _repository;

        public UserRoleService(IUserRoleRepository repository)
        {
            _repository = repository;
        }

        private UserRoleDTO ToDTO(UserRole item) => new UserRoleDTO
        {
            UserRoleId = item.UserRoleId,
            UserRoleUserId = item.UserRoleUserId,
            UserFullName = item.UserFullName,
            UserName = item.UserName,
            UserRoleRoleId = item.UserRoleRoleId,
            RoleName = item.RoleName,
            UserRoleCreatorId = item.UserRoleCreatorId,
            UserRoleCreationDate = item.UserRoleCreationDate,
            UserRoleModificatorId = item.UserRoleModificatorId,
            UserRoleModificationDate = item.UserRoleModificationDate,
            UserRoleStatusId = item.UserRoleStatusId
        };

        public async Task<IEnumerable<UserRoleDTO>> ListarPorUsuario(int userId)
        {
            var lista = await _repository.ListarPorUsuarioAsync(userId);
            return lista.Select(ToDTO);
        }

        public async Task AsignarRol(UserRoleCreateDTO dto)
        {
            await _repository.AsignarRolAsync(new UserRole
            {
                UserRoleUserId = dto.UserRoleUserId,
                UserRoleRoleId = dto.UserRoleRoleId,
                UserRoleCreatorId = dto.UserRoleCreatorId
            });
        }

        public async Task EliminarAsignacion(int id, int idModificador)
        {
            await _repository.EliminarAsignacionAsync(id, idModificador);
        }
    }
}