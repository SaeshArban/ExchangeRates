namespace API.Presenters.YearStatisticPresenters
{
    using API.DataTransferObjects.ExchangeRateReport;

    public interface IYearStatisticPresenter
    {
        string GetString(YearStatistic yearStatistic);
    }
}
