using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Providers
    {
        public int ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public string? ProviderDescription { get; set; }
        public int ProviderCreatorId { get; set; }
        public DateTime? ProviderCreationDate { get; set; }
        public int? ProviderModificatorId { get; set; }
        public DateTime? ProviderModificationDate { get; set; }
        public bool ProviderStatusId { get; set; }
    }
}
