using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductVariableUpdateDTO
    {
        public int ProductVariableId { get; set; }
        public int ProductVariableProductId { get; set; }
        public string? ProductVariableValue { get; set; }
        public decimal ProductVariablePrice { get; set; }
        public int ProductVariableCurrencyId { get; set; }
        public int ProductVariableModificatorId { get; set; }
    }
}
