using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UserPaymentMethodUpdateDTO
    {
        public int UserPaymentMethodId { get; set; }
        public int UserPaymentMethodPaymentMethodTypeId { get; set; }
        public string? ExpirationDate { get; set; }    // texto plano → se cifra en servicio
        public string? UserPaymentMethodCardHolderName { get; set; }
        public int UserPaymentMethodModificatorId { get; set; }
    }
}
