namespace API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using API.Models.ExchangeReportModel;
    using API.Presenters.MonthExchangeRateReportPresenters;
    using API.Presenters.YearStatisticPresenters;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IExchangeReportModel exchangeReportModel;
        private readonly IMonthExchangeRateReportPresenter monthExchangeRateReportPresenter;
        private readonly IYearStatisticPresenter yearStatisticPresenter;

        public RateController(
            IExchangeReportModel exchangeReportModel,
            IMonthExchangeRateReportPresenter monthExchangeRateReportPresenter,
            IYearStatisticPresenter yearStatisticPresenter)
        {
            this.exchangeReportModel = exchangeReportModel;
            this.monthExchangeRateReportPresenter = monthExchangeRateReportPresenter;
            this.yearStatisticPresenter = yearStatisticPresenter;
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> Get(int year, [FromQuery] string type)
        {
            var report = await this.exchangeReportModel.GetYearStatistic(new DateTime(year, 1, 1));

            if (type.ToLower() == "json")
            {
                return new JsonResult(report);
            }

            if (type.ToLower() == "txt")
            {
                return this.Ok(this.yearStatisticPresenter.GetString(report));
            }

            return this.BadRequest();
        }

        [HttpGet("{month}.{year}")]
        public async Task<IActionResult> Get(int year, int month, [FromQuery] string type)
        {
            var report = await this.exchangeReportModel.GetMonthReport(new DateTime(year, month, 1));

            if (type.ToLower() == "json")
            {
                return new JsonResult(report);
            }

            if (type.ToLower() == "txt")
            {
                return this.Ok(this.monthExchangeRateReportPresenter.GetString(report));
            }

            return this.BadRequest();
        }
    }
}
