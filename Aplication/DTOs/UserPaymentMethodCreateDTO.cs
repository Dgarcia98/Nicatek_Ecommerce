using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UserPaymentMethodCreateDTO
    {
        public int UserPaymentMethodUserId { get; set; }
        public int UserPaymentMethodPaymentMethodTypeId { get; set; }
        public string? CardNumber { get; set; }        // texto plano → se cifra en servicio
        public string? ExpirationDate { get; set; }    // texto plano → se cifra en servicio
        public string? CVV { get; set; }               // texto plano → se cifra en servicio
        public string? UserPaymentMethodCardHolderName { get; set; }
        public int UserPaymentMethodCreatorId { get; set; }
    }
}
