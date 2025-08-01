using System.ComponentModel.DataAnnotations;

namespace PatPilot.Models
{
    public class Evenement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Evenement")]
        public string Nom { get; set; } = null!;
        public string? Icone { get; set; } // peut contenir un nom de classe CSS ou un chemin d'image

        public ICollection<Gateau> Gateaux { get; set; } = new List<Gateau>();
    }
}
