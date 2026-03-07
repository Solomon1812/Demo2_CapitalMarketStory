//pentru citire csv
using CsvHelper;
using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Models;
using Demo2_CapitalMarketStory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // !!serviciul de calcul al kpi-urilor impl in controller
        private readonly ICalculatorService _calcService; 

        public CreateModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context, ICalculatorService calcService)
        {
            _context = context;
            _calcService = calcService; //DEPENDENCY INJECTION
        }

        [BindProperty]
        public Import Import { get; set; } = default!;

        //fisierul incarcat
        [BindProperty]
        public IFormFile UserFile { get; set; }

        public IActionResult OnGet(int? companyId) //adaugat int? companyId,  inainte gol
        {
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "Name");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            //1 validare fisier incarcat
            if (UserFile == null || UserFile.Length == 0)
            {
                ModelState.AddModelError("", "Incarca un fisier valid");
                return Page();
            }

            // 2 completat date import
            Import.ImportDate = DateTime.Now;
            Import.FileName = UserFile.FileName;

            // save + importID
            _context.Import.Add(Import);
            await _context.SaveChangesAsync();


            // 3 citit csv si salvat in tabela YearlyFinancialReport 
            List<YearlyFinancialReport> RoughReport;

            //https://joshclose.github.io/CsvHelper/examples/reading/get-class-records/
            // fisier nesavlat, doar citit in memorie, convertit in lista de obiecte C#
            using (var stream = UserFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // randuri -> obiecte C#
                RoughReport = csv.GetRecords<YearlyFinancialReport>().ToList();
            }

            // 4 date relationale
            foreach (var raport in RoughReport)
            {
                // Legam fiecare rand din CSV de fisier importat
                raport.ImportId = Import.ImportId;
            }

            // 5 serviciul de calcul
            var CalculatedReport = _calcService.CalculateKpi(RoughReport);

            // 6 salvat dtb toate rapoartele financiare completate
            _context.YearlyFinancialReport.AddRange(CalculatedReport);
            await _context.SaveChangesAsync();

            //dashboard grafice
            return RedirectToPage("/Dashboard/Index", new { companyId = Import.CompanyId });
        }
    }
}
