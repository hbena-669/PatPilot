using PatPilot.Models;
using PatPilot.Repositories;

namespace PatPilot.ViewModels
{
    public class CommandeVM
    {
        public Enseigne enseigne { get; set; } // Infos de l'enseigne
        public List<Gateau> gateaux { get; set; } = new List<Gateau>(); // Gâteaux de l’enseigne
        public List<Gateau> GateauxSpeciaux { get; set; } = new List<Gateau>(); // Gâteaux de l’enseigne
        public List<CommandeDetail> details { get; set; } = new List<CommandeDetail>(); // Contenu du panier
        
        public List<Evenement> evenements { get; set; }
        public int totalQuantite { get; set; } // Nombre total d’articles
        
    }
}
