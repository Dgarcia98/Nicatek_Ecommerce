using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementDetailUpdateDTO
    {
        public int StockMovementDetailId { get; set; }
        public DateTime? StockMovementDetailFactoryDate { get; set; }
        public DateTime? StockMovementDetailExpirationDate { get; set; }
        public int StockMovementDetailModifierId { get; set; }
    }
}
