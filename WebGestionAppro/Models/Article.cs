namespace WebGestionAppro.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PrixUnitaire { get; set; }
        public int StockActuel { get; set; }
        public string? Unite { get; set; } 
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<ApprovisionnementArticle> ApprovisionnementArticles { get; set; } = new List<ApprovisionnementArticle>();
    }
}
