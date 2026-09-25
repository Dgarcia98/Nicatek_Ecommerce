using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _repository;

        public UserAddressService(IUserAddressRepository repository)
        {
            _repository = repository;
        }

        private UserAddressDTO ToDTO(UserAddress item) => new UserAddressDTO
        {
            UserAddressId = item.UserAddressId,
            UserAddressUserId = item.UserAddressUserId,
            UserName = item.UserName,
            UserAddressCountryId = item.UserAddressCountryId,
            UserAddressZIPCode = item.UserAddressZIPCode,
            UserAddressDescription = item.UserAddressDescription,
            UserAddressIsPrincipal = item.UserAddressIsPrincipal,
            UserAddressCreatorId = item.UserAddressCreatorId,
            UserAddressCreationDate = item.UserAddressCreationDate,
            UserAddressModificatorId = item.UserAddressModificatorId,
            UserAddressModificationDate = item.UserAddressModificationDate,
            UserAddressStatusId = item.UserAddressStatusId
        };

        public async Task<IEnumerable<UserAddressDTO>> ListarPorUsuario(int userId)
        {
            var lista = await _repository.ListarPorUsuarioAsync(userId);
            return lista.Select(ToDTO);
        }

        public async Task NuevaDireccion(UserAddressCreateDTO dto)
        {
            await _repository.NuevadireccionAsync(new UserAddress
            {
                UserAddressUserId = dto.UserAddressUserId,
                UserAddressCountryId = dto.UserAddressCountryId,
                UserAddressZIPCode = dto.UserAddressZIPCode,
                UserAddressDescription = dto.UserAddressDescription,
                UserAddressIsPrincipal = dto.UserAddressIsPrincipal,
                UserAddressCreatorId = dto.UserAddressCreatorId
            });
        }

        public async Task EditarDireccion(UserAddressUpdateDTO dto)
        {
            await _repository.EditarDireccionAsync(new UserAddress
            {
                UserAddressId = dto.UserAddressId,
                UserAddressCountryId = dto.UserAddressCountryId,
                UserAddressZIPCode = dto.UserAddressZIPCode,
                UserAddressDescription = dto.UserAddressDescription,
                UserAddressIsPrincipal = dto.UserAddressIsPrincipal,
                UserAddressModificatorId = dto.UserAddressModificatorId
            });
        }

        public async Task EliminarDireccion(int id, int idModificador)
        {
            await _repository.EliminarDireccionAsync(id, idModificador);
        }
    }
}