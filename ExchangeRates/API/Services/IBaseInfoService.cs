namespace API.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IBaseInfoService<T>
        where T : class
    {
        Task<T> GetByDate(DateTime date);
    }
}
