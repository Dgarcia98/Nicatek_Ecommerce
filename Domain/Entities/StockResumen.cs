using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StockResumen
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int ProductVariableId { get; set; }
        public string? ProductVariableValue { get; set; }
        public string? CurrencyISO { get; set; }
        public decimal ProductVariablePrice { get; set; }
        public int StockTotal { get; set; }
        public int StockVencido { get; set; }
        public int StockPorVencer { get; set; }
        public int StockVigente { get; set; }
        public DateTime? ProximoVencimiento { get; set; }
    }
}
