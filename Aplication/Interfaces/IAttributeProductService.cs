using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IAttributeProductService
    {
        Task<IEnumerable<AttributeProductDTO>> Listar(int? productId = null);
        Task<AttributeProductDTO?> ObtenerPorId(int id);
        Task<IEnumerable<AttributeProductDTO>> Filtrar(string buscar);
        Task NuevoAtributo(AttributeProductCreateDTO dto);
        Task EditarAtributo(AttributeProductUpdateDTO dto);
        Task EliminarAtributo(int id, int idModificador);
    }
}
