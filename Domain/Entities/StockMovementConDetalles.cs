using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StockMovementConDetalles
    {
        public StockMovement Cabecera { get; set; } = new();
        public List<StockMovementDetalle> Detalles { get; set; } = new();
    }
}
