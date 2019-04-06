namespace API.Services.YearyInfoService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using API.Entities;

    public class YearInfoService : BaseInfoService<List<ExchangeRate>>, IYearInfoService
    {
        private readonly string baseUrl;
        private readonly string[] currencies;

        public YearInfoService(HttpClient httpClient, Config config)
            : base(httpClient)
        {
            this.baseUrl = config.YearHost;
            this.currencies = config.Currencies;
        }

        protected internal override string GetUrl(DateTime date)
        {
            return $"{this.baseUrl}?year={date.ToString("yyyy")}";
        }

        protected internal override List<ExchangeRate> Parse(string input)
        {
            var data = new List<ExchangeRate>();

            var lines = input
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var currencyInfos = this.GetInfos(lines);

            for (int i = 1; i < lines.Length; i++)
            {
                var ratesByDate = lines[i].Split('|');

                var date = DateTime.Parse(ratesByDate[0]);

                this.FillRatesForDate(data, currencyInfos, ratesByDate, date);
            }

            return data;
        }

        protected internal virtual void FillRatesForDate(
            List<ExchangeRate> data,
            List<(string Currency, int Coefficient, int Index)> currencyInfos,
            string[] ratesByDate,
            DateTime date)
        {
            foreach (var currencyInfo in currencyInfos)
            {
                var currencyRate = double.Parse(ratesByDate[currencyInfo.Index]);
                data.Add(new ExchangeRate
                {
                    Code = currencyInfo.Currency,
                    Date = date,
                    Rate = currencyRate / currencyInfo.Coefficient
                });
            }
        }

        protected internal virtual List<(string Currency, int Coefficient, int Index)> GetInfos(string[] lines)
        {
            var dirtyCurrencyInfos = lines[0]
                            .Split('|')
                            .Skip(1)
                            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                            .ToArray();

            var currencyInfos = new List<(string Currency, int Coefficient, int Index)>();
            for (int i = 0; i < dirtyCurrencyInfos.Length; i++)
            {
                var info = dirtyCurrencyInfos[i];
                if (this.currencies.Contains(info[1]))
                {
                    currencyInfos.Add((info[1], int.Parse(info[0]), i));
                }
            }

            return currencyInfos;
        }
    }
}
