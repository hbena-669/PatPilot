namespace PatPilot.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; }

        [Required]
        public Guid EnseigneId { get; set; }  // Lien avec l'enseigne

        [Required, MaxLength(50)]
        public string Nom { get; set; }

        [Required, MaxLength(50)]
        public string Prenom { get; set; }

        [Required, MaxLength(255)]
        public string Adresse { get; set; }

        [Required, MaxLength(20)]
        public string Telephone { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string MotDePasse { get; set; } // Hashé avant stockage

        // Relation avec l'enseigne
        [ForeignKey("EnseigneId")]
        public Enseigne Enseigne { get; set; }
        public List<Commande> Commandes { get; set; } = new List<Commande>();
    }



}
