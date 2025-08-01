namespace PatPilot.Models
{
    public class EnsPanier
    {
        public Guid EnseigneId { get; set; }
        public Guid CommandeId { get; set; }

        public int nbArticles { get; set; }

        public List<CommandeDetail> CommandeDetailList { get; set; }
    }
}
