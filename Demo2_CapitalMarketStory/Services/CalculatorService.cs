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

                decimal activeTotale = current.ActiveImobilizate + current.ActiveCirculante + current.CheltuieliAvans;

                current.ROA = SafeDivide(current.ProfitNet, activeTotale);

                current.ROE = SafeDivide(current.ProfitNet, current.CapitaluriTotale);

                current.MarjaProfit = SafeDivide(current.ProfitNet, current.CifraAfaceriNet);

                current.RataCrestereCifraAfaceriNet = 0;
                current.RataCrestereProfitNet = 0;
            }


            for (int i = 1; i < SortedReports.Count; i++)
            {
                var current = SortedReports[i];
                var previous = SortedReports[i - 1];

                current.RataCrestereCifraAfaceriNet = SafeDivide(
                    current.CifraAfaceriNet - previous.CifraAfaceriNet,
                    previous.CifraAfaceriNet
                );

                current.RataCrestereProfitNet = SafeDivide(
                    current.ProfitNet - previous.ProfitNet,
                    previous.ProfitNet
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