using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementDetailDTO
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
        public string? MarkName { get; set; }
        public string? CurrencyISO { get; set; }
        public decimal? ProductVariablePrice { get; set; }
        public int StockMovementDetailCreatorId { get; set; }
        public DateTime? StockMovementDetailCreationDate { get; set; }
        public int? StockMovementDetailModifierId { get; set; }
        public DateTime? StockMovementDetailModificationDate { get; set; }
        public bool StockMovementDetailStatusId { get; set; }
    }
}
