using Demo2_CapitalMarketStory.Models;

namespace Demo2_CapitalMarketStory.Services
{
    public interface ICalculatorService
    {
        List<YearlyFinancialReport> CalculateKpi(List<YearlyFinancialReport> reports);


    }
}
