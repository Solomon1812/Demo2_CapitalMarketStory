using Demo2_CapitalMarketStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo2_CapitalMarketStory.Services
{
    public class CalculatorService : ICalculatorService
    {
        public List<YearlyFinancialReport> CalculateKpiAsync(List<YearlyFinancialReport> reports)
        {
            // 1. Sortăm rapoartele crescător după an. 
            // OBLIGATORIU pentru ca formulele de creștere YoY (Year-over-Year) să funcționeze corect!
            var sortedReports = reports.OrderBy(r => r.YearReported).ToList();

            for (int i = 0; i < sortedReports.Count; i++)
            {
                var current = sortedReports[i];

                // --- CALCULUL INDICATORILOR STATICI ---

                // Active Totale = Imobilizate + Circulante + Cheltuieli in avans 
                decimal activeTotale = current.ActiveImobilizate + current.ActiveCirculante + current.CheltuieliAvans;

                // 1. ROA (Rentabilitatea activelor)
                // Evitam impartirea la zero (DivideByZeroException)
                current.ROA = activeTotale != 0 ? (current.ProfitNet / activeTotale) : 0;

                // 2. ROE (Rentabilitatea capitalului propriu)
                current.ROE = current.CapitaluriTotale != 0 ? (current.ProfitNet / current.CapitaluriTotale) : 0;

                // 3. Marja de profit net
                current.MarjaProfit = current.CifraAfaceriNet != 0 ? (current.ProfitNet / current.CifraAfaceriNet) : 0;


                // --- CALCULUL INDICATORILOR DINAMICI (Rate de creștere) ---

                if (i == 0)
                {
                    // Suntem în primul an disponibil din fișier (ex: 2014). 
                    // Nu avem date din 2013, deci nu putem calcula o creștere. Rata e 0.
                    current.RataCrestereCifraAfaceriNet = 0;
                    current.RataCrestereProfitNet = 0;
                }
                else
                {
                    // Luăm datele din anul precedent
                    var previous = sortedReports[i - 1];

                    // 4. Rata de creștere a cifrei de afaceri nete
                    if (previous.CifraAfaceriNet != 0)
                    {
                        current.RataCrestereCifraAfaceriNet = (current.CifraAfaceriNet - previous.CifraAfaceriNet) / previous.CifraAfaceriNet;
                    }
                    else
                    {
                        current.RataCrestereCifraAfaceriNet = 0;
                    }

                    // 5. Rata de creștere a profitului net
                    if (previous.ProfitNet != 0)
                    {
                        // ATENȚIE: Folosim Math.Abs la numitor! 
                        // Daca anul trecut compania a avut pierdere (-1000) si acum are profit (500), 
                        // formula clasica ar da un procentaj gresit/negativ de crestere. Math.Abs repara asta.
                        current.RataCrestereProfitNet = (current.ProfitNet - previous.ProfitNet) / Math.Abs(previous.ProfitNet);
                    }
                    else
                    {
                        current.RataCrestereProfitNet = 0;
                    }
                }
            }

            return sortedReports;
        }
    }
}