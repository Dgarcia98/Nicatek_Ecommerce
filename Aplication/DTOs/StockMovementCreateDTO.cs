using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementCreateDTO
    {
        public int StockMovementType { get; set; }
        public int? StockMovementOrderId { get; set; }
        public string? StockMovementReference { get; set; }
        public DateTime StockMovementDate { get; set; }
        public int StockMovementCreatorId { get; set; }
        public int StockMovementStatusId { get; set; }
    }
}
