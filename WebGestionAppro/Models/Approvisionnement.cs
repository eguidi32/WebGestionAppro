namespace WebGestionAppro.Models
{
    public enum StatutApprovisionnement
    {
        Recu,
        EnAttente,
        Annule
    }

    public class Approvisionnement
    {
        public int Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public DateTime DateApprovisionnement { get; set; }
        public int FournisseurId { get; set; }
        public decimal MontantTotal { get; set; }
        public StatutApprovisionnement Statut { get; set; } = StatutApprovisionnement.Recu;
        public string? Observations { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Navigation properties
        public Fournisseur Fournisseur { get; set; } = null!;
        public ICollection<ApprovisionnementArticle> ApprovisionnementArticles { get; set; } = new List<ApprovisionnementArticle>();
    }
}
