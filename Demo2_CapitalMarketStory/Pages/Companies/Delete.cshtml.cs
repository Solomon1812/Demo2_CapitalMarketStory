using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;

namespace Demo2_CapitalMarketStory.Pages.Companies
{
    public class DeleteModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public DeleteModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FirstOrDefaultAsync(m => m.CompanyId == id);

            if (company == null)
            {
                return NotFound();
            }
            else
            {
                Company = company;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);
            if (company != null)
            {
                var imports = _context.Import
                    .Where(i => i.CompanyId == id)
                    .ToList();

                var importIds = imports
                    .Select(i => i.ImportId)
                    .ToList();

                var rapoarteCopii = _context.YearlyFinancialReport
                    .Where(r => importIds.Contains(r.ImportId))
                    .ToList();

                // sters
                if (rapoarteCopii.Any())
                {
                    _context.YearlyFinancialReport
                        .RemoveRange(rapoarteCopii); 
                }

                if (imports.Any())
                {
                    _context.Import
                        .RemoveRange(imports); 
                }

                Company = company;
                _context.Company.Remove(Company);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");

        
        }
    }
}
