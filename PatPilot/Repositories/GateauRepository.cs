using Microsoft.EntityFrameworkCore;
using PatPilot.Data;
using PatPilot.Models;

namespace PatPilot.Repositories
{
    public class GateauRepository : IGateauRepository
    {
        private readonly ApplicationDbContext _context;

        public GateauRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gateau>> GetAllByEnseigneAsync(Guid enseigneId)
        {
            return await _context.Gateaux.Include(g => g.Enseigne)
                                         .Where(g => g.EnseigneId == enseigneId)
                                         .ToListAsync();
        }

        public async Task<Gateau> GetByIdAsync(Guid id)
        {
            return await _context.Gateaux.Include(g => g.Enseigne)
                                         .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Gateau gateau)
        {
            await _context.Gateaux.AddAsync(gateau);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Gateau gateau)
        {
            _context.Gateaux.Update(gateau);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var gateau = await _context.Gateaux.FindAsync(id);
            if (gateau != null)
            {
                _context.Gateaux.Remove(gateau);
                await _context.SaveChangesAsync();
            }
        }
    }

}
