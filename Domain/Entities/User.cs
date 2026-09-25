using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int UserCountryId { get; set; }
        public int UserGenderId { get; set; }
        public DateTime UserBirthDay { get; set; }
        public int UserCreatorId { get; set; }
        public DateTime UserCreationDate { get; set; }
        public int? UserModificatorId { get; set; }
        public DateTime? UserModificationDate { get; set; }
        public int UserStatusId { get; set; }
    }
}
