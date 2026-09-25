using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IMarkService
    {
        Task<IEnumerable<MarkDTO>> ListarMarks();
        Task<MarkDTO?> ObtenerMarkPorId(int id);
        Task<IEnumerable<MarkDTO>> ListarPorNombre(string buscar);
        Task NuevoMark(MarkDTO dto);
        Task EditarMark(MarkDTO dto);
        Task EliminarMark(int id, int idModificador);
    }
}
