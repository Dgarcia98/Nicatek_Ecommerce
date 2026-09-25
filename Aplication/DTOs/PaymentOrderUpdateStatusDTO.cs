using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class PaymentOrderUpdateStatusDTO
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderModificatorId { get; set; }
    }
}
