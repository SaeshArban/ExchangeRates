namespace API
{
    using System;
    using System.Linq;
    using API.Cache;
    using API.Entities;
    using API.Jobs;
    using API.Models.ExchangeRateModels;
    using API.Models.ExchangeReportModel;
    using API.Presenters.MonthExchangeRateReportPresenters;
    using API.Presenters.YearStatisticPresenters;
    using API.Services.DailyInfoService;
    using API.Services.YearyInfoService;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = this.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RateDBContext>(options =>
                options.UseSqlServer(connection));

            this.AddConfigSingleton(services);

            services.AddHttpClient<IDailyInfoService, DailyInfoService>();
            services.AddHttpClient<IYearInfoService, YearInfoService>();

            services.AddTransient<IExchangeRateModel, ExchangeRateModel>();
            services.AddTransient<IExchangeReportModel, ExchangeReportModel>();

            services.AddTransient<IMonthExchangeRateReportPresenter, MonthExchangeRateReportPresenter>();
            services.AddTransient<IYearStatisticPresenter, YearStatisticPresenter>();

            services.AddTransient<IIntegrationJob, IntegrationJob>();

            services.AddSingleton(typeof(RateCache), new RateCache());
            services.AddSingleton(typeof(IntegrationJob));

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            GlobalConfiguration.Configuration.UseActivator(new ContainerJobActivator(app.ApplicationServices));

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            BackgroundJob
                .Schedule<IIntegrationJob>(
                x => x.FillCurrentRateJob(),
                DateTimeOffset.Now.AddHours(int.Parse(this.Configuration["IntegrationPeriod"])));

            app.UseMvc();
        }

        private void AddConfigSingleton(IServiceCollection services)
        {
            var config = new Config
            {
                DailyHost = this.Configuration["Hosts:DailyHost"],
                YearHost = this.Configuration["Hosts:YearHost"],
                Currencies = this.Configuration.GetSection("Codes").AsEnumerable().Select(x => x.Value).Where(x => x != null).ToArray(),
                IntegrationPeriod = int.Parse(this.Configuration["IntegrationPeriod"])
            };
            services.AddSingleton(typeof(Config), config);
        }
    }
}
