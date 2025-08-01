using System.ComponentModel.DataAnnotations;

namespace PatPilot.Models
{
    public class RegisterClientViewModel
    {
        [Required]
        [Display(Name = "Nom")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Téléphone")]
        public string Tel { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Adresse")]
        public string Adresse { get; set; }

        public int? EnseigneId { get; set; } // si besoin
    }

}
