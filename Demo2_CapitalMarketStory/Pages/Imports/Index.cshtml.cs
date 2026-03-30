using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;

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

            IQueryable<Import> importsIQ = _context.Import
                .Include(i => i.Company);

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

            importsIQ = importsIQ
                .OrderByDescending(i => i.ImportDate);

            Import = await importsIQ
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
