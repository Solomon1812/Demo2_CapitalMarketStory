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

            IQueryable<Company> companiesIQ = from c in _context.Company select c;

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!User.IsInRole("Admin"))
            {
                companiesIQ = companiesIQ.Where(c => c.UserId == currentUserId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                companiesIQ = companiesIQ.Where(c => c.Name.Contains(searchString)
                                                  || c.CUI.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    companiesIQ = companiesIQ.OrderByDescending(c => c.Name);
                    break;
                default:
                    companiesIQ = companiesIQ.OrderBy(c => c.Name);
                    break;
            }

            Company = await companiesIQ
                .AsNoTracking()
                .ToListAsync();

        }

    }
}

