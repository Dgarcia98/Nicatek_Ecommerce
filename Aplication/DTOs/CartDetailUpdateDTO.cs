using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CartDetailUpdateDTO
    {
        public int CartDetailId { get; set; }
        public int CartDetailQuantity { get; set; }
        public decimal CartDetailDiscount { get; set; }
        public decimal CartDetailSubTotal { get; set; }
        public decimal CartDetailTAX { get; set; }
        public decimal CartDetailTotal { get; set; }
        public int CartDetailModificatorId { get; set; }
    }
}
