namespace API.Services.DailyInfoService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using API.Entities;

    public class DailyInfoService : BaseInfoService<List<ExchangeRate>>, IDailyInfoService
    {
        private readonly string baseUrl;
        private readonly string[] currencies;

        public DailyInfoService(HttpClient httpClient, Config config)
            : base(httpClient)
        {
            this.baseUrl = config.DailyHost;
            this.currencies = config.Currencies;
        }

        protected internal override string GetUrl(DateTime date)
        {
            return $"{this.baseUrl}?date={date.ToString("DD.MM.YYYY")}";
        }

        protected internal override List<ExchangeRate> Parse(string input)
        {
            var data = new List<ExchangeRate>();

            var lines = input
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            DateTime date = this.GetDate(lines);

            var extraElements = 2;

            var currencyLines = lines
                .Skip(extraElements)
                .Select(x => x.Split('|', StringSplitOptions.RemoveEmptyEntries).Skip(extraElements).ToArray())
                .ToArray();

            foreach (var item in currencyLines)
            {
                var currency = item[1];
                if (!this.currencies.Contains(currency))
                {
                    continue;
                }

                var coefficient = int.Parse(item[0]);
                var exchangeRate = double.Parse(item[2]);

                data.Add(new ExchangeRate
                {
                    Code = currency,
                    Date = date,
                    Rate = exchangeRate / coefficient
                });
            }

            return data;
        }

        protected internal virtual DateTime GetDate(string[] lines)
        {
            var dateStrings = lines[0]
                            .Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries)
                            .Take(3)
                            .ToArray();
            var date = DateTime.Parse($"{dateStrings[0]}.{dateStrings[1]} {dateStrings[2]}");
            return date;
        }
    }
}
