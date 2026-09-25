using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class SegmentService : ISegmentService
    {
        private readonly ISegmentRepository _repository;

        public SegmentService(ISegmentRepository repository)
        {
            _repository = repository;
        }

        private SegmentDTO ToDTO(Segments item) => new SegmentDTO
        {
            SegmentId = item.SegmentId,
            SegmentName = item.SegmentName,
            SegmentDescription = item.SegmentDescription,
            SegmentCreatorId = item.SegmentCreatorId,
            SegmentCreationDate = item.SegmentCreationDate,
            SegmentModificatorId = item.SegmentModificatorId,
            SegmentModificationDate = item.SegmentModificationDate,
            SegmentStatusId = item.SegmentStatusId
        };

        public async Task<IEnumerable<SegmentDTO>> ListarSegments(bool soloActivos = true)
        {
            var lista = await _repository.ListarSegmentsAsync(soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<SegmentDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<SegmentDTO>();

            var lista = await _repository.ListarSegmentsFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<SegmentDTO?> ObtenerSegmentPorId(int id)
        {
            var lista = await _repository.ListarSegmentsAsync(false);
            var item = lista.FirstOrDefault(x => x.SegmentId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoSegment(SegmentDTO dto)
        {
            await _repository.NuevoSegmentAsync(new Segments
            {
                SegmentName = dto.SegmentName,
                SegmentDescription = dto.SegmentDescription,
                SegmentCreatorId = dto.SegmentCreatorId
            });
        }

        public async Task EditarSegment(SegmentDTO dto)
        {
            await _repository.EditarSegmentAsync(new Segments
            {
                SegmentId = dto.SegmentId,
                SegmentName = dto.SegmentName,
                SegmentDescription = dto.SegmentDescription,
                SegmentModificatorId = dto.SegmentModificatorId ?? dto.SegmentCreatorId
            });
        }

        public async Task EliminarSegment(int id, int idModificador)
        {
            await _repository.EliminarSegmentAsync(id, idModificador);
        }
    }
}