using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementDetailCreateDTO
    {
        public int StockMovementDetailMovementId { get; set; }
        public int? StockMovementDetailOrderDetailId { get; set; }
        public int? StockMovementDetailStockId { get; set; }
        public int StockMovementDetailQuantity { get; set; }
        public DateTime? StockMovementDetailFactoryDate { get; set; }
        public DateTime? StockMovementDetailExpirationDate { get; set; }
        public int StockMovementDetailCreatorId { get; set; }
    }
}
