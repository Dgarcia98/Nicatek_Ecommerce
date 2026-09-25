using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CartDetailCreateDTO
    {
        public int CartDetailCartId { get; set; }
        public int CartDetailProductVariableId { get; set; }
        public decimal CartDetailPrice { get; set; }
        public int CartDetailQuantity { get; set; }
        public decimal CartDetailDiscount { get; set; }
        public decimal CartDetailSubTotal { get; set; }
        public decimal CartDetailTAX { get; set; }
        public decimal CartDetailTotal { get; set; }
        public int CartDetailCurrencyId { get; set; }
        public int CartDetailCreatorId { get; set; }
    }
}
