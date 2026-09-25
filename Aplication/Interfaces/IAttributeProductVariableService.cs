using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface IAttributeProductVariableService
    {
        Task<IEnumerable<AttributeProductVariableDTO>> Listar(int? productVariableId = null);
        Task<IEnumerable<AttributeProductVariableDTO>> Filtrar(string buscar);
        Task NuevoAtributoVariable(AttributeProductVariableCreateDTO dto);
        Task EditarAtributoVariable(AttributeProductVariableUpdateDTO dto);
        Task EliminarAtributoVariable(int id, int idModificador);
    }
}