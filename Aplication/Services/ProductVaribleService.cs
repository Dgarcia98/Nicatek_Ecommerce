using System;
using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class ProductVariableService : IProductVariableService
    {
        private readonly IProductVariableRepository _repository;

public Task ActualizarDescuento(int productVariableId, decimal descuento, DateTime? hasta, int modificadorId)
            => _repository.ActualizarDescuentoAsync(productVariableId, descuento, hasta, modificadorId);

                public ProductVariableService(IProductVariableRepository repository)
        {
            _repository = repository;
        }

        private ProductVariableDTO ToDTO(ProductVariable p) => new ProductVariableDTO
        {
            ProductVariableId = p.ProductVariableId,
            ProductVariableProductId = p.ProductVariableProductId,
            ProductName = p.ProductName,
            ProductVariableValue = p.ProductVariableValue,
            ProductVariablePrice = p.ProductVariablePrice,
            ProductVariableCurrencyId = p.ProductVariableCurrencyId,
            CurrencyISO = p.CurrencyISO,
            CurrencyName = p.CurrencyName,
            StockDisponible = p.StockDisponible,
            VariableTypeName = p.VariableTypeName,
            ProductVariableCreatorId = p.ProductVariableCreatorId,
            ProductVariableCreationDate = p.ProductVariableCreationDate,
            ProductVariableModificatorId = p.ProductVariableModificatorId,
            ProductVariableModificationDate = p.ProductVariableModificationDate,
            ProductVariableStatusId = p.ProductVariableStatusId,
            ProductVariableDiscount = p.ProductVariableDiscount
        };

        public async Task<IEnumerable<ProductVariableDTO>> Listar(int? productId = null, bool soloActivos = true)
        {
            var lista = await _repository.ListarAsync(productId, soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<ProductVariableDTO>> Filtrar(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<ProductVariableDTO>();
            var lista = await _repository.FiltrarAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<ProductVariableDTO?> ObtenerPorId(int id)
        {
            var item = await _repository.ObtenerPorIdAsync(id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevaVariable(ProductVariableCreateDTO dto)
        {
            await _repository.NuevaVariableAsync(new ProductVariable
            {
                ProductVariableProductId = dto.ProductVariableProductId,
                ProductVariableValue = dto.ProductVariableValue,
                ProductVariablePrice = dto.ProductVariablePrice,
                ProductVariableCurrencyId = dto.ProductVariableCurrencyId,
                ProductVariableCreatorId = dto.ProductVariableCreatorId
            });
        }

        public async Task EditarVariable(ProductVariableUpdateDTO dto)
        {
            await _repository.EditarVariableAsync(new ProductVariable
            {
                ProductVariableId = dto.ProductVariableId,
                ProductVariableProductId = dto.ProductVariableProductId,
                ProductVariableValue = dto.ProductVariableValue,
                ProductVariablePrice = dto.ProductVariablePrice,
                ProductVariableCurrencyId = dto.ProductVariableCurrencyId,
                ProductVariableModificatorId = dto.ProductVariableModificatorId
            });
        }

        public async Task EliminarVariable(int id, int idModificador)
        {
            await _repository.EliminarVariableAsync(id, idModificador);
        }
    }
}