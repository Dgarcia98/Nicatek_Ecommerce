using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository _repository;

        public MarkService(IMarkRepository repository)
        {
            _repository = repository;
        }

        private MarkDTO ToDTO(Marks item) => new MarkDTO
        {
            MarkId = item.MarkId,
            MarkName = item.MarkName,
            MarkDescription = item.MarkDescription,
            MarkCreatorId = item.MarkCreatorId,
            MarkCreationDate = item.MarkCreationDate,
            MarkModificatorId = item.MarkModificatorId,
            MarkModificationDate = item.MarkModificationDate,
            MarkStatusId = item.MarkStatusId
        };

        public async Task<IEnumerable<MarkDTO>> ListarMarks()
        {
            var lista = await _repository.ListarMarksAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<MarkDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<MarkDTO>();

            var lista = await _repository.ListarMarksFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<MarkDTO?> ObtenerMarkPorId(int id)
        {
            var lista = await _repository.ListarMarksFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.MarkId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoMark(MarkDTO dto)
        {
            await _repository.NuevoMarkAsync(new Marks
            {
                MarkName = dto.MarkName,
                MarkDescription = dto.MarkDescription,
                MarkCreatorId = dto.MarkCreatorId
            });
        }

        public async Task EditarMark(MarkDTO dto)
        {
            await _repository.EditarMarkAsync(new Marks
            {
                MarkId = dto.MarkId,
                MarkName = dto.MarkName,
                MarkDescription = dto.MarkDescription,
                MarkModificatorId = dto.MarkModificatorId ?? dto.MarkCreatorId
            });
        }

        public async Task EliminarMark(int id, int idModificador)
        {
            await _repository.EliminarMarkAsync(id, idModificador);
        }
    }
}