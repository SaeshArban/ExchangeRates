namespace API.Cache
{
    using System;
    using System.Collections.Concurrent;
    using API.DataTransferObjects.ExchangeRateReport;

    public class RateCache
    {
        private readonly ConcurrentDictionary<string, ExchangeRateStatisticInfo> cache = new ConcurrentDictionary<string, ExchangeRateStatisticInfo>();

        public virtual ExchangeRateStatisticInfo GetExchange(string code, DateTime startDate, DateTime endDate)
        {
            string key = GetKey(code, startDate, endDate);
            if (this.cache.ContainsKey(key))
            {
                return this.cache[key];
            }

            return null;
        }

        public virtual void AddExchange(ExchangeRateStatisticInfo info, DateTime startDate, DateTime endDate)
        {
            string key = GetKey(info.Code, startDate, endDate);

            this.cache.AddOrUpdate(key, info, (k, prevValue) => info);
        }

        private static string GetKey(string code, DateTime startDate, DateTime endDate)
        {
            return $"{code}{startDate.ToString("dd.MM.yyyy")}{endDate.ToString("dd.MM.yyyy")}";
        }
    }
}
