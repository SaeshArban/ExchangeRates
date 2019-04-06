namespace API.Models.ExchangeRateModels
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using API.Cache;
    using API.DataTransferObjects.ExchangeRateReport;
    using API.Entities;
    using API.Jobs;
    using API.Services.YearyInfoService;
    using Microsoft.EntityFrameworkCore;

    public class ExchangeRateModel : IExchangeRateModel
    {
        private readonly IYearInfoService infoService;
        private readonly RateDBContext context;
        private readonly RateCache rateCache;
        private readonly IIntegrationJob integrationJob;

        public ExchangeRateModel(
            IYearInfoService infoService,
            RateDBContext context,
            RateCache rateCache)
        {
            this.infoService = infoService;
            this.context = context;
            this.rateCache = rateCache;
        }

        public virtual async Task<ExchangeRateStatisticInfo> GetExchangeRates(DateTime startDate, DateTime endDate, string code)
        {
            var cachedResult = this.rateCache.GetExchange(code, startDate, endDate);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            (double? median, double? min, double? max) = this.GetExchangeRatesFromDB(startDate, endDate, code);

            // усилить проверку - данных может не быть только на части интервала
            if (!median.HasValue)
            {
                if (startDate.Year != endDate.Year)
                {
                    await this.integrationJob.FillCurrentRate(startDate);
                }

                await this.integrationJob.FillCurrentRate(endDate);

                (median, min, max) = this.GetExchangeRatesFromDB(startDate, endDate, code);
            }

            cachedResult = new ExchangeRateStatisticInfo
            {
                Code = code,
                MaxRate = max ?? 0,
                MedianRate = median ?? 0,
                MinRate = min ?? 0
            };
            this.rateCache.AddExchange(cachedResult, startDate, endDate);

            return cachedResult;
        }

        protected internal virtual (double? median, double? min, double? max) GetExchangeRatesFromDB(DateTime startDate, DateTime endDate, string code)
        {
            var codeParam = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@code",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Size = 450,
                Value = code
            };
            var startDateParam = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@startDate",
                SqlDbType = SqlDbType.DateTime2,
                Direction = ParameterDirection.Input,
                Value = startDate
            };
            var endDateParam = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@endDate",
                SqlDbType = SqlDbType.DateTime2,
                Direction = ParameterDirection.Input,
                Value = endDate
            };

            var median = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@median",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Output
            };

            var max = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@max",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Output
            };

            var min = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@min",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Output
            };

            this.context.Database
                .ExecuteSqlCommand(
                "GetInfoForRate @code, @startDate, @endDate, @median OUT, @max OUT, @min OUT",
                codeParam,
                startDateParam,
                endDateParam,
                median,
                max,
                min);

            return (median.Value as double?, min.Value as double?, max.Value as double?);
        }
    }
}
