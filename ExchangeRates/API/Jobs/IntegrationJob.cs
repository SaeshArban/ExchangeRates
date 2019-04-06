namespace API.Jobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using API.Entities;
    using API.Entities.Comparers;
    using API.Services.DailyInfoService;
    using Hangfire;

    public class IntegrationJob : IIntegrationJob
    {
        private readonly IDailyInfoService infoService;
        private readonly RateDBContext context;
        private readonly Config config;

        public IntegrationJob(
            IDailyInfoService infoService,
            RateDBContext context,
            Config config)
        {
            this.infoService = infoService;
            this.context = context;
            this.config = config;
        }

        public async Task FillCurrentRateJob()
        {
            await this.FillCurrentRate(DateTime.Now);
            BackgroundJob
                .Schedule<IIntegrationJob>(
                x => x.FillCurrentRateJob(),
                DateTimeOffset.Now.AddHours(this.config.IntegrationPeriod));
        }

        public async Task FillCurrentRate(DateTime time)
        {
            var info = await this.infoService.GetByDate(time);
            var comparer = new ExchangeRateComparer();

            var exists = this.context.ExchangeRates.Where(x => info.Contains(x, comparer)).ToList();
            var dataForSave = info.Except(exists, comparer);

            await this.context.ExchangeRates.AddRangeAsync(dataForSave);
            await this.context.SaveChangesAsync();
        }
    }
}
