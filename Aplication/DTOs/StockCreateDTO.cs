using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class StockCreateDTO
    {
        public int StockProductVariableId { get; set; }
        public int StockQuantity { get; set; }
        public DateTime StockFactoryDate { get; set; }
        public DateTime StockExpirationDate { get; set; }
        public int StockCreatorId { get; set; }
    }
}
