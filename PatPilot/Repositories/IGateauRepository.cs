using PatPilot.Models;

namespace PatPilot.Repositories
{
    public interface IGateauRepository
    {
        Task<IEnumerable<Gateau>> GetAllByEnseigneAsync(Guid enseigneId);
        Task<Gateau> GetByIdAsync(Guid id);
        Task AddAsync(Gateau gateau);
        Task UpdateAsync(Gateau gateau);
        Task DeleteAsync(Guid id);
    }
}
