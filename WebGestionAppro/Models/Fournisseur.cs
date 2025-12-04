namespace WebGestionAppro.Models
{
    public class Fournisseur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Adresse { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<Approvisionnement> Approvisionnements { get; set; } = new List<Approvisionnement>();
    }
}
