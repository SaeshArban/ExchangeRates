namespace API.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class BaseInfoService<T> : IBaseInfoService<T>
        where T : class
    {
        private readonly HttpClient httpClient;

        public BaseInfoService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> GetByDate(DateTime date)
        {
            var url = this.GetUrl(date);
            var responseString = await this.httpClient.GetStringAsync(url);

            var data = this.Parse(responseString);
            return data;
        }

        protected internal abstract T Parse(string input);

        protected internal abstract string GetUrl(DateTime date);
    }
}
