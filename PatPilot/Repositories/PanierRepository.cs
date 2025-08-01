using PatPilot.Data;
using PatPilot.Models;
using PatPilot.Utils;
using System.Security.AccessControl;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace PatPilot.Repositories
{
    public class PanierRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKey = "Panier";
        private const string EnseigneKey = Constantes.S_enseigne_client;

        public PanierRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public EnsPanier GetPanier()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var panierJson = session.GetString(SessionKey);
            return string.IsNullOrEmpty(panierJson) ? new EnsPanier() : JsonSerializer.Deserialize<EnsPanier>(panierJson);
        }

        public int getNbArticle()
        {
            return GetPanier().nbArticles;
        }

        public Enseigne? getEnseigneFromSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var EnseigneJson = session.GetString(EnseigneKey);
            return string.IsNullOrEmpty(EnseigneJson) ? new Enseigne() : JsonSerializer.Deserialize<Enseigne>(EnseigneJson);
        }
        public void AjouterAuPanier(Guid gateauId, int quantite,Guid enseigneId, ApplicationDbContext _context)
        {
            var panier = GetPanier();
            var gateau = _context.Gateaux.Find(gateauId);
            if (gateau == null) return;
            if (panier.CommandeDetailList == null) { panier.CommandeDetailList = new List<CommandeDetail>(); }
            var item = panier.CommandeDetailList.FirstOrDefault(p => p.GateauId == gateauId);
            if (item != null)
            {
                item.Quantite += quantite;
                panier.nbArticles += 1;
                //item.PrixUnitaire = gateau.Price;
                //item.SousTotal = gateau.Price * quantite;
            }
            else
                panier.CommandeDetailList.Add(new CommandeDetail { GateauId = gateauId, Quantite = quantite, Gateau = gateau, PrixUnitaire = gateau.Price });

            panier.EnseigneId = enseigneId;
            
            _httpContextAccessor.HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(panier));
        }

        public void ModifierQuantite(Guid gateauId, int quantite)
        {
            var panier = GetPanier();
            var item = panier.CommandeDetailList.FirstOrDefault(p => p.GateauId == gateauId);
            if (item != null) 
            {
                item.Quantite = quantite;
                if (quantite == 0)
                {
                    panier.CommandeDetailList.Remove(item);
                }
            }
            
            _httpContextAccessor.HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(panier));
        }

        public void ViderPanier()
        {
            _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
        }
    }

}
