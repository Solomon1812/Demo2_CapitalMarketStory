//pentru citire csv
using CsvHelper;
using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;
using Demo2_CapitalMarketStory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Demo2_CapitalMarketStory.Pages.Imports
{
    public class CreateModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        // !!serviciul de calcul al kpi-urilor 
        private readonly ICalculatorService _calcService; 

        public CreateModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context, ICalculatorService calcService)
        {
            _context = context;
            _calcService = calcService; //DEPENDENCY INJECTION
        }

        [BindProperty]
        public Import Import { get; set; } = default!;

        [BindProperty]
        public IFormFile UserFile { get; set; }

        [BindProperty]
        public bool ConfirmOverwrite { get; set; } = false;

        public IActionResult OnGet(int? companyId) 
        {
            if (companyId == null)
            {
                return RedirectToPage("/Companies/Index");
            }

            Import = new Import { CompanyId = companyId.Value }; 
            return Page();

        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (UserFile == null || UserFile.Length == 0)
            {
                ModelState.AddModelError("", "Incarca un fisier valid");
                return Page();
            }

            try
            {
                Import.ImportDate = DateTime.Now;
                Import.FileName = UserFile.FileName;


                List<YearlyFinancialReport> RoughReport;


                using (var stream = UserFile.OpenReadStream())
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    RoughReport = csv.GetRecords<YearlyFinancialReport>().ToList();

                }
                Import.StartYear = RoughReport.Min(r => r.YearReported);
                Import.EndYear = RoughReport.Max(r => r.YearReported);


                var CalculatedReport = _calcService.CalculateKpi(RoughReport);

                var IncomingYears = CalculatedReport
                    .Select(r => r.YearReported)
                    .ToList();


                var OldReport = _context.YearlyFinancialReport
                    .Include(r => r.Import)
                    .Where(r => r.Import.CompanyId == Import.CompanyId)
                    .ToList();


                var OverlappingReports = OldReport
                    .Where(r => IncomingYears.Contains(r.YearReported)).ToList();
                
                if (OverlappingReports.Any())
                {
                    if (ConfirmOverwrite == true)
                    {
                        _context.YearlyFinancialReport.RemoveRange(OverlappingReports);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Fisierul contine ani care exista deja. Bifeaza casuta pentru a suprascrie anii existenti.");
                        return Page();
                    }
                }

                Import.Reports = CalculatedReport;
                _context.Import.Add(Import);

                _context.YearlyFinancialReport.AddRange(CalculatedReport);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Dashboard", new { companyId = Import.CompanyId });


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare: " + ex.Message);

                return Page();
            }
        }
    }
}
