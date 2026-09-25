using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISegmentRepository
    {
        Task<IEnumerable<Segments>> ListarSegmentsAsync(bool soloActivos = true);
        Task<IEnumerable<Segments>> ListarSegmentsFiltroAsync(string filtro);
        Task NuevoSegmentAsync(Segments oSegment);
        Task EditarSegmentAsync(Segments oSegment);
        Task EliminarSegmentAsync(int id, int idModificador);
    }
}
