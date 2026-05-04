using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo2_CapitalMarketStory.Pages.ControlPanel
{
    public class IndexModel : PageModel
    {
        private readonly Demo2_CapitalMarketStoryContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(Demo2_CapitalMarketStoryContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // --- STATISTICI ---
        public int TotalUsers { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalReports { get; set; }

        // --- LISTA UTILIZATORI ---
        public class UserViewModel
        {
            public string ID { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
        public List<UserViewModel> UsersList { get; set; } = new List<UserViewModel>();

        // Mesaje de succes/eroare
        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            // 1. Înc?rc?m statisticile
            TotalUsers = await _userManager.Users.CountAsync();
            TotalCompanies = await _context.Company.CountAsync();
            TotalReports = await _context.YearlyFinancialReport.CountAsync();

            // 2. Înc?rc?m lista de utilizatori ?i rolurile lor
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UsersList.Add(new UserViewModel
                {
                    ID = user.Id,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "F?r? Rol"
                });
            }
        }

        // --- AC?IUNEA 1: CUR??ENIE DE PRIM?VAR? ---
        public async Task<IActionResult> OnPostCleanCompaniesAsync()
        {
            // C?ut?m companiile care NU au niciun import asociat
            var inactiveCompanies = await _context.Company
                .Include(c => c.Imports)
                .Where(c => !c.Imports.Any())
                .ToListAsync();

            if (inactiveCompanies.Any())
            {
                int count = inactiveCompanies.Count;
                _context.Company.RemoveRange(inactiveCompanies);
                await _context.SaveChangesAsync();

                StatusMessage = $"Succes: Am ?ters {count} companii inactive (f?r? date financiare).";
            }
            else
            {
                StatusMessage = "Info: Baza de date este curat?. Nu exist? companii inactive.";
            }

            return RedirectToPage(); // Reînc?rc?m pagina
        }

        // --- AC?IUNEA 2: ?TERGERE UTILIZATOR ---
        public async Task<IActionResult> OnPostDeleteUserAsync(string idToDelete)
        {
            var user = await _userManager.FindByIdAsync(idToDelete);
            if (user != null)
            {
                // Identity ?terge utilizatorul. 
                // *Not?: Companiile lui ar trebui s? se ?tearg? automat dac? ai configurat Cascade Delete în baza de date.
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    StatusMessage = $"Succes: Utilizatorul {user.Email} a fost ?ters din sistem.";
                }
            }
            return RedirectToPage();
        }
    }
}