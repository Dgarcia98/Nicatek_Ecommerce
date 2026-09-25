using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Stock
    {
        public int StockId { get; set; }
        public int StockProductVariableId { get; set; }
        public string? ProductVariableValue { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? MarkName { get; set; }
        public string? ProviderName { get; set; }
        public int StockQuantity { get; set; }
        public DateTime StockFactoryDate { get; set; }
        public DateTime StockExpirationDate { get; set; }
        public bool StockProximoVencer { get; set; }
        public bool StockVencido { get; set; }
        public int StockCreatorId { get; set; }
        public DateTime? StockCreationDate { get; set; }
        public int? StockModificatorId { get; set; }
        public DateTime? StockModificationDate { get; set; }
        public bool StockStatusId { get; set; }
    }
}
