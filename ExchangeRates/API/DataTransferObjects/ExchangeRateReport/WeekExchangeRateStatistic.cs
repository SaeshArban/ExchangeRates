namespace API.DataTransferObjects.ExchangeRateReport
{
    using System;
    using System.Collections.Generic;

    public class WeekExchangeRateStatistic
    {
        public DateTime WeekStart { get; set; }

        public DateTime WeekEndDate { get; set; }

        public IEnumerable<ExchangeRateStatisticInfo> Infos { get; set; }
    }
}
