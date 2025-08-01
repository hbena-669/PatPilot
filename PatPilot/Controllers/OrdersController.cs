using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatPilot.Data;
using PatPilot.Extensions;
using PatPilot.Models;
using PatPilot.Models.DTO;
using PatPilot.Utils;
using PatPilot.ViewModels;

namespace PatPilot.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Orders(
                StatutCommande? statut = null,
                string searchNom = null,
                DateTime? dateDebut = null,
                DateTime? dateFin = null,
                string sortOrder = "desc")
        {
            var now = DateTime.Now.Date;

            // Initialisation automatique si aucun filtre date
            if (dateDebut == null && dateFin == null)
            {
                dateDebut = now.AddDays(-1); // Hier
                dateFin = now.AddDays(1);    // Demain à minuit exclu
            }

            var commandes = _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.commandeDetails)
                .AsQueryable();

            if (statut != null)
                commandes = commandes.Where(c => c.Statut == statut);

            if (!string.IsNullOrWhiteSpace(searchNom))
                commandes = commandes.Where(c => c.Client.Nom.Contains(searchNom));

            if (dateDebut.HasValue)
                commandes = commandes.Where(c => c.DateCommande >= dateDebut.Value);

            if (dateFin.HasValue)
                commandes = commandes.Where(c => c.DateCommande <= dateFin.Value);

            commandes = sortOrder == "asc"
                ? commandes.OrderBy(c => c.DateCommande)
                : commandes.OrderByDescending(c => c.DateCommande);

            ViewBag.SelectedStatut = statut;
            ViewBag.SearchNom = searchNom;
            ViewBag.DateDebut = dateDebut?.ToString("yyyy-MM-dd");
            ViewBag.DateFin = dateFin?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;

            return View(commandes.ToList());
        }


        public IActionResult Details(Guid id)
        {
            var enseigne = HttpContext.Session.GetObjectFromJson<Enseigne>(Constantes.S_enseigne_admin);
            var commande = _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.commandeDetails)
                .ThenInclude(cd => cd.Gateau)
                .FirstOrDefault(c => c.Id == id);

            return ViewComponent("CommandeDetailAdmin", new { commande });
        }


        [HttpPost]
        public IActionResult UpdateCommande(Guid id, DateTime DateLivraison, StatutCommande Statut)
        {
            var commande = _context.Commandes.FirstOrDefault(c => c.Id == id);
            if (commande == null) return NotFound();

            commande.DateLivraison = DateLivraison;
            commande.Statut = Statut;
            _context.SaveChanges();

            TempData["Success"] = "Commande mise à jour avec succès.";
            return RedirectToAction("Details", new { id });
        }

    }

}
