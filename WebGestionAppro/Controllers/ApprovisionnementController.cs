using Microsoft.AspNetCore.Mvc;
using WebGestionAppro.Models;
using WebGestionAppro.Models.ViewModels;
using WebGestionAppro.Services;

namespace WebGestionAppro.Controllers
{
    public class ApprovisionnementController : Controller
    {
        private readonly IApprovisionnementService _approService;

        public ApprovisionnementController(IApprovisionnementService approService)
        {
            _approService = approService;
        }

        // GET: Approvisionnement
        public async Task<IActionResult> Index(DateTime? dateDebut, DateTime? dateFin, int? fournisseurId, int? articleId)
        {
            try
            {
                // Log pour debugging
                Console.WriteLine($"Filtres reçus - DateDebut: {dateDebut}, DateFin: {dateFin}, FournisseurId: {fournisseurId}, ArticleId: {articleId}");

                var viewModel = new ApprovisionnementListViewModel
                {
                    DateDebut = dateDebut,
                    DateFin = dateFin,
                    FournisseurId = fournisseurId,
                    ArticleId = articleId,
                    Fournisseurs = (await _approService.GetAllFournisseursAsync())?.ToList() ?? new List<Fournisseur>(),
                    Articles = (await _approService.GetAllArticlesAsync())?.ToList() ?? new List<Article>()
                };

                // Si au moins un filtre est appliqué
                if (dateDebut.HasValue || dateFin.HasValue || fournisseurId.HasValue || articleId.HasValue)
                {
                    Console.WriteLine("Recherche avec filtres...");
                    viewModel.Approvisionnements = (await _approService.SearchApprovisionnementsAsync(
                        dateDebut, dateFin, fournisseurId, articleId))?.ToList() ?? new List<Approvisionnement>();
                    Console.WriteLine($"Nombre de résultats: {viewModel.Approvisionnements.Count}");
                }
                else
                {
                    Console.WriteLine("Chargement de tous les approvisionnements...");
                    viewModel.Approvisionnements = (await _approService.GetAllApprovisionnementsAsync())?.ToList() ?? new List<Approvisionnement>();
                    Console.WriteLine($"Nombre total: {viewModel.Approvisionnements.Count}");
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
                TempData["ErrorMessage"] = $"Erreur lors du chargement des approvisionnements: {ex.Message}";
                return View(new ApprovisionnementListViewModel
                {
                    Fournisseurs = new List<Fournisseur>(),
                    Articles = new List<Article>(),
                    Approvisionnements = new List<Approvisionnement>()
                });
            }
        }

        // GET: Approvisionnement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var approvisionnement = await _approService.GetApprovisionnementByIdAsync(id);
            if (approvisionnement == null)
            {
                return NotFound();
            }

            return View(approvisionnement);
        }

        // GET: Approvisionnement/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ApprovisionnementCreateViewModel
            {
                Fournisseurs = (await _approService.GetAllFournisseursAsync()).ToList(),
                ArticlesDisponibles = (await _approService.GetAllArticlesAsync()).ToList(),
                DateApprovisionnement = DateTime.Now
            };

            return View(viewModel);
        }

        // POST: Approvisionnement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApprovisionnementCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var approvisionnement = new Approvisionnement
                {
                    DateApprovisionnement = viewModel.DateApprovisionnement,
                    FournisseurId = viewModel.FournisseurId,
                    Reference = viewModel.Reference ?? string.Empty,
                    Observations = viewModel.Observations,
                    Statut = StatutApprovisionnement.Recu,
                    ApprovisionnementArticles = viewModel.Articles.Select(a => new ApprovisionnementArticle
                    {
                        ArticleId = a.ArticleId,
                        Quantite = a.Quantite,
                        PrixUnitaire = a.PrixUnitaire,
                        Montant = a.Quantite * a.PrixUnitaire
                    }).ToList()
                };

                await _approService.CreateApprovisionnementAsync(approvisionnement);
                TempData["SuccessMessage"] = "Approvisionnement créé avec succès!";
                return RedirectToAction(nameof(Index));
            }

            // Recharger les listes en cas d'erreur
            viewModel.Fournisseurs = (await _approService.GetAllFournisseursAsync()).ToList();
            viewModel.ArticlesDisponibles = (await _approService.GetAllArticlesAsync()).ToList();
            return View(viewModel);
        }

        // GET: Approvisionnement/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var approvisionnement = await _approService.GetApprovisionnementByIdAsync(id);
            if (approvisionnement == null)
            {
                return NotFound();
            }

            return View(approvisionnement);
        }

        // POST: Approvisionnement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _approService.DeleteApprovisionnementAsync(id);
            TempData["SuccessMessage"] = "Approvisionnement supprimé avec succès!";
            return RedirectToAction(nameof(Index));
        }

        // API pour récupérer les détails d'un article (utilisé par JavaScript)
        [HttpGet]
        public async Task<IActionResult> GetArticleDetails(int id)
        {
            var article = await _approService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = article.Id,
                nom = article.Nom,
                prixUnitaire = article.PrixUnitaire,
                unite = article.Unite
            });
        }
    }
}
