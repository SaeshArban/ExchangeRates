namespace API.Jobs
{
    using System;
    using System.Threading.Tasks;

    public interface IIntegrationJob
    {
        Task FillCurrentRateJob();

        Task FillCurrentRate(DateTime time);
    }
}
