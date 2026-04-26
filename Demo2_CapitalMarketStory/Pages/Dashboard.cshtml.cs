using Demo2_CapitalMarketStory.Models;
using Demo2_CapitalMarketStory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Demo2_CapitalMarketStory.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;
        private readonly IFinancialAnalysisService _analysisService; // 1. Injectează serviciul

        public DashboardModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context,
                              IFinancialAnalysisService analysisService)
        {
            _context = context;
            _analysisService = analysisService;
        }

        public Company Company { get; set; }
        public List<YearlyFinancialReport> FinancialReports { get; set; }

        public string CompanyStatus { get; set; }
        public double AltmanZScore { get; set; }
        public string InsolvencyRisk { get; set; }
        public decimal PredictedCapValue { get; set; }
        public decimal RealCurrentCapital { get; set; }
        public decimal PredictedProfit2025 { get; set; }

        public async Task<IActionResult> OnGetAsync(int? companyId)
        {


            // 1. Luăm ID-ul userului logat
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Dacă a dat click pe "Dashboard" din meniu fără să specifice compania
            if (companyId == null)
            {
                // Căutăm ultima companie creată de acest user
                var lastCompany = await _context.Company
                    .Where(c => c.UserId == currentUserId)
                    .OrderByDescending(c => c.CompanyId) // Cea mai recentă
                    .FirstOrDefaultAsync();

                if (lastCompany != null)
                {
                    // ȘMECHERIA: Îl redirecționăm automat pe dashboard-ul ultimei companii!
                    return RedirectToPage("/Dashboard", new { companyId = lastCompany.CompanyId });
                }
                else
                {
                    TempData["InfoMessage"] = "Nu ai nicio companie înregistrată. Creează prima ta companie pentru a putea vizualiza Dashboard-ul!";                    // Dacă e un user complet nou și nu are nicio companie, abia atunci îl punem să creeze.
                    return RedirectToPage("/Companies/Create");
                }
            }

            Company = await _context.Company.FirstOrDefaultAsync(m => m.CompanyId == companyId);
            if (Company == null) 
                return RedirectToPage("/Companies/Create");

            FinancialReports = await _context.YearlyFinancialReport
                .Include(r => r.Import)
                .Where(r => r.Import.CompanyId == companyId)
                .OrderBy(r => r.YearReported)
                .ToListAsync();

            if (!FinancialReports.Any())
            {
                CompanyStatus = "Nu exista date financiare";
                return Page();
            }

            // 2. Apelează noul serviciu curat
            var analysisResult = _analysisService.Analyze(FinancialReports);

            // 3. Mapează rezultatele
            CompanyStatus = analysisResult.CompanyStatus;
            AltmanZScore = analysisResult.AltmanZScore;
            InsolvencyRisk = analysisResult.InsolvencyRisk;
            PredictedCapValue = analysisResult.PredictedCapitalValue;
            RealCurrentCapital = analysisResult.RealCurrentCapital;
            PredictedProfit2025 = analysisResult.PredictedProfit2025;

            return Page();
        }
    }
}