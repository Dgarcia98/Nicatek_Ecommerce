using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementUpdateDTO
    {
        public int StockMovementId { get; set; }
        public string? StockMovementReference { get; set; }
        public DateTime StockMovementDate { get; set; }
        public int StockMovementStatusId { get; set; }
        public int StockMovementModifierId { get; set; }
    }
}
