using Demo2_CapitalMarketStory.Models;

namespace Demo2_CapitalMarketStory.Services
{
    public interface ICalculatorService
    {
        // dupa import create submit -> se calculeaza kpi-urile
        //lista rapoarte, calcul multi anual
        List<YearlyFinancialReport> CalculateKpi(List<YearlyFinancialReport> reports);


    }
}
