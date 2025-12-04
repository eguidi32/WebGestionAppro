using Microsoft.AspNetCore.Mvc;
using WebGestionAppro.Models.ViewModels;
using WebGestionAppro.Services;

namespace WebGestionAppro.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IApprovisionnementService _approService;

        public DashboardController(IApprovisionnementService approService)
        {
            _approService = approService;
        }

        public async Task<IActionResult> Index(DateTime? dateDebut, DateTime? dateFin)
        {
            // Par défaut, afficher les statistiques du mois en cours
            if (!dateDebut.HasValue)
                dateDebut = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            
            if (!dateFin.HasValue)
                dateFin = DateTime.Now;

            var viewModel = new DashboardViewModel
            {
                MontantTotal = await _approService.GetMontantTotalAsync(dateDebut, dateFin),
                NombreApprovisionnements = await _approService.GetNombreApprovisionnementsAsync(dateDebut, dateFin),
                StatistiquesParFournisseur = await _approService.GetStatistiquesParFournisseurAsync(dateDebut, dateFin),
                DerniersApprovisionnements = (await _approService.GetAllApprovisionnementsAsync()).Take(5).ToList(),
                Periode = $"{dateDebut.Value:MMMM yyyy}"
            };

            var fournisseurPrincipal = await _approService.GetFournisseurPrincipalAsync(dateDebut, dateFin);
            if (fournisseurPrincipal != null)
            {
                viewModel.FournisseurPrincipalNom = fournisseurPrincipal.Nom;
                
                if (viewModel.StatistiquesParFournisseur.ContainsKey(fournisseurPrincipal.Nom))
                {
                    viewModel.FournisseurPrincipalMontant = viewModel.StatistiquesParFournisseur[fournisseurPrincipal.Nom];
                    viewModel.FournisseurPrincipalPourcentage = viewModel.MontantTotal > 0 
                        ? (viewModel.FournisseurPrincipalMontant / viewModel.MontantTotal) * 100 
                        : 0;
                }
            }

            return View(viewModel);
        }
    }
}
