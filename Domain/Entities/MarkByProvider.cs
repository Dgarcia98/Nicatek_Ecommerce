using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MarkByProviders
    {
        public int MarkByProviderId { get; set; }
        public int MarkByProviderMarkId { get; set; }
        public string? MarkName { get; set; }
        public int MarkByProviderProviderId { get; set; }
        public string? ProviderName { get; set; }
        public int MarkByProviderCreatorId { get; set; }
        public DateTime? MarkByProviderCreationDate { get; set; }
        public int? MarkByProviderModificatorId { get; set; }
        public DateTime? MarkByProviderModificationDate { get; set; }
        public bool MarkByProviderStatusId { get; set; }
    }
}
