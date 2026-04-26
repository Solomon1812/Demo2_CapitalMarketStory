using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo2_CapitalMarketStory.Pages.Imports
{
    public class IndexModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public IndexModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        public IList<Import> Import { get;set; } = default!;

        public string? CurrentCompanyFilter { get; set; }
        public DateTime? CurrentDateFilter { get; set; }

        public async Task OnGetAsync(string? searchCompany, DateTime? searchDate)
        {
            CurrentCompanyFilter = searchCompany;
            CurrentDateFilter = searchDate;

            // 1. Începem interogarea și includem compania
            IQueryable<Import> importsIQ = _context.Import
                .Include(i => i.Company);

            // 2. APLICĂM SECURITATEA PRIMA DATĂ
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!User.IsInRole("Admin"))
            {
                // Tăiem importurile care aparțin altor companii
                importsIQ = importsIQ.Where(i => i.Company.UserId == currentUserId);
            }

            // 3. APLICĂM FILTRELE DE CĂUTARE (pe lista deja securizată)
            if (!String.IsNullOrEmpty(searchCompany))
            {
                importsIQ = importsIQ
                    .Where(i => i.Company.Name.Contains(searchCompany));
            }

            if (searchDate.HasValue)
            {
                importsIQ = importsIQ
                    .Where(i => i.ImportDate.Date == searchDate.Value.Date);
            }

            // 4. APLICĂM ORDONAREA
            importsIQ = importsIQ
                .OrderByDescending(i => i.ImportDate);

            // 5. EXECUTĂM INTEROGAREA
            Import = await importsIQ
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
