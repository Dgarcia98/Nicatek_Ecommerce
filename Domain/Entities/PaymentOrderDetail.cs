using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
        public class PaymentOrderDetail
        {
            public int OrderDetailId { get; set; }
            public int OrderDetailOrderId { get; set; }
            public int OrderDetailProductVariableId { get; set; }
            public string? ProductVariableValue { get; set; }
            public string? ProductName { get; set; }
            public decimal OrderDetailPrice { get; set; }
            public int OrderDetailQuantity { get; set; }
            public decimal OrderDetailDiscount { get; set; }
            public decimal OrderDetailSubTotal { get; set; }
            public decimal OrderDetailTAX { get; set; }
            public decimal OrderDetailTotal { get; set; }
            public int OrderDetailCurrencyId { get; set; }
            public string? CurrencyISO { get; set; }
            public int OrderDetailCreatorId { get; set; }
            public DateTime? OrderDetailCreationDate { get; set; }
            public int? OrderDetailModificatorId { get; set; }
            public DateTime? OrderDetailModificationDate { get; set; }
            public bool OrderDetailStatusId { get; set; }
        }
    
}
