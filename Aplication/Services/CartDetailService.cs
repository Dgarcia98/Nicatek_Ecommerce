using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class CartDetailService : ICartDetailService
    {
        private readonly ICartDetailRepository _repository;

        public CartDetailService(ICartDetailRepository repository)
        {
            _repository = repository;
        }

        private CartDetailDTO ToDTO(CartDetail c) => new CartDetailDTO
        {
            CartDetailId = c.CartDetailId,
            CartDetailCartId = c.CartDetailCartId,
            CartDetailProductVariableId = c.CartDetailProductVariableId,
            ProductVariableValue = c.ProductVariableValue,
            ProductName = c.ProductName,
            CartDetailPrice = c.CartDetailPrice,
            CartDetailQuantity = c.CartDetailQuantity,
            CartDetailDiscount = c.CartDetailDiscount,
            CartDetailSubTotal = c.CartDetailSubTotal,
            CartDetailTAX = c.CartDetailTAX,
            CartDetailTotal = c.CartDetailTotal,
            CartDetailCurrencyId = c.CartDetailCurrencyId,
            CurrencyISO = c.CurrencyISO,
            CartDetailCreatorId = c.CartDetailCreatorId,
            CartDetailCreationDate = c.CartDetailCreationDate,
            CartDetailModificatorId = c.CartDetailModificatorId,
            CartDetailModificationDate = c.CartDetailModificationDate,
            CartDetailStatusId = c.CartDetailStatusId
        };

        public async Task<IEnumerable<CartDetailDTO>> ListarPorCarrito(int cartId)
        {
            var lista = await _repository.ListarPorCarritoAsync(cartId);
            return lista.Select(ToDTO);
        }

        public async Task NuevoDetalle(CartDetailCreateDTO dto)
        {
            await _repository.NuevoDetalleAsync(new CartDetail
            {
                CartDetailCartId = dto.CartDetailCartId,
                CartDetailProductVariableId = dto.CartDetailProductVariableId,
                CartDetailPrice = dto.CartDetailPrice,
                CartDetailQuantity = dto.CartDetailQuantity,
                CartDetailDiscount = dto.CartDetailDiscount,
                CartDetailSubTotal = dto.CartDetailSubTotal,
                CartDetailTAX = dto.CartDetailTAX,
                CartDetailTotal = dto.CartDetailTotal,
                CartDetailCurrencyId = dto.CartDetailCurrencyId,
                CartDetailCreatorId = dto.CartDetailCreatorId
            });
        }

        public async Task EditarDetalle(CartDetailUpdateDTO dto)
        {
            await _repository.EditarDetalleAsync(new CartDetail
            {
                CartDetailId = dto.CartDetailId,
                CartDetailQuantity = dto.CartDetailQuantity,
                CartDetailDiscount = dto.CartDetailDiscount,
                CartDetailSubTotal = dto.CartDetailSubTotal,
                CartDetailTAX = dto.CartDetailTAX,
                CartDetailTotal = dto.CartDetailTotal,
                CartDetailModificatorId = dto.CartDetailModificatorId
            });
        }

        public async Task EliminarDetalle(int id, int idModificador)
        {
            await _repository.EliminarDetalleAsync(id, idModificador);
        }
    }
}