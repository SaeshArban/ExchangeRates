namespace API.Presenters.MonthExchangeRateReportPresenters
{
    using API.DataTransferObjects.ExchangeRateReport;

    public interface IMonthExchangeRateReportPresenter
    {
        string GetString(MonthExchangeRateReport report);
    }
}
