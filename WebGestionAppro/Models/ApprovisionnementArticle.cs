namespace WebGestionAppro.Models
{
    // Table de liaison (Many-to-Many) entre Approvisionnement et Article
    public class ApprovisionnementArticle
    {
        public int Id { get; set; }
        public int ApprovisionnementId { get; set; }
        public int ArticleId { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal Montant { get; set; } // Quantite * PrixUnitaire

        // Navigation properties
        public Approvisionnement Approvisionnement { get; set; } = null!;
        public Article Article { get; set; } = null!;
    }
}
