using PatPilot.Models;
using PatPilot.Models.DTO;

namespace PatPilot.Repositories
{
    public interface IGateauRepository
    {
        Task<IEnumerable<Gateau>> GetAllByEnseigneAsync(Guid enseigneId);
        Task<IEnumerable<Gateau>> GetAllByEnseigneAndTypeAsync(Guid enseigneId, GateauxType t);
        Task<Gateau> GetByIdAsync(Guid id);
        Task AddAsync(Gateau gateau);
        Task UpdateAsync(Gateau gateau);
        Task DeleteAsync(Guid id);
    }
}
