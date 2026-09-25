using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class PaymentOrderFilterDTO
    {
        public string? Filtro { get; set; }
        public int? OrderStatusId { get; set; }
    }
}
