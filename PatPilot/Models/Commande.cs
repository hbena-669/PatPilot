namespace PatPilot.Models
{
    using PatPilot.Models.DTO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Commande
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EnseigneId { get; set; }  // Lien avec l'enseigne

        [Required]
        public Guid ClientId { get; set; } // Lien avec le client

        [Required]
        public DateTime DateCommande { get; set; } = DateTime.UtcNow;

        [Required]
        public StatutCommande Statut { get; set; } = StatutCommande.EnAttente;  // Statut (En attente, Validée, Payée)

        public decimal Total { get; set; }  // Prix total

        // Relations
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [ForeignKey("EnseigneId")]
        public Enseigne Enseigne { get; set; }

        public List<Gateau> Gateaux { get; set; } = new List<Gateau>();
    }

}
