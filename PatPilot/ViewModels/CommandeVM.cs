using PatPilot.Models;

namespace PatPilot.ViewModels
{
    public class CommandeVM
    {
        public Guid EnseigneId { get; set; }
        public List<CommandeDetails> Details { get; set; } = new List<CommandeDetails>();
        public Client Client { get; set; }
        public decimal Total => Details.Sum(d => d.SousTotal);
        public int NombreTotalGateaux => Details.Sum(d => d.Quantite); // Nombre total de gâteaux commandés
    }
}
