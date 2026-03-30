using Demo2_CapitalMarketStory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo2_CapitalMarketStory.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public DashboardModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        public Company Company { get; set; }
        public List<YearlyFinancialReport> FinancialReports { get; set; }
        public string CompanyStatus { get; set; }


        public async Task<IActionResult> OnGetAsync(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToPage("/Companies/Create");
            }

            Company = await _context.Company
                .FirstOrDefaultAsync(m => m.CompanyId == companyId);

            if (Company == null)
            {
                return RedirectToPage("/Companies/Create");
            }

            FinancialReports = await _context.YearlyFinancialReport
                .Include(r => r.Import)
                .Where(r => r.Import.CompanyId == companyId)
                .OrderBy(r => r.YearReported)
                .ToListAsync();



            var lastReport = FinancialReports
                .LastOrDefault(r => r.ROA != 0);

            if (lastReport != null)
            {
                int scor = 0;

                if (lastReport.ROA >= 0.05m) scor++;

                if (lastReport.ROE >= 0.15m) scor++;

                if (lastReport.MarjaProfit >= 0.01m) scor++;

                if (lastReport.RataCrestereCifraAfaceriNet > 0) scor++;

                if (lastReport.RataCrestereProfitNet > 0) scor++;

                if (scor >= 4)
                {
                    CompanyStatus = "Performanta excelenta";
                }
                else if (scor >= 2)
                {
                    CompanyStatus = "Performanta stabila ";
                }
                else
                {
                    CompanyStatus = "Risc major / Dificultati financiare ";
                }
            }
            else
            {
                CompanyStatus = "Date insuficiente";
            }


            return Page();
        }
    }
}