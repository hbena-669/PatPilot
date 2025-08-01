using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PatPilot.Models
{
    public class CommandeDetail
    {
        public Guid Id { get; set; }
        public Guid CommandeId { get; set; } // Référence à la commande
        public Commande Commande { get; set; }
        public Guid GateauId { get; set; } // Référence au gâteau
        
        public Gateau Gateau { get; set; }
        public int Quantite { get; set; } // Quantité commandée

        [Precision(18, 2)] // Définit la précision (18 chiffres, 2 après la virgule)
        [DataType(DataType.Currency)]
        public decimal PrixUnitaire { get; set; }

        [Precision(18, 2)] // Définit la précision (18 chiffres, 2 après la virgule)
        [DataType(DataType.Currency)]
        public decimal SousTotal  => Quantite * PrixUnitaire;// Calcul du sous-total
    }

}
