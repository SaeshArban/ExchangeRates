namespace API.Models.ExchangeReportModel
{
    using System;
    using System.Threading.Tasks;
    using API.DataTransferObjects.ExchangeRateReport;

    public interface IExchangeReportModel
    {
        Task<MonthExchangeRateReport> GetMonthReport(DateTime month);

        Task<YearStatistic> GetYearStatistic(DateTime year);
    }
}
