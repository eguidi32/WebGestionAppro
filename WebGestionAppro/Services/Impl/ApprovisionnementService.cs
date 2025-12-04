using Microsoft.EntityFrameworkCore;
using WebGestionAppro.Data;
using WebGestionAppro.Models;

namespace WebGestionAppro.Services.Impl
{
    public class ApprovisionnementService : IApprovisionnementService
    {
        private readonly GesApproDbContext _context;

        public ApprovisionnementService(GesApproDbContext context)
        {
            _context = context;
        }

        // Approvisionnements
        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnementsAsync()
        {
            return await _context.Approvisionnements
                .Include(a => a.Fournisseur)
                .Include(a => a.ApprovisionnementArticles)
                    .ThenInclude(aa => aa.Article)
                .OrderByDescending(a => a.DateApprovisionnement)
                .ToListAsync();
        }

        public async Task<Approvisionnement?> GetApprovisionnementByIdAsync(int id)
        {
            return await _context.Approvisionnements
                .Include(a => a.Fournisseur)
                .Include(a => a.ApprovisionnementArticles)
                    .ThenInclude(aa => aa.Article)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Approvisionnement> CreateApprovisionnementAsync(Approvisionnement approvisionnement)
        {
            // Générer une référence unique si elle n'est pas fournie
            if (string.IsNullOrEmpty(approvisionnement.Reference))
            {
                approvisionnement.Reference = await GenerateReferenceAsync();
            }

            // Calculer le montant total
            approvisionnement.MontantTotal = approvisionnement.ApprovisionnementArticles
                .Sum(aa => aa.Quantite * aa.PrixUnitaire);

            // Calculer le montant de chaque ligne
            foreach (var article in approvisionnement.ApprovisionnementArticles)
            {
                article.Montant = article.Quantite * article.PrixUnitaire;
            }

            _context.Approvisionnements.Add(approvisionnement);
            await _context.SaveChangesAsync();

            // Mettre à jour le stock des articles
            foreach (var aa in approvisionnement.ApprovisionnementArticles)
            {
                var article = await _context.Articles.FindAsync(aa.ArticleId);
                if (article != null)
                {
                    article.StockActuel += aa.Quantite;
                }
            }
            await _context.SaveChangesAsync();

            return approvisionnement;
        }

        public async Task<bool> UpdateApprovisionnementAsync(Approvisionnement approvisionnement)
        {
            _context.Approvisionnements.Update(approvisionnement);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteApprovisionnementAsync(int id)
        {
            var approvisionnement = await _context.Approvisionnements.FindAsync(id);
            if (approvisionnement == null)
                return false;

            _context.Approvisionnements.Remove(approvisionnement);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Approvisionnement>> SearchApprovisionnementsAsync(
            DateTime? dateDebut,
            DateTime? dateFin,
            int? fournisseurId = null,
            int? articleId = null)
        {
            var query = _context.Approvisionnements
                .Include(a => a.Fournisseur)
                .Include(a => a.ApprovisionnementArticles)
                    .ThenInclude(aa => aa.Article)
                .AsQueryable();

            if (dateDebut.HasValue)
            {
                Console.WriteLine($"Filtrage par dateDebut: {dateDebut.Value:yyyy-MM-dd}");
                query = query.Where(a => a.DateApprovisionnement.Date >= dateDebut.Value.Date);
            }

            if (dateFin.HasValue)
            {
                Console.WriteLine($"Filtrage par dateFin: {dateFin.Value:yyyy-MM-dd}");
                query = query.Where(a => a.DateApprovisionnement.Date <= dateFin.Value.Date);
            }

            if (fournisseurId.HasValue)
            {
                Console.WriteLine($"Filtrage par fournisseur: {fournisseurId.Value}");
                query = query.Where(a => a.FournisseurId == fournisseurId.Value);
            }

            if (articleId.HasValue)
            {
                Console.WriteLine($"Filtrage par article: {articleId.Value}");
                query = query.Where(a => a.ApprovisionnementArticles.Any(aa => aa.ArticleId == articleId.Value));
            }

            var result = await query.OrderByDescending(a => a.DateApprovisionnement).ToListAsync();
            Console.WriteLine($"Nombre de résultats trouvés: {result.Count}");
            return result;
        }

        // Articles
        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.OrderBy(a => a.Nom).ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        // Fournisseurs
        public async Task<IEnumerable<Fournisseur>> GetAllFournisseursAsync()
        {
            return await _context.Fournisseurs.OrderBy(f => f.Nom).ToListAsync();
        }

        public async Task<Fournisseur?> GetFournisseurByIdAsync(int id)
        {
            return await _context.Fournisseurs.FindAsync(id);
        }

        // Statistiques
        public async Task<decimal> GetMontantTotalAsync(DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            var query = _context.Approvisionnements.AsQueryable();

            if (dateDebut.HasValue)
                query = query.Where(a => a.DateApprovisionnement >= dateDebut.Value);

            if (dateFin.HasValue)
                query = query.Where(a => a.DateApprovisionnement <= dateFin.Value);

            return await query.SumAsync(a => a.MontantTotal);
        }

        public async Task<int> GetNombreApprovisionnementsAsync(DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            var query = _context.Approvisionnements.AsQueryable();

            if (dateDebut.HasValue)
                query = query.Where(a => a.DateApprovisionnement >= dateDebut.Value);

            if (dateFin.HasValue)
                query = query.Where(a => a.DateApprovisionnement <= dateFin.Value);

            return await query.CountAsync();
        }

        public async Task<Fournisseur?> GetFournisseurPrincipalAsync(DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            var query = _context.Approvisionnements
                .Include(a => a.Fournisseur)
                .AsQueryable();

            if (dateDebut.HasValue)
                query = query.Where(a => a.DateApprovisionnement >= dateDebut.Value);

            if (dateFin.HasValue)
                query = query.Where(a => a.DateApprovisionnement <= dateFin.Value);

            var fournisseurPrincipal = await query
                .GroupBy(a => a.Fournisseur)
                .Select(g => new
                {
                    Fournisseur = g.Key,
                    MontantTotal = g.Sum(a => a.MontantTotal)
                })
                .OrderByDescending(x => x.MontantTotal)
                .FirstOrDefaultAsync();

            return fournisseurPrincipal?.Fournisseur;
        }

        public async Task<Dictionary<string, decimal>> GetStatistiquesParFournisseurAsync(DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            var query = _context.Approvisionnements
                .Include(a => a.Fournisseur)
                .AsQueryable();

            if (dateDebut.HasValue)
                query = query.Where(a => a.DateApprovisionnement >= dateDebut.Value);

            if (dateFin.HasValue)
                query = query.Where(a => a.DateApprovisionnement <= dateFin.Value);

            var stats = await query
                .GroupBy(a => a.Fournisseur.Nom)
                .Select(g => new
                {
                    Nom = g.Key,
                    Montant = g.Sum(a => a.MontantTotal)
                })
                .ToDictionaryAsync(x => x.Nom, x => x.Montant);

            return stats;
        }

        // Méthode privée pour générer une référence unique
        private async Task<string> GenerateReferenceAsync()
        {
            var year = DateTime.Now.Year;
            var lastRef = await _context.Approvisionnements
                .Where(a => a.Reference.StartsWith($"APP-{year}-"))
                .OrderByDescending(a => a.Reference)
                .Select(a => a.Reference)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastRef != null)
            {
                var parts = lastRef.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int currentNumber))
                {
                    nextNumber = currentNumber + 1;
                }
            }

            return $"APP-{year}-{nextNumber:D3}";
        }
    }
}
