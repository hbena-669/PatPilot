namespace PatPilot.Models.DTO
{
    public enum StatutCommande
    {
        EnAttente,  // Commande en attente de validation
        Validee,    // Commande validée par l'enseigne
        Payee,      // Commande payée par le client
        EnPreparation, // Commande en cours de préparation
        Expediee,   // Commande envoyée
        Livree,     // Commande livrée
        Annulee     // Commande annulée
    }

    public enum GateauxType
    {
        Normal,
        Special
    }
}
