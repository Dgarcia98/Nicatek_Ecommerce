using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CurrencyDTO
    {
        public int CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencyISO { get; set; }
        public int CurrencyCode { get; set; }
        public string? CurrencyDescription { get; set; }
        public int CurrencyCreatorId { get; set; }
        public DateTime? CurrencyCreationDate { get; set; }
        public int? CurrencyModificatorId { get; set; }
        public DateTime? CurrencyModificationDate { get; set; }
        public bool CurrencyStatusId { get; set; }
    }
}
