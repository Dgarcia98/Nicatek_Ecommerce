using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IAttributeTypeService
    {
        Task<IEnumerable<AttributeTypeDTO>> ListarAttributesTypes();
        Task<AttributeTypeDTO?> ObtenerAttributeTypePorId(int id);
        Task<IEnumerable<AttributeTypeDTO>> ListarPorNombre(string buscar);
        Task NuevoAttributeType(AttributeTypeDTO dto);
        Task EditarAttributeType(AttributeTypeDTO dto);
        Task EliminarAttributeType(int id, int idModificador);
    }
}
