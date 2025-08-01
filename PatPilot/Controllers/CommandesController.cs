using Microsoft.AspNetCore.Mvc;
using PatPilot.Data;
using PatPilot.Extensions;
using PatPilot.Models;
using PatPilot.Models.DTO;
using PatPilot.Repositories;
using PatPilot.ViewModels;
using System.Security.Claims;
using MailKit.Net.Smtp;
using MimeKit;
using PatPilot.Helper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using PatPilot.Utils;

namespace PatPilot.Controllers
{
    public class CommandesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PanierRepository _panierRepository;

        public CommandesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _panierRepository = new PanierRepository(httpContextAccessor);
        }


        public IActionResult Index(Guid Id)
        {
            Enseigne enseigneSession;
            
            if(Id == Guid.Empty)
            {
                enseigneSession = _panierRepository.getEnseigneFromSession();
                Id = enseigneSession.Id;
            }
            
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            Enseigne enseigne = _context.Enseigne.Include(e => e.Gateaux).FirstOrDefault(e => e.Id == Id);
            if( enseigne == null)
            {
                HttpContext.Session.Remove(Constantes.S_client_client);
                HttpContext.Session.Remove(Constantes.S_client_id_client);
                return RedirectToAction("ChoisirEnseigne");
            }
            //Enregistrement de l'enseigne en session
            var enseigneJson = JsonSerializer.Serialize(_context.Enseigne.FirstOrDefault(m => m.Id == Id), options);
            HttpContext.Session.SetString(Constantes.S_enseigne_client, enseigneJson);
            
            
            if (enseigne == null)
            {
                return NotFound();
            }
            

            ViewBag.EnseigneName = enseigne.Name;
            ViewBag.EnseigneLogo = $"~/img/{enseigne.logo}" ;
            ViewBag.EnseigneAddr = enseigne.Address;
            ViewBag.EnseigneTel = enseigne.PhoneNumber;

            CommandeVM cmd = new CommandeVM();
            cmd.enseigne = enseigne;
            foreach (Gateau g in enseigne.Gateaux?.Where(g => g.GateauxType == GateauxType.Normal || g.GateauxType == null).ToList())
            {
                cmd.gateaux.Add(g);
            }
            foreach (Gateau g in enseigne.Gateaux?.Where(g => g.GateauxType == GateauxType.Special).ToList())
            {
                cmd.GateauxSpeciaux.Add(g);
            }
            cmd.evenements = _context.Evenement.ToList();
            //ViewBag.NombreGateaux = GetNombreGateauxDansPanier(); // Si tu veux afficher le nombre d'articles
            var panier = _panierRepository.GetPanier();
            panier.EnseigneId = enseigne.Id;
            HttpContext.Session.SetString("Panier", JsonSerializer.Serialize(panier));

            cmd.totalQuantite = 0;
            cmd.details = panier.CommandeDetailList;

            if (panier != null && panier.CommandeDetailList !=null)
            {
               foreach(CommandeDetail g in panier.CommandeDetailList)
                {
                    cmd.totalQuantite += g.Quantite;

                }

            }
            ViewBag.NbArticles = cmd.totalQuantite;
            return View(cmd);
        }

        public IActionResult ChoisirEnseigne()
        {
            var enseignes = _context.Enseigne.Where(e => e.IsActive == true).ToList(); // ou filtrées selon besoins
            return View(enseignes);
        }
        public IActionResult Panier()
        {
            var panier = _panierRepository.GetPanier();
            return View(panier);
        }

        [HttpPost]
        public IActionResult AjouterAuPanier(Guid gateauId, int quantite, Guid enseigneId)
        {
            _panierRepository.AjouterAuPanier(gateauId, quantite, enseigneId, _context);
            return RedirectToAction("Index", new { id = enseigneId });
        }

        [HttpPost]
        public IActionResult ModifierQuantite(Guid gateauId, int quantite)
        {
            _panierRepository.ModifierQuantite(gateauId, quantite);
            return RedirectToAction("Panier");
        }

        
        [HttpPost]
        public IActionResult ConfirmerCommande()
        {
            var panier = _panierRepository.GetPanier();
            if (!panier.CommandeDetailList.Any()) return RedirectToAction("Panier");

            var clientIdStr = HttpContext.Session.GetString(Constantes.S_client_id_client);
            if (string.IsNullOrEmpty(clientIdStr))
            {
                HttpContext.Session.SetString("ReturnUrl", "/Commandes/Panier");
                return RedirectToAction("RegisterClient", "ClientAccount");
            }

            Guid clientId = Guid.Parse(clientIdStr);

            var commande = new Commande
            {
                Id = Guid.NewGuid(),
                EnseigneId = panier.EnseigneId,
                ClientId = clientId,
                Statut = StatutCommande.EnAttente,
                commandeDetails = new List<CommandeDetail>()
            };

            foreach (var item in panier.CommandeDetailList)
            {
                commande.commandeDetails.Add(new CommandeDetail
                {
                    Id = Guid.NewGuid(),
                    GateauId = item.GateauId,
                    Quantite = item.Quantite,
                    CommandeId = commande.Id,
                    PrixUnitaire = item.PrixUnitaire,

                });
            }

            commande.Total = commande.commandeDetails.Sum(d => d.SousTotal);
            if(commande.Commentaire == null) commande.Commentaire = string.Empty;

            _context.Commandes.Add(commande);
            _context.SaveChanges();
            _panierRepository.ViderPanier();

            /*
            // Générer le PDF
            var pdfBytes = PdfHelper.GenererFacturePdf(commande, client, commande.Details);

            // Envoi de l'email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Votre App", "no-reply@votreapp.com"));
            message.To.Add(new MailboxAddress("Admin", enseigne.Email));
            message.Subject = $"Nouvelle commande #{commande.Id}";

            var builder = new BodyBuilder
            {
                TextBody = $"Bonjour, une nouvelle commande a été passée par {client.Prenom} {client.Nom}. Voir le PDF en pièce jointe."
            };

            builder.Attachments.Add($"Commande_{commande.Id}.pdf", pdfBytes, new ContentType("application", "pdf"));
            message.Body = builder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false); // Modifier selon ton provider
                smtp.Authenticate("ton-email@gmail.com", "ton-mdp");
                smtp.Send(message);
                smtp.Disconnect(true);
            }
            */
            TempData["Confirmation"] = "Votre commande a bien été enregistrée !";
            return RedirectToAction("Confirmation", new { id = commande.Id });
        }

        public IActionResult Confirmation(Guid id)
        {
            return View(model: id); // On passe simplement l'ID, le ViewComponent fera le reste
        }

        [HttpGet]
        public IActionResult GetGateauxByEvenement(Guid id, Guid enseigneId)
        {
            var gateaux = _context.Gateaux
                .Where(g => g.EventID == id && g.GateauxType == GateauxType.Special && g.EnseigneId == enseigneId)
                .ToList();

            return PartialView("_GateauxSpeciaux", gateaux);
        }

    }

}
