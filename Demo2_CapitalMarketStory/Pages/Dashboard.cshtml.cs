using Demo2_CapitalMarketStory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo2_CapitalMarketStory.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext _context;

        public DashboardModel(Demo2_CapitalMarketStory.Data.Demo2_CapitalMarketStoryContext context)
        {
            _context = context;
        }

        public Company Company { get; set; }
        public List<YearlyFinancialReport> FinancialReports { get; set; }
        public string CompanyStatus { get; set; }

        public decimal PredictedProfit2025 { get; set; }

        public double AltmanZScore { get; set; }
        public string InsolvencyRisk { get; set; }

        public decimal PredictedCapValue { get; set; }
        public decimal RealCurrentCapital { get; set; }

        public async Task<IActionResult> OnGetAsync(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToPage("/Companies/Create");
            }

            Company = await _context.Company
                .FirstOrDefaultAsync(m => m.CompanyId == companyId);

            if (Company == null)
            {
                return RedirectToPage("/Companies/Create");
            }

            FinancialReports = await _context.YearlyFinancialReport
                .Include(r => r.Import)
                .Where(r => r.Import.CompanyId == companyId)
                .OrderBy(r => r.YearReported)
                .ToListAsync();



            var lastReport = FinancialReports
                .LastOrDefault(r => r.ROA != 0);

            if (lastReport != null)
            {
                decimal rezultatNet = lastReport.ProfitNet - lastReport.PierdereNet;
                decimal rezultatBrut = lastReport.ProfitBrut - lastReport.PierdereBrut;

                int scor = 0;

                // O firmă cu capital negativ sau pierdere uriașă NU primește puncte de performanță
                if (lastReport.CapitaluriTotale > 0 && rezultatNet > 0)
                {
                    if (lastReport.ROA >= 0.05m) scor++;
                    if (lastReport.ROE >= 0.15m) scor++;
                    if (lastReport.MarjaProfit >= 0.01m) scor++;
                    if (lastReport.RataCrestereCifraAfaceriNet > 0) scor++;
                    if (lastReport.RataCrestereProfitNet > 0) scor++;
                }

                if (scor >= 4)
                {
                    CompanyStatus = "Performanta excelenta";
                }
                else if (scor >= 2)
                {
                    CompanyStatus = "Performanta stabila ";
                }
                else
                {
                    CompanyStatus = "Risc major / Dificultati financiare ";
                }



                decimal totalActive = lastReport.ActiveImobilizate + lastReport.ActiveCirculante + lastReport.CheltuieliAvans;
                decimal totalDatorii = lastReport.Datorii + lastReport.Provizioane;

                decimal X1 = (lastReport.ActiveCirculante - lastReport.Datorii) / totalActive;

                decimal X2 = rezultatNet / totalActive;

                decimal X3 = rezultatBrut / totalActive;

                decimal X4 = totalDatorii = lastReport.CapitaluriTotale / totalDatorii;


                AltmanZScore = (double)(6.56m * X1 + 3.26m * X2 + 6.72m * X3 + 1.05m * X4);

                if (AltmanZScore > 2.6)
                {
                    InsolvencyRisk = "Risc scazut (Safe Zone)";
                }
                else if (AltmanZScore >= 1.1 && AltmanZScore <= 2.6)
                {
                    InsolvencyRisk = "Risc mediu (Grey Zone)";
                }
                else
                {
                    InsolvencyRisk = "Risc ridicat de insolventa";
                }



                var sampleDataProfit = new MLProfitNetModel.ModelInput()
                {
                    I1 = (float)lastReport.ActiveImobilizate,
                    I2 = (float)lastReport.ActiveCirculante,
                    I3 = (float)lastReport.Stocuri,
                    I4 = (float)lastReport.Creante,
                    I5 = (float)lastReport.Casa,
                    I6 = (float)lastReport.CheltuieliAvans,

                    I8 = (float)lastReport.VenituriAvans,
                    I10 = (float)lastReport.CapitaluriTotale,
                    I11 = (float)lastReport.CapitaluriVarsate,
                    I13 = (float)lastReport.CifraAfaceriNet,

                    I16 = (float)lastReport.ProfitBrut,
                    I17 = (float)lastReport.PierdereBrut,
                    I19 = (float)lastReport.PierdereNet,
                    I20 = (float)lastReport.NumarSalariati
                };


                var P1 = MLProfitNetModel.Predict(sampleDataProfit);
                PredictedProfit2025 = (decimal)P1.Score;



                RealCurrentCapital = lastReport.CapitaluriTotale;

                var sampleDataCap = new MLCapitalModel.ModelInput()
                {
                    I1 = (float)lastReport.ActiveImobilizate,
                    I2 = (float)lastReport.ActiveCirculante,
                    I7 = (float)lastReport.Datorii,
                    I13 = (float)lastReport.CifraAfaceriNet,
                    I18 = (float)lastReport.ProfitNet,
                    I20 = (float)lastReport.NumarSalariati
                };

                var PredictionBenchmark = MLCapitalModel.Predict(sampleDataCap);
                PredictedCapValue = (decimal)PredictionBenchmark.Score;
            }
            else
            {
                CompanyStatus = "Date insuficiente";
                AltmanZScore = 0;
                InsolvencyRisk = "Date insuficiente";

            }


            return Page();
        }
    }
}