using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatPilot.Data;
using PatPilot.Models;

namespace PatPilot.ViewComponents
{
    public class CommandeDetailsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CommandeDetailsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid commandeId)
        {
            var commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.commandeDetails)
                    .ThenInclude(d => d.Gateau)
                .FirstOrDefaultAsync(c => c.Id == commandeId);

            if (commande == null)
                return View("Empty");

            return View("Default", commande);
        }
    }
}
