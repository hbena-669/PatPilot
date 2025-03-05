namespace PatPilot.Models
{
    public class CommandeDetails
    {
        public Guid Id { get; set; }
        public Guid CommandeId { get; set; } // Référence à la commande
        public Guid GateauId { get; set; } // Référence au gâteau
        public int Quantite { get; set; } // Quantité commandée
        public decimal PrixUnitaire { get; set; }
        public decimal SousTotal  => Quantite * PrixUnitaire;// Calcul du sous-total
    }

}
