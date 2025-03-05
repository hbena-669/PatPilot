using System.ComponentModel.DataAnnotations;
namespace PatPilot.ViewModels
{
    public class EnseigneWM
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; } = Guid.NewGuid(); // Identifiant unique

        [StringLength(100, MinimumLength = 2)]
        [Required(ErrorMessage = "Entrez le Nom de l'enseigne.")]
        [Display(Name = "Nom de l'enseigne")]
        public string Name { get; set; } // Nom de l'enseigne

        [Required(ErrorMessage = "Entrez l'adresse de l'enseigne.")]
        [Display(Name = "Adresse de l'enseigne")]
        public string Address { get; set; } // Adresse

        [Required(ErrorMessage = "Entrez le numéro de téléphone")]
        [Display(Name = "Téléphone")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email Admin")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Mot de pass Admin")]
        public string Password { get; set; }

        [Display(Name = "Contact de l'enseigne")]
        public string ContactName { get; set; }

        public string logo { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        public DateTime DateActivation { get; set; }


        [Display(Name = "Devise")]
        public string Currency { get; set; }
    }
}
