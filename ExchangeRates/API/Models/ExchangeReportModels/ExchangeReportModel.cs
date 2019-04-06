namespace API.Models.ExchangeReportModel
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API.DataTransferObjects.ExchangeRateReport;
    using API.Models.ExchangeRateModels;

    public class ExchangeReportModel : IExchangeReportModel
    {
        private readonly Config config;

        private readonly IExchangeRateModel exchangeRateModel;

        public ExchangeReportModel(Config config, IExchangeRateModel exchangeRateModel)
        {
            this.config = config;
            this.exchangeRateModel = exchangeRateModel;
        }

        public virtual async Task<MonthExchangeRateReport> GetMonthReport(DateTime month)
        {
            var model = new MonthExchangeRateReport
            {
                ReportDate = month
            };

            var weekExchangeRateStatistics = new List<WeekExchangeRateStatistic>();

            var startDate = new DateTime(month.Year, month.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            while (startDate < endDate)
            {
                var currentEndDate = startDate.AddDays((int)DayOfWeek.Saturday - (int)startDate.DayOfWeek + 1);

                if (currentEndDate > endDate)
                {
                    currentEndDate = endDate;
                }

                // TODO: распараллелить?
                weekExchangeRateStatistics.Add(new WeekExchangeRateStatistic
                {
                    WeekStart = startDate,
                    WeekEndDate = currentEndDate,
                    Infos = await this.GetInfos(startDate, currentEndDate)
                });
                startDate = currentEndDate.AddDays(1);
            }

            model.ByWeek = weekExchangeRateStatistics;

            return model;
        }

        public virtual async Task<YearStatistic> GetYearStatistic(DateTime year)
        {
            var model = new YearStatistic
            {
                Date = year
            };

            var startDate = new DateTime(year.Year, 1, 1);
            var endDate = startDate.AddYears(1).AddDays(-1);

            var list = new List<MonthExchangeRateReport>(12);

            while (startDate < endDate)
            {
                var monthReport = await this.GetMonthReport(startDate);
                list.Add(monthReport);
                startDate = startDate.AddMonths(1);
            }

            model.ByMonth = list;

            return model;
        }

        protected internal virtual async Task<IEnumerable<ExchangeRateStatisticInfo>> GetInfos(
            DateTime startDate,
            DateTime currentEndDate)
        {
            var list = new List<ExchangeRateStatisticInfo>(this.config.Currencies.Length);
            foreach (var currency in this.config.Currencies)
            {
                var rates = await this.exchangeRateModel.GetExchangeRates(startDate, currentEndDate, currency);
                if (rates.MedianRate != 0)
                {
                    list.Add(rates);
                }
            }

            return list;
        }
    }
}
