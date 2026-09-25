using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        private CartDTO ToDTO(Cart c) => new CartDTO
        {
            CartId = c.CartId,
            CartUserId = c.CartUserId,
            UserName = c.UserName,
            UserFullName = c.UserFullName,
            CartCreatorId = c.CartCreatorId,
            CartCreationDate = c.CartCreationDate,
            CartModificatorId = c.CartModificatorId,
            CartModificationDate = c.CartModificationDate,
            CartStatusId = c.CartStatusId
        };

        public async Task<IEnumerable<CartDTO>> ListarPorUsuario(int userId)
        {
            var lista = await _repository.ListarPorUsuarioAsync(userId);
            return lista.Select(ToDTO);
        }

        public async Task<CartDTO?> ObtenerPorId(int cartId)
        {
            var item = await _repository.ObtenerPorIdAsync(cartId);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoCarrito(CartCreateDTO dto)
        {
            await _repository.NuevoCarritoAsync(new Cart
            {
                CartUserId = dto.CartUserId,
                CartCreatorId = dto.CartCreatorId
            });
        }

        public async Task EliminarCarrito(int id, int idModificador)
        {
            await _repository.EliminarCarritoAsync(id, idModificador);
        }
    }
}