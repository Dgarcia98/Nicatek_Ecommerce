using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    // Usado para actualización de datos generales
    public class UserUpdateDTO
    {
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int UserCountryId { get; set; }
        public int UserGenderId { get; set; }
        public DateTime UserBirthDay { get; set; }
        public int UserModificatorId { get; set; }
    }
}
