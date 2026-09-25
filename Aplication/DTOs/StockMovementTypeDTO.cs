using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementTypeDTO
    {
        public int StockMovementTypeId { get; set; }
        public string? StockMovementTypeName { get; set; }
        public string? StockMovementTypeDescription { get; set; }
        public int StockMovementTypeCreatorId { get; set; }
        public DateTime? StockMovementTypeCreationDate { get; set; }
        public int? StockMovementTypeModificatorId { get; set; }
        public DateTime? StockMovementTypeModificationDate { get; set; }
        public bool StockMovementTypeStatusId { get; set; }
    }
}
