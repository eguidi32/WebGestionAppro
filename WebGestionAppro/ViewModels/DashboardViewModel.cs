using WebGestionAppro.Models;

namespace WebGestionAppro.ViewModels
{
    public class DashboardViewModel
    {
        public decimal MontantTotal { get; set; }
        public int NombreApprovisionnements { get; set; }
        public string FournisseurPrincipalNom { get; set; } = string.Empty;
        public decimal FournisseurPrincipalMontant { get; set; }
        public decimal FournisseurPrincipalPourcentage { get; set; }
        public string Periode { get; set; } = string.Empty;
        public Dictionary<string, decimal> StatistiquesParFournisseur { get; set; } = new();
        public List<Approvisionnement> DerniersApprovisionnements { get; set; } = new();
    }
}
