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