namespace API.Models.ExchangeRateModels
{
    using System;
    using System.Threading.Tasks;
    using API.DataTransferObjects.ExchangeRateReport;

    public interface IExchangeRateModel
    {
        Task<ExchangeRateStatisticInfo> GetExchangeRates(DateTime startDate, DateTime endDate, string code);
    }
}
