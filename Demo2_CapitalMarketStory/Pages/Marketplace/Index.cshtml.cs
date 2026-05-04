using Demo2_CapitalMarketStory.Models;
using Demo2_CapitalMarketStory.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo2_CapitalMarketStory.Pages.Marketplace
{
    public class IndexModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;
        private readonly IFinancialAnalysisService _analysisService;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(
            Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context,
            IFinancialAnalysisService analysisService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _analysisService = analysisService;
            _userManager = userManager; // Avem nevoie de el ca s? lu?m adresa de email a antreprenorului
        }

        // Un model special doar pentru afisarea în Marketplace
        public class InvestorViewModel
        {
            public Company Company { get; set; }
            public string OwnerEmail { get; set; }
            public string CompanyStatus { get; set; }

            // --- DATE NOI ADAUGATE PENTRU CARD ---
            public decimal RealProfit { get; set; }
            public decimal PredictedProfit { get; set; }
            public decimal RealCapital { get; set; }
            public decimal ROA { get; set; }
            public decimal ROE { get; set; }
        }

        public List<InvestorViewModel> AvailableCompanies { get; set; } = new List<InvestorViewModel>();

        public async Task OnGetAsync()
        {
            // 1. Aducem TOATE companiile care au cel pu?in un Import (au istoric)
            var companiesWithData = await _context.Company
                .Include(c => c.Imports)
                    .ThenInclude(i => i.Reports)
                .Where(c => c.Imports.Any())
                .OrderBy(c => c.CAEN) // Sortate dup? CAEN, cum ai vrut
                .ToListAsync();

            // 2. Trecem prin ele, le calcul?m statusul ?i le extragem email-ul
            foreach (var comp in companiesWithData)
            {
                // G?sim utilizatorul proprietar în baza de date de Identity
                var owner = await _userManager.FindByIdAsync(comp.UserId);

                var latestImport = comp.Imports.OrderByDescending(i => i.ImportDate).First();

                // Re?inem ultimul raport real înc?rcat pentru a extrage ROA, ROE ?i profitul
                var lastReport = latestImport.Reports.OrderByDescending(r => r.YearReported).First();

                var analysisResult = _analysisService.Analyze(latestImport.Reports.ToList());

                AvailableCompanies.Add(new InvestorViewModel
                {
                    Company = comp,
                    OwnerEmail = owner.Email,
                    CompanyStatus = analysisResult.CompanyStatus,

                    // --- ADAUGAM VALORILE NOILE ---
                    RealProfit = lastReport.ProfitNet - lastReport.PierdereNet, // Profitul real din ultimul an
                    PredictedProfit = analysisResult.PredictedProfit2025,       // Predic?ia de profit care func?ioneaz?
                    RealCapital = analysisResult.RealCurrentCapital,            // Capitalul real la zi calculat de service
                    ROA = lastReport.ROA,                                       // Return on Assets
                    ROE = lastReport.ROE                                        // Return on Equity
                });
            }
        }
    }
}