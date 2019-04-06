namespace API.DataTransferObjects.ExchangeRateReport
{
    using System;
    using System.Collections.Generic;

    public class MonthExchangeRateReport
    {
        public DateTime ReportDate { get; set; }

        public IEnumerable<WeekExchangeRateStatistic> ByWeek { get; set; }
    }
}
