using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UserPaymentMethodDTO
    {
        public int UserPaymentMethodId { get; set; }
        public int UserPaymentMethodUserId { get; set; }
        public string? UserName { get; set; }
        public int UserPaymentMethodPaymentMethodTypeId { get; set; }
        public string? PaymentMethodTypeName { get; set; }
        public string? UserPaymentMethodCardHolderName { get; set; }
        public string? CardNumberMasked { get; set; }        // ej: **** **** **** 1234
        public string? ExpirationDateDecrypted { get; set; } // descifrado en la capa app
        public int UserPaymentMethodCreatorId { get; set; }
        public DateTime? UserPaymentMethodCreationDate { get; set; }
        public int? UserPaymentMethodModificatorId { get; set; }
        public DateTime? UserPaymentMethodModificationDate { get; set; }
        public bool UserPaymentMethodStatusId { get; set; }
    }
}
