namespace PatPilot.Models
{
    using Microsoft.EntityFrameworkCore;
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
        public DateTime DateLivraison { get; set; } 

        public string Commentaire { get; set; }
        [Required]
        public StatutCommande Statut { get; set; } = StatutCommande.EnAttente;  // Statut (En attente, Validée, Payée)

        [Precision(18, 2)] // Définit la précision (18 chiffres, 2 après la virgule)
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }  // Prix total

        // Relations
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [ForeignKey("EnseigneId")]
        public Enseigne Enseigne { get; set; }

        public List<CommandeDetail> commandeDetails { get; set; } = new List<CommandeDetail>();
    }

}
