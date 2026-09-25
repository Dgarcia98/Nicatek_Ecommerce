using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserPaymentMethod
    {
        public int UserPaymentMethodId { get; set; }
        public int UserPaymentMethodUserId { get; set; }
        public string? UserName { get; set; }
        public int UserPaymentMethodPaymentMethodTypeId { get; set; }
        public string? PaymentMethodTypeName { get; set; }
        public byte[]? UserPaymentMethodCardNumber { get; set; }
        public byte[]? UserPaymentMethodExpirationDate { get; set; }
        public byte[]? UserPaymentMethodCVV { get; set; }
        public string? UserPaymentMethodCardHolderName { get; set; }
        public int UserPaymentMethodCreatorId { get; set; }
        public DateTime? UserPaymentMethodCreationDate { get; set; }
        public int? UserPaymentMethodModificatorId { get; set; }
        public DateTime? UserPaymentMethodModificationDate { get; set; }
        public bool UserPaymentMethodStatusId { get; set; }
    }
}
