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
                    Math.Abs(previous.ProfitNet)
                );
            }

            ////Load sample data    CODE SNIPPET CONSUME ML
            //var sampleData = new MLTESTModel.ModelInput()
            //{
            //    I1 = 32364096F,
            //    I2 = 16288872F,
            //    I3 = 1011436F,
            //    I4 = 14733855F,
            //    I5 = 543581F,
            //    I6 = 414046F,
            //    I7 = 40508996F,
            //    I8 = 796412F,
            //    I10 = 7761606F,
            //    I11 = 310755F,
            //    I13 = 53073256F,
            //    I14 = 62077670F,
            //    I15 = 55431836F,
            //    I16 = 6645837F,
            //    I17 = 0F,
            //    I19 = 0F,
            //    I20 = 214F,
            //};

            ////Load model and predict output
            //var result = MLTESTModel.Predict(sampleData);


            var lastReport = SortedReports.Last();

            var sampleData = new MLTESTModel.ModelInput()
            {
                I1 = (float)lastReport.ActiveImobilizate,     
                I2 = (float)lastReport.ActiveCirculante,      
                I3 = (float)lastReport.Stocuri,                
                I4 = (float)lastReport.Creante,                
                I5 = (float)lastReport.Casa,                   
                I6 = (float)lastReport.CheltuieliAvans,        
                //I7 = (float)lastReport.Datorii,               
                I8 = (float)lastReport.VenituriAvans,         
                I10 = (float)lastReport.CapitaluriTotale,      
                I11 = (float)lastReport.CapitaluriVarsate,     
                I13 = (float)lastReport.CifraAfaceriNet,       
                //I14 = (float)lastReport.VenituriTotale,        
                //I15 = (float)lastReport.CheltuieliTotale,      
                I16 = (float)lastReport.ProfitBrut,            
                I17 = (float)lastReport.PierdereBrut,          
                I19 = (float)lastReport.PierdereNet,           
                I20 = (float)lastReport.NumarSalariati
            };

            var Prediction1 = MLTESTModel.Predict(sampleData);
            var Prediction2 = MLCifraAfaceriModel.Predict();

            decimal PredictedProfit0 = (decimal)Prediction1.Score;
            decimal PredictedCA0 = (decimal)Prediction2.Cifra_de_afaceri_neta[0];


            var Report25 = new YearlyFinancialReport
            {
                YearReported = SortedReports.Last().YearReported + 1,
                ProfitNet = PredictedProfit0,
                CifraAfaceriNet = PredictedCA0
            };

            SortedReports.Add(Report25);


            //var Prediction3 = MLTESTModel.Predict(sampleData);
            //var Prediction4 = MLCifraAfaceriModel.Predict();

            //decimal PredictedProfit1 = (decimal)Prediction3.Score;
            //decimal PredictedCA1 = (decimal)Prediction4.Cifra_de_afaceri_neta[1];

            //var Report26 = new YearlyFinancialReport
            //{
            //    YearReported = SortedReports.Last().YearReported + 1,
            //    ProfitNet = PredictedProfit1,
            //    CifraAfaceriNet = PredictedCA1
            //};

            //SortedReports.Add(Report26);


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