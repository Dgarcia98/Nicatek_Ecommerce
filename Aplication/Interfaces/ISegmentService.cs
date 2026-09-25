using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface ISegmentService
    {
        Task<IEnumerable<SegmentDTO>> ListarSegments(bool soloActivos = true);
        Task<SegmentDTO?> ObtenerSegmentPorId(int id);
        Task<IEnumerable<SegmentDTO>> ListarPorNombre(string buscar);
        Task NuevoSegment(SegmentDTO dto);
        Task EditarSegment(SegmentDTO dto);
        Task EliminarSegment(int id, int idModificador);
    }
}
