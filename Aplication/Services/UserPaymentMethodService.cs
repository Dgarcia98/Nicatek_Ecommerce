using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Aplication.Services
{
    public class UserPaymentMethodService : IUserPaymentMethodService
    {
        private readonly IUserPaymentMethodRepository _repository;

        public UserPaymentMethodService(IUserPaymentMethodRepository repository)
        {
            _repository = repository;
        }

        // Cifrado AES-256 en .NET para los campos sensibles
        private static readonly byte[] _key = Encoding.UTF8.GetBytes("NicatekEcommerceKey2026!@#$%^&*+");
        private static readonly byte[] _iv = Encoding.UTF8.GetBytes("NicatekIV2026!@#");

        private byte[] Cifrar(string texto)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var encryptor = aes.CreateEncryptor();
            var bytes = Encoding.UTF8.GetBytes(texto);
            return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
        }

        private string Descifrar(byte[] datos)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;
                using var decryptor = aes.CreateDecryptor();
                var bytes = decryptor.TransformFinalBlock(datos, 0, datos.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch { return "****"; }
        }

        private string MascararTarjeta(byte[] cardNumberBytes)
        {
            try
            {
                var numero = Descifrar(cardNumberBytes);
                if (numero.Length >= 4)
                    return "**** **** **** " + numero[^4..];
                return "****";
            }
            catch { return "****"; }
        }

        private UserPaymentMethodDTO ToDTO(UserPaymentMethod item) => new UserPaymentMethodDTO
        {
            UserPaymentMethodId = item.UserPaymentMethodId,
            UserPaymentMethodUserId = item.UserPaymentMethodUserId,
            UserName = item.UserName,
            UserPaymentMethodPaymentMethodTypeId = item.UserPaymentMethodPaymentMethodTypeId,
            PaymentMethodTypeName = item.PaymentMethodTypeName,
            UserPaymentMethodCardHolderName = item.UserPaymentMethodCardHolderName,
            CardNumberMasked = item.UserPaymentMethodCardNumber != null
                                                    ? MascararTarjeta(item.UserPaymentMethodCardNumber)
                                                    : null,
            ExpirationDateDecrypted = item.UserPaymentMethodExpirationDate != null
                                                    ? Descifrar(item.UserPaymentMethodExpirationDate)
                                                    : null,
            UserPaymentMethodCreatorId = item.UserPaymentMethodCreatorId,
            UserPaymentMethodCreationDate = item.UserPaymentMethodCreationDate,
            UserPaymentMethodModificatorId = item.UserPaymentMethodModificatorId,
            UserPaymentMethodModificationDate = item.UserPaymentMethodModificationDate,
            UserPaymentMethodStatusId = item.UserPaymentMethodStatusId
        };

        public async Task<IEnumerable<UserPaymentMethodDTO>> ListarPorUsuario(int userId)
        {
            var lista = await _repository.ListarPorUsuarioAsync(userId);
            return lista.Select(ToDTO);
        }

        public async Task NuevoMetodoPago(UserPaymentMethodCreateDTO dto)
        {
            await _repository.NuevoMetodoPagoAsync(new UserPaymentMethod
            {
                UserPaymentMethodUserId = dto.UserPaymentMethodUserId,
                UserPaymentMethodPaymentMethodTypeId = dto.UserPaymentMethodPaymentMethodTypeId,
                UserPaymentMethodCardNumber = Cifrar(dto.CardNumber!),
                UserPaymentMethodExpirationDate = Cifrar(dto.ExpirationDate!),
                UserPaymentMethodCVV = Cifrar(dto.CVV!),
                UserPaymentMethodCardHolderName = dto.UserPaymentMethodCardHolderName,
                UserPaymentMethodCreatorId = dto.UserPaymentMethodCreatorId
            });
        }

        public async Task EditarMetodoPago(UserPaymentMethodUpdateDTO dto)
        {
            await _repository.EditarMetodoPagoAsync(new UserPaymentMethod
            {
                UserPaymentMethodId = dto.UserPaymentMethodId,
                UserPaymentMethodPaymentMethodTypeId = dto.UserPaymentMethodPaymentMethodTypeId,
                UserPaymentMethodExpirationDate = dto.ExpirationDate != null
                                                        ? Cifrar(dto.ExpirationDate)
                                                        : null,
                UserPaymentMethodCardHolderName = dto.UserPaymentMethodCardHolderName,
                UserPaymentMethodModificatorId = dto.UserPaymentMethodModificatorId
            });
        }

        public async Task EliminarMetodoPago(int id, int idModificador)
        {
            await _repository.EliminarMetodoPagoAsync(id, idModificador);
        }
    }
}