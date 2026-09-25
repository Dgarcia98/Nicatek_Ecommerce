using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Permissions
    {
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionDescription { get; set; }
        public string? PermissionModule { get; set; }
        public int PermissionCreatorId { get; set; }
        public DateTime? PermissionCreationDate { get; set; }
        public int? PermissionModificatorId { get; set; }
        public DateTime? PermissionModificationDate { get; set; }
        public bool PermissionStatusId { get; set; }
    }
}