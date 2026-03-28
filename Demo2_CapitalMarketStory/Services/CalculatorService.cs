using Demo2_CapitalMarketStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo2_CapitalMarketStory.Services
{
    public class CalculatorService : ICalculatorService
    {
        public List<YearlyFinancialReport> CalculateKpi(List<YearlyFinancialReport> reports)
        {
            var SortedReports = reports
                .OrderBy(r => r.YearReported)
                .ToList();

            for (int i = 0; i < SortedReports.Count; i++)
            {
                var current = SortedReports[i];

                // CALCUL INDICATORI

                // Active Totale = Imobilizate + Circulante + Cheltuieli in avans 
                decimal activeTotale = current.ActiveImobilizate + current.ActiveCirculante + current.CheltuieliAvans;

                current.ROA = SafeDivide(current.ProfitNet, activeTotale);

                current.ROE = SafeDivide(current.ProfitNet, current.CapitaluriTotale);

                current.MarjaProfit = SafeDivide(current.ProfitNet, current.CifraAfaceriNet);

                current.RataCrestereCifraAfaceriNet = 0;
                current.RataCrestereProfitNet = 0;
            }

            // CALCUL RATE
            for (int i = 1; i < SortedReports.Count; i++)
            {
                var current = SortedReports[i];
                var previous = SortedReports[i - 1];

                // Numarator: Diferenta dintre anul curent si anul precedent
                // Numitor: Anul precedent
                current.RataCrestereCifraAfaceriNet = SafeDivide(
                    current.CifraAfaceriNet - previous.CifraAfaceriNet,
                    previous.CifraAfaceriNet
                );

                // Numarator: Diferenta profitului
                // Numitor: Modulul profitului precedent
                current.RataCrestereProfitNet = SafeDivide(
                    current.ProfitNet - previous.ProfitNet,
                    Math.Abs(previous.ProfitNet)
                );
            }


            var Prediction1 = MLProfitNetModel.Predict();
            var Prediction2 = MLTESTModel.Predict();


            decimal PredictedProfit0 = (decimal)Prediction1.Profitul_Net[0];
            decimal PredictedCA0 = (decimal)Prediction2.Cifra_de_afaceri_neta[0];


            var Report25 = new YearlyFinancialReport
            {
                YearReported = SortedReports.Last().YearReported + 1,
                ProfitNet = PredictedProfit0,
                CifraAfaceriNet = PredictedCA0
            };

            SortedReports.Add(Report25);

            var Prediction3 = MLProfitNetModel.Predict();
            var Prediction4 = MLTESTModel.Predict();


            decimal PredictedProfit1 = (decimal)Prediction1.Profitul_Net[1];
            decimal PredictedCA1 = (decimal)Prediction2.Cifra_de_afaceri_neta[1];

            var Report26 = new YearlyFinancialReport
            {
                YearReported = SortedReports.Last().YearReported + 1,
                ProfitNet = PredictedProfit1,
                CifraAfaceriNet = PredictedCA1
            };

            SortedReports.Add(Report26);


            return SortedReports;
        }

        private decimal SafeDivide(decimal numarator, decimal numitor)
        {
            if (numitor == 0)
            {
                return 0;
            }
            return numarator / numitor;
        }
    }
}