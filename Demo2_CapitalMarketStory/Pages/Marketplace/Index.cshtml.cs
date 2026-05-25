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
            _userManager = userManager; 
        }

        public class InvestorViewModel
        {
            public Company Company { get; set; }
            public string OwnerEmail { get; set; }
            public string CompanyStatus { get; set; }

            
            public decimal RealProfit { get; set; }
            public decimal PredictedProfit { get; set; }
            public decimal RealCapital { get; set; }
            public decimal ROA { get; set; }
            public decimal ROE { get; set; }
        }

        public List<InvestorViewModel> AvailableCompanies { get; set; } = new List<InvestorViewModel>();

        public async Task OnGetAsync()
        {
            var companiesWithData = await _context.Company
                .Include(c => c.Imports)
                    .ThenInclude(i => i.Reports)
                .Where(c => c.Imports.Any())
                .OrderBy(c => c.CAEN) 
                .ToListAsync();

            foreach (var comp in companiesWithData)
            {
                var owner = await _userManager.FindByIdAsync(comp.UserId);

                var latestImport = comp.Imports.OrderByDescending(i => i.ImportDate).First();

                var lastReport = latestImport.Reports.OrderByDescending(r => r.YearReported).First();

                var analysisResult = _analysisService.Analyze(latestImport.Reports.ToList());

                AvailableCompanies.Add(new InvestorViewModel
                {
                    Company = comp,
                    OwnerEmail = owner.Email,
                    CompanyStatus = analysisResult.CompanyStatus,

                    
                    RealProfit = lastReport.ProfitNet - lastReport.PierdereNet, 
                    PredictedProfit = analysisResult.PredictedProfit2025,       
                    RealCapital = analysisResult.RealCurrentCapital,            
                    ROA = lastReport.ROA,                                       
                    ROE = lastReport.ROE                                        
                });
            }
        }
    }
}