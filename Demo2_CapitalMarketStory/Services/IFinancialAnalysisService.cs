using Demo2_CapitalMarketStory.Models;

namespace Demo2_CapitalMarketStory.Services
{
    public interface IFinancialAnalysisService
    {
        CompanyAnalysisResult Analyze(List<YearlyFinancialReport> reports);
    }
}
