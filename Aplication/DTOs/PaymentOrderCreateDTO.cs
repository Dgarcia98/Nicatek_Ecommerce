using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class PaymentOrderCreateDTO
    {
        public int OrderUserId { get; set; }
        public int OrderDeliveryAddress { get; set; }
        public int OrderPaymentMethodId { get; set; }
        public decimal OrderSubtotal { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderShipping { get; set; }
        public decimal OrderTAX { get; set; }
        public decimal OrderTotal { get; set; }
        public int OrderCurrencyId { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderCreatorId { get; set; }
    }
}
