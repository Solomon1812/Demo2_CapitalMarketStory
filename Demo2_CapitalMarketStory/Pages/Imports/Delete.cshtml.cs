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
    public class DeleteModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public DeleteModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Import Import { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var import = await _context.Import.FirstOrDefaultAsync(m => m.ImportId == id);

            if (import == null)
            {
                return NotFound();
            }
            else
            {
                Import = import;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var import = await _context.Import.FindAsync(id);
            if (import != null)
            {
                Import = import;
                _context.Import.Remove(Import);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
