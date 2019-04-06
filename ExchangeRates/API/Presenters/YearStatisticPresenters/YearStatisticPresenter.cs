namespace API.Presenters.YearStatisticPresenters
{
    using System;
    using System.Text;
    using API.DataTransferObjects.ExchangeRateReport;
    using API.Presenters.MonthExchangeRateReportPresenters;

    public class YearStatisticPresenter : IYearStatisticPresenter
    {
        private readonly IMonthExchangeRateReportPresenter monthExchangeRateReportPresenter;

        public YearStatisticPresenter(IMonthExchangeRateReportPresenter monthExchangeRateReportPresenter)
        {
            this.monthExchangeRateReportPresenter = monthExchangeRateReportPresenter;
        }

        public string GetString(YearStatistic yearStatistic)
        {
            var sb = new StringBuilder();

            sb.Append($"Year: {yearStatistic.Date.Year}");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            foreach (var month in yearStatistic.ByMonth)
            {
                sb.Append(this.monthExchangeRateReportPresenter.GetString(month));
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
