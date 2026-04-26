using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;
using System.Security.Claims;

namespace Demo2_CapitalMarketStory.Pages.Companies
{
    public class IndexModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public IndexModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        public IList<Company> Company { get;set; } = default!;

        public string CompanySort { get; set; }
        public string? CurrentFilter { get; set; }


        public async Task OnGetAsync(string? sortOrder, string? searchString)
        {
            CompanySort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CurrentFilter = searchString;

            // 1. Începem interogarea
            IQueryable<Company> companiesIQ = from c in _context.Company select c;

            // 2. APLICĂM SECURITATEA PRIMA DATĂ (Izolarea datelor)
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!User.IsInRole("Admin"))
            {
                // Dacă NU e admin, tăiem din start companiile care nu îi aparțin
                companiesIQ = companiesIQ.Where(c => c.UserId == currentUserId);
            }

            // 3. APLICĂM CĂUTAREA (acum va căuta doar în companiile la care are voie)
            if (!String.IsNullOrEmpty(searchString))
            {
                companiesIQ = companiesIQ.Where(c => c.Name.Contains(searchString)
                                                  || c.CUI.ToString().Contains(searchString));
            }

            // 4. APLICĂM SORTAREA
            switch (sortOrder)
            {
                case "name_desc":
                    companiesIQ = companiesIQ.OrderByDescending(c => c.Name);
                    break;
                default:
                    // Implicit le ordonam alfabetic A-Z
                    companiesIQ = companiesIQ.OrderBy(c => c.Name);
                    break;
            }

            // 5. ABIA LA FINAL EXECUTĂM INTEROGAREA
            Company = await companiesIQ
                .AsNoTracking()
                .ToListAsync();

        }

    }
}

