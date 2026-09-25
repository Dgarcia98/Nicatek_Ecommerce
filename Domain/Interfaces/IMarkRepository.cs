using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMarkRepository
    {
        Task<IEnumerable<Marks>> ListarMarksAsync();
        Task<IEnumerable<Marks>> ListarMarksFiltroAsync(string filtro);
        Task NuevoMarkAsync(Marks oMark);
        Task EditarMarkAsync(Marks oMark);
        Task EliminarMarkAsync(int id, int idModificador);
    }
}
