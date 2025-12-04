using WebGestionAppro.Models;

namespace WebGestionAppro.Services.Interfaces
{
    public interface IApprovisionnementService
    {
        // Méthodes pour les approvisionnements
        Task<IEnumerable<Approvisionnement>> GetAllApprovisionnementsAsync();
        Task<Approvisionnement?> GetApprovisionnementByIdAsync(int id);
        Task<Approvisionnement> CreateApprovisionnementAsync(Approvisionnement approvisionnement);
        Task<bool> UpdateApprovisionnementAsync(Approvisionnement approvisionnement);
        Task<bool> DeleteApprovisionnementAsync(int id);
        
        // Méthodes de recherche et filtrage
        Task<IEnumerable<Approvisionnement>> SearchApprovisionnementsAsync(
            DateTime? dateDebut, 
            DateTime? dateFin, 
            int? fournisseurId = null,
            int? articleId = null);
        
        // Méthodes pour les articles
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article?> GetArticleByIdAsync(int id);
        
        // Méthodes pour les fournisseurs
        Task<IEnumerable<Fournisseur>> GetAllFournisseursAsync();
        Task<Fournisseur?> GetFournisseurByIdAsync(int id);
        
        // Méthodes pour les statistiques (Dashboard)
        Task<decimal> GetMontantTotalAsync(DateTime? dateDebut = null, DateTime? dateFin = null);
        Task<int> GetNombreApprovisionnementsAsync(DateTime? dateDebut = null, DateTime? dateFin = null);
        Task<Fournisseur?> GetFournisseurPrincipalAsync(DateTime? dateDebut = null, DateTime? dateFin = null);
        Task<Dictionary<string, decimal>> GetStatistiquesParFournisseurAsync(DateTime? dateDebut = null, DateTime? dateFin = null);
    }
}
