using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class PaymentMethodTypeService : IPaymentMethodTypeService
    {
        private readonly IPaymentMethodTypeRepository _repository;

        public PaymentMethodTypeService(IPaymentMethodTypeRepository repository)
        {
            _repository = repository;
        }

        private PaymentMethodTypeDTO ToDTO(PaymentMethodTypes item) => new PaymentMethodTypeDTO
        {
            PaymentMethodTypeId = item.PaymentMethodTypeId,
            PaymentMethodTypeName = item.PaymentMethodTypeName,
            PaymentMethodTypeDescription = item.PaymentMethodTypeDescription,
            PaymentMethodTypeCreatorId = item.PaymentMethodTypeCreatorId,
            PaymentMethodTypeCreationDate = item.PaymentMethodTypeCreationDate,
            PaymentMethodTypeModificatorId = item.PaymentMethodTypeModificatorId,
            PaymentMethodTypeModificationDate = item.PaymentMethodTypeModificationDate,
            PaymentMethodTypeStatusId = item.PaymentMethodTypeStatusId
        };

        public async Task<IEnumerable<PaymentMethodTypeDTO>> ListarPaymentMethodTypes()
        {
            var lista = await _repository.ListarPaymentMethodTypesAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<PaymentMethodTypeDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<PaymentMethodTypeDTO>();

            var lista = await _repository.ListarPaymentMethodTypesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<PaymentMethodTypeDTO?> ObtenerPaymentMethodTypePorId(int id)
        {
            var lista = await _repository.ListarPaymentMethodTypesFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.PaymentMethodTypeId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoPaymentMethodType(PaymentMethodTypeDTO dto)
        {
            await _repository.NuevoPaymentMethodTypeAsync(new PaymentMethodTypes
            {
                PaymentMethodTypeName = dto.PaymentMethodTypeName,
                PaymentMethodTypeDescription = dto.PaymentMethodTypeDescription,
                PaymentMethodTypeCreatorId = dto.PaymentMethodTypeCreatorId
            });
        }

        public async Task EditarPaymentMethodType(PaymentMethodTypeDTO dto)
        {
            await _repository.EditarPaymentMethodTypeAsync(new PaymentMethodTypes
            {
                PaymentMethodTypeId = dto.PaymentMethodTypeId,
                PaymentMethodTypeName = dto.PaymentMethodTypeName,
                PaymentMethodTypeDescription = dto.PaymentMethodTypeDescription,
                PaymentMethodTypeModificatorId = dto.PaymentMethodTypeModificatorId ?? dto.PaymentMethodTypeCreatorId
            });
        }

        public async Task EliminarPaymentMethodType(int id, int idModificador)
        {
            await _repository.EliminarPaymentMethodTypeAsync(id, idModificador);
        }
    }
}