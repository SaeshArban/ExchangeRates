namespace API.DataTransferObjects.ExchangeRateReport
{
    public class ExchangeRateStatisticInfo
    {
        public string Code { get; set; }

        public double MinRate { get; set; }

        public double MaxRate { get; set; }

        public double MedianRate { get; set; }
    }
}
