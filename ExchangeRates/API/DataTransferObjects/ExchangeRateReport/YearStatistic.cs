namespace API.DataTransferObjects.ExchangeRateReport
{
    using System;
    using System.Collections.Generic;

    public class YearStatistic
    {
        public DateTime Date { get; set; }

        public IEnumerable<MonthExchangeRateReport> ByMonth { get; set; }
    }
}
