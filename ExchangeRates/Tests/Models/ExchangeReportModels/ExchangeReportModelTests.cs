using API;
using API.DataTransferObjects.ExchangeRateReport;
using API.Models.ExchangeRateModels;
using API.Models.ExchangeReportModel;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Models.ExchangeReportModels
{
    public class ExchangeReportModelTests
    {
        [Fact]
        public async Task GetYearStatistic_SomeYear_ShouldCallGetMonthReport12Times()
        {
            // Arrange
            // не DateTime.Now потому, что если код внутри зависит от времени - тест может иногда падать
            var date = new DateTime(2019, 2, 12);
            var config = new Config();
            var rateModel = new Mock<IExchangeRateModel>();
            var reportModel = new Mock<ExchangeReportModel>(config, rateModel.Object);

            reportModel
                .Setup(x => x.GetMonthReport(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new MonthExchangeRateReport()));

            reportModel
                .Setup(x => x.GetYearStatistic(It.IsAny<DateTime>()))
                .CallBase();

            // Act
            var result = await reportModel.Object.GetYearStatistic(date);

            // Assert
            reportModel.Verify(x => x.GetMonthReport(It.IsAny<DateTime>()), Times.Exactly(12));
            result.Date.Should().Be(date);
            result.ByMonth.Should().HaveCountLessOrEqualTo(12);
        }
    }
}
