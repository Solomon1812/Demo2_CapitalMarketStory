using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Claims;

namespace Demo2_CapitalMarketStory.Pages.Companies
{
    public class CreateModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public CreateModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Set the UserId property to the current user's ID
            Company.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Company.Add(Company);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Imports/Create", new { companyId = Company.CompanyId });
        }
    }
}
