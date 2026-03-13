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
    public class DetailsModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public DetailsModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        public Import Import { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var import = await _context.Import
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.ImportId == id);

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
    }
}
