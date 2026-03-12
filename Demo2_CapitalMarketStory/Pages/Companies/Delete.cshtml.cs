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
                // 2. Gasim "Parintii" (Toate importurile facute pentru aceasta companie)
                var imports = _context.Import.Where(i => i.CompanyId == id).ToList();

                // Extragem doar ID-urile importurilor ca sa putem cauta copiii
                var importIds = imports.Select(i => i.ImportId).ToList();

                // 3. Gasim "Copiii" (Toate rapoartele financiare legate de aceste importuri)
                var rapoarte = _context.YearlyFinancialReport
                    .Where(r => importIds.Contains(r.ImportId))
                    .ToList();

                // 4. Marea Curatenie (DE JOS IN SUS!)
                if (rapoarte.Any())
                {
                    _context.YearlyFinancialReport.RemoveRange(rapoarte); // Stergem copiii
                }

                if (imports.Any())
                {
                    _context.Import.RemoveRange(imports); // Stergem parintii
                }

                Company = company;
                _context.Company.Remove(Company);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");

        
        }
    }
}
