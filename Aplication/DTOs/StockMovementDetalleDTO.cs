using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementDetalleDTO
    {
        public int StockMovementDetailId { get; set; }
        public int StockMovementDetailMovementId { get; set; }
        public int? StockMovementDetailOrderDetailId { get; set; }
        public int? StockMovementDetailStockId { get; set; }
        public int StockMovementDetailQuantity { get; set; }
        public DateTime? StockMovementDetailFactoryDate { get; set; }
        public DateTime? StockMovementDetailExpirationDate { get; set; }
        public string? ProductVariableValue { get; set; }
        public string? ProductName { get; set; }
        public bool StockMovementDetailStatusId { get; set; }
    }
}
