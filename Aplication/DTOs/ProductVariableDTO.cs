using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductVariableDTO
    {
        public int ProductVariableId { get; set; }
        public int ProductVariableProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductVariableValue { get; set; }
        public decimal ProductVariablePrice { get; set; }
        public int ProductVariableCurrencyId { get; set; }
        public string? CurrencyISO { get; set; }
        public string? CurrencyName { get; set; }
        public int StockDisponible { get; set; }

        /// <summary>Tipo de la variante: Color, Talla, Memoria… Null si no tiene.</summary>
        public string? VariableTypeName { get; set; }
        public int ProductVariableCreatorId { get; set; }
        public DateTime? ProductVariableCreationDate { get; set; }
        public int? ProductVariableModificatorId { get; set; }
        public DateTime? ProductVariableModificationDate { get; set; }
        public bool ProductVariableStatusId { get; set; }
        public decimal ProductVariableDiscount { get; set; }
    }
}
