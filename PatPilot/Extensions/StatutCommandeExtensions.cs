// Extensions/StatutCommandeExtensions.cs
using PatPilot.Models.DTO;

namespace PatPilot.Extensions
{
    public static class StatutCommandeExtensions
    {
        public static string ToFriendlyString(this StatutCommande statut)
        {
            return statut switch
            {
                StatutCommande.EnAttente => "En Attente",
                StatutCommande.EnPreparation => "En Preparation",
                StatutCommande.Livree => "Livrée",
                StatutCommande.Annulee => "Annulée",
                StatutCommande.Expediee => "Expediée",
                StatutCommande.Validee => "Validée",
                StatutCommande.Payee => "Payée",
                _ => "Inconnu"
            };
        }
    }
}
