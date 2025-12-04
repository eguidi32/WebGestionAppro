namespace WebGestionAppro.Models.ViewModels
{
    public class ApprovisionnementCreateViewModel
    {
        public DateTime DateApprovisionnement { get; set; } = DateTime.Now;
        public int FournisseurId { get; set; }
        public string? Reference { get; set; }
        public string? Observations { get; set; }
        public List<ApprovisionnementArticleItem> Articles { get; set; } = new();
        
        // Listes pour les dropdowns
        public List<Fournisseur> Fournisseurs { get; set; } = new();
        public List<Article> ArticlesDisponibles { get; set; } = new();
    }

    public class ApprovisionnementArticleItem
    {
        public int ArticleId { get; set; }
        public string ArticleNom { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal Montant { get; set; }
    }

    public class ApprovisionnementListViewModel
    {
        public List<Approvisionnement> Approvisionnements { get; set; } = new();
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int? FournisseurId { get; set; }
        public int? ArticleId { get; set; }
        public List<Fournisseur> Fournisseurs { get; set; } = new();
        public List<Article> Articles { get; set; } = new();
    }
}
