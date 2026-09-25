using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementDTO
    {
        public int StockMovementId { get; set; }
        public int StockMovementType { get; set; }
        public string? StockMovementTypeName { get; set; }
        public int? StockMovementOrderId { get; set; }
        public string? StockMovementReference { get; set; }
        public DateTime StockMovementDate { get; set; }
        public int StockMovementStatusId { get; set; }
        public string? StatusName { get; set; }
        public int TotalUnidadesMovidas { get; set; }
        public int StockMovementCreatorId { get; set; }
        public DateTime? StockMovementCreationDate { get; set; }
        public int? StockMovementModifierId { get; set; }
        public DateTime? StockMovementModificationDate { get; set; }
    }
}
