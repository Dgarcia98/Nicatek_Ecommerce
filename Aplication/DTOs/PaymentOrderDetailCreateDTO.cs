using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class PaymentOrderDetailCreateDTO
    {
        public int OrderDetailOrderId { get; set; }
        public int OrderDetailProductVariableId { get; set; }
        public decimal OrderDetailPrice { get; set; }
        public int OrderDetailQuantity { get; set; }
        public decimal OrderDetailDiscount { get; set; }
        public decimal OrderDetailSubTotal { get; set; }
        public decimal OrderDetailTAX { get; set; }
        public decimal OrderDetailTotal { get; set; }
        public int OrderDetailCurrencyId { get; set; }
        public int OrderDetailCreatorId { get; set; }
    }
}
