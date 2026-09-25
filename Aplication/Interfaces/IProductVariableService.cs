using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IProductVariableService
    {
        Task ActualizarDescuento(int productVariableId, decimal descuento, DateTime? hasta, int modificadorId);
        Task<IEnumerable<ProductVariableDTO>> Listar(int? productId = null, bool soloActivos = true);
        Task<ProductVariableDTO?> ObtenerPorId(int id);
        Task<IEnumerable<ProductVariableDTO>> Filtrar(string buscar);
        Task NuevaVariable(ProductVariableCreateDTO dto);
        Task EditarVariable(ProductVariableUpdateDTO dto);
        Task EliminarVariable(int id, int idModificador);
    }
}
