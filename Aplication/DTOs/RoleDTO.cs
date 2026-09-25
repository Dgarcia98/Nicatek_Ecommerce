using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public int RoleCreatorId { get; set; }
        public DateTime? RoleCreationDate { get; set; }
        public int? RoleModificatorId { get; set; }
        public DateTime? RoleModificationDate { get; set; }
        public bool RoleStatusId { get; set; }
    }
}
