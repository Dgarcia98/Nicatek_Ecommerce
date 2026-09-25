using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{     // Usado exclusivamente para la inserción
    public class UserCreateDTO
    {
        public string? UserFullName { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; } // Viaja en texto plano seguro hacia el SP
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int UserCountryId { get; set; }
        public int UserGenderId { get; set; }
        public DateTime UserBirthDay { get; set; }
        public int UserCreatorId { get; set; }
    }
}
