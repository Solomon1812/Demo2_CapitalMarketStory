using Demo2_CapitalMarketStory.Models;

namespace Demo2_CapitalMarketStory.Services
{
    public class FinancialAnalysisService : IFinancialAnalysisService
    {
        public CompanyAnalysisResult Analyze(List<YearlyFinancialReport> reports)
        {
            var result = new CompanyAnalysisResult();

            //var lastReport = reports.LastOrDefault(r => r.ROA != 0 || r.ROE != 0);

            var lastReport = reports
                .OrderBy(r => r.YearReported)
                .LastOrDefault();

            if (lastReport == null)
            {
                result.CompanyStatus = "Date insuficiente";
                result.AltmanZScore = 0;
                result.InsolvencyRisk = "Date insuficiente";
                return result;
            }

            // 1. Calcul Performanță (Iluzia ROE și Unghiul Mort reparate)
            decimal rezultatNet = lastReport.ProfitNet - lastReport.PierdereNet;
            decimal rezultatBrut = lastReport.ProfitBrut - lastReport.PierdereBrut;
            int scor = 0;

            if (lastReport.CapitaluriTotale > 0 && rezultatNet > 0)
            {
                if (lastReport.ROA >= 0.05m) scor++;
                if (lastReport.ROE >= 0.15m) scor++;
                if (lastReport.MarjaProfit >= 0.01m) scor++;
                if (lastReport.RataCrestereCifraAfaceriNet > 0) scor++;
                if (lastReport.RataCrestereProfitNet > 0) scor++;
            }

            if (scor >= 4) result.CompanyStatus = "Performanta excelenta";
            else if (scor >= 2) result.CompanyStatus = "Performanta stabila";
            else result.CompanyStatus = "Risc major / Dificultati financiare";

            // 2. Calcul Z-Score
            decimal totalActive = lastReport.ActiveImobilizate + lastReport.ActiveCirculante + lastReport.CheltuieliAvans;
            decimal totalDatorii = lastReport.Datorii + lastReport.Provizioane;

            if (totalActive > 0)
            {
                decimal X1 = (lastReport.ActiveCirculante - totalDatorii) / totalActive;
                decimal X2 = rezultatNet / totalActive;
                decimal X3 = rezultatBrut / totalActive;
                decimal X4 = totalDatorii == 0 ? 0 : lastReport.CapitaluriTotale / totalDatorii;

                result.AltmanZScore = (double)(6.56m * X1 + 3.26m * X2 + 6.72m * X3 + 1.05m * X4);

                if (result.AltmanZScore > 2.6) result.InsolvencyRisk = "Risc scazut (Safe Zone)";
                else if (result.AltmanZScore >= 1.1 && result.AltmanZScore <= 2.6) result.InsolvencyRisk = "Risc mediu (Grey Zone)";
                else result.InsolvencyRisk = "Risc ridicat de insolventa (Distress Zone)";
            }
            else
            {
                result.AltmanZScore = 0;
                result.InsolvencyRisk = "Date insuficiente";
            }

            // 3. Evaluare "Fair Value" pe Capital (AI-ul Auditor)
            result.RealCurrentCapital = lastReport.CapitaluriTotale;
            var sampleDataCap = new MLCapitalModel.ModelInput()
            {
                I1 = (float)lastReport.ActiveImobilizate,
                I2 = (float)lastReport.ActiveCirculante,
                I7 = (float)lastReport.Datorii,
                I13 = (float)lastReport.CifraAfaceriNet,
                I18 = (float)lastReport.ProfitNet,
                I20 = (float)(lastReport.NumarSalariati ?? 0)
            };
            var PredictionBenchmark = MLCapitalModel.Predict(sampleDataCap);
            result.PredictedCapitalValue = (decimal)PredictionBenchmark.Score;

            // 4. Predicția Profitului pe anul viitor (AI-ul Forecaster)
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
                I20 = (float)(lastReport.NumarSalariati ?? 0)
            };
            var PredictionProfit = MLProfitNetModel.Predict(sampleDataProfit);
            result.PredictedProfit2025 = (decimal)PredictionProfit.Score;

            return result;
        }
    }
}
