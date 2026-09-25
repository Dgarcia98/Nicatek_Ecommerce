using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class PaymentMethodTypeDTO
    {
        public int PaymentMethodTypeId { get; set; }
        public string? PaymentMethodTypeName { get; set; }
        public string? PaymentMethodTypeDescription { get; set; }
        public int PaymentMethodTypeCreatorId { get; set; }
        public DateTime? PaymentMethodTypeCreationDate { get; set; }
        public int? PaymentMethodTypeModificatorId { get; set; }
        public DateTime? PaymentMethodTypeModificationDate { get; set; }
        public bool PaymentMethodTypeStatusId { get; set; }
    }
}
