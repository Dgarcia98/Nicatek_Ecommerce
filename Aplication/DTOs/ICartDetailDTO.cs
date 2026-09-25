using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CartDetailDTO
    {
        public int CartDetailId { get; set; }
        public int CartDetailCartId { get; set; }
        public int CartDetailProductVariableId { get; set; }
        public string? ProductVariableValue { get; set; }
        public string? ProductName { get; set; }
        public decimal CartDetailPrice { get; set; }
        public int CartDetailQuantity { get; set; }
        public decimal CartDetailDiscount { get; set; }
        public decimal CartDetailSubTotal { get; set; }
        public decimal CartDetailTAX { get; set; }
        public decimal CartDetailTotal { get; set; }
        public int CartDetailCurrencyId { get; set; }
        public string? CurrencyISO { get; set; }
        public int CartDetailCreatorId { get; set; }
        public DateTime? CartDetailCreationDate { get; set; }
        public int? CartDetailModificatorId { get; set; }
        public DateTime? CartDetailModificationDate { get; set; }
        public bool CartDetailStatusId { get; set; }
    }
}
