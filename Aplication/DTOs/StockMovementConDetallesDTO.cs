using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockMovementConDetallesDTO
    {
        public StockMovementDTO Cabecera { get; set; } = new();
        public List<StockMovementDetalleDTO> Detalles { get; set; } = new();
    }
}
