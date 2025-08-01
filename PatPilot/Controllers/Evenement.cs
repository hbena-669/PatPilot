using Microsoft.AspNetCore.Mvc;
using PatPilot.Data;
using PatPilot.Models;

namespace PatPilot.Controllers
{
    public class EvenementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvenementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var evenements = _context.Evenement.ToList();
            return View(evenements);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEvenement(Guid? id, string nom, string icone)
        {
            if (string.IsNullOrWhiteSpace(nom))
            {
                ModelState.AddModelError("Nom", "Le nom est requis.");
                return RedirectToAction("Index");
            }

            Evenement evt;
            if (id.HasValue && id != Guid.Empty)
            {
                evt = await _context.Evenement.FindAsync(id.Value);
                if (evt == null) return NotFound();

                evt.Nom = nom;
                evt.Icone = icone;
                _context.Evenement.Update(evt);
            }
            else
            {
                evt = new Evenement
                {
                    Nom = nom,
                    Icone = icone
                };
                _context.Evenement.Add(evt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

}
