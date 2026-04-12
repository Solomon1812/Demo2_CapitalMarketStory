namespace Demo2_CapitalMarketStory.Models
{
    public class CompanyAnalysisResult
    {
        // Rezultate Evaluare Prezentă
        public string CompanyStatus { get; set; }
        public double AltmanZScore { get; set; }
        public string InsolvencyRisk { get; set; }

        // Rezultate Audit ML (Capitaluri)
        public decimal RealCurrentCapital { get; set; }
        public decimal PredictedCapValue { get; set; }

        // Rezultate Forecast ML (Profit)
        public decimal PredictedProfit2025 { get; set; }
    }
}


