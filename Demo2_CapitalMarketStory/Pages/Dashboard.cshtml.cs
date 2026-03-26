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
        public string CompanyStatus { get; private set; }

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


            if (FinancialReports != null && FinancialReports.Any())
            {
                var ultimulRaport = FinancialReports.Last();

                decimal activeTotale = ultimulRaport.ActiveImobilizate + ultimulRaport.ActiveCirculante + ultimulRaport.CheltuieliAvans;

                if (ultimulRaport.ROA < 0.05m && ultimulRaport.Datorii > activeTotale)
                    CompanyStatus = "Risc de faliment";
                else if (ultimulRaport.ROA >= 0.05m && ultimulRaport.ROA < 0.15m)
                    CompanyStatus = "Companie stabila ";
                else
                    CompanyStatus = "Performanta excelenta ";
            }


            return Page();
        }
    }
}