namespace API.Presenters.MonthExchangeRateReportPresenters
{
    using System;
    using System.Linq;
    using System.Text;
    using API.DataTransferObjects.ExchangeRateReport;

    public class MonthExchangeRateReportPresenter : IMonthExchangeRateReportPresenter
    {
        public string GetString(MonthExchangeRateReport report)
        {
            var sb = new StringBuilder();

            this.AddHeader(report, sb);
            sb.Append("Week periods:");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            foreach (var week in report.ByWeek)
            {
                sb.Append($"{week.WeekStart.Day}...{week.WeekEndDate.Day}: ");

                foreach (var info in week.Infos)
                {
                    sb.Append($"{info.Code} - max: {info.MaxRate}, min: {info.MinRate}, median: {info.MedianRate}; ");
                }

                if (!week.Infos.Any())
                {
                    sb.Append("There is no existing data");
                }

                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        protected internal virtual void AddHeader(MonthExchangeRateReport report, StringBuilder sb)
        {
            sb.Append("Year: ");
            sb.Append(report.ReportDate.Year);
            sb.Append(", month: ");
            sb.Append(report.ReportDate.ToString("MMMM"));
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
        }
    }
}
