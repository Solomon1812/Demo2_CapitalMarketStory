using CsvHelper;
using CsvHelper.Configuration;
using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;
using Demo2_CapitalMarketStory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
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

        private readonly ICalculatorService _calcService; 

        public CreateModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context, ICalculatorService calcService)
        {
            _context = context;
            _calcService = calcService; //DEPENDENCY INJECTION
        }

        [BindProperty]
        public Import Import { get; set; } = default!;

        [BindProperty]
        public IFormFile? UserFile { get; set; }

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


                List<YearlyFinancialReport> RawReport;

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var stream = UserFile.OpenReadStream())
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, config))
                {
                    RawReport = csv.GetRecords<YearlyFinancialReport>().ToList();

                }

                if (!RawReport.Any())
                {
                    ModelState.AddModelError("", "Fisierul nu contine date.");
                    return Page();
                }


                Import.StartYear = RawReport.Min(r => r.YearReported);
                Import.EndYear = RawReport.Max(r => r.YearReported);


                var IncomingYears = RawReport
                    .Select(r => r.YearReported)
                    .ToList();


                var ExistingReport = _context.YearlyFinancialReport
                    .Include(r => r.Import)
                    .Where(r => r.Import.CompanyId == Import.CompanyId)
                    .ToList();

                var OverlappingReport = ExistingReport
                    .Where(r => IncomingYears
                        .Contains(r.YearReported))
                    .ToList();


                if (OverlappingReport.Any() && !ConfirmOverwrite)
                {
                    ModelState.AddModelError("",
                        "Fisierul contine ani existenti. Bifeaza suprascriere.");
                    return Page();
                }

                _context.YearlyFinancialReport.RemoveRange(OverlappingReport);

                var PredictionToDelete = ExistingReport
                    .Where(r => r.YearReported > Import.EndYear)
                    .ToList();

                if (PredictionToDelete.Any())
                {
                    _context.YearlyFinancialReport.RemoveRange(PredictionToDelete);
                }


                var CalculatedReport = _calcService.CalculateKpi(RawReport);

                

                Import.Reports = CalculatedReport;
                _context.Import.Add(Import);

                await _context.SaveChangesAsync();

                return RedirectToPage("/Dashboard", new { companyId = Import.CompanyId });

            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "";

                ModelState.AddModelError("",
                    "Eroare: " + ex.Message + " | " + inner);

                return Page();
            }
        }
    }
}
