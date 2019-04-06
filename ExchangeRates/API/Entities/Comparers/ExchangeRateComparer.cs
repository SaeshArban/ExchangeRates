namespace API.Entities.Comparers
{
    using System.Collections.Generic;

    public class ExchangeRateComparer : IEqualityComparer<ExchangeRate>
    {
        public bool Equals(ExchangeRate x, ExchangeRate y)
        {
            return x.Code == y.Code && x.Date == y.Date;
        }

        public int GetHashCode(ExchangeRate obj)
        {
            return obj.Code.GetHashCode() ^ obj.Date.GetHashCode();
        }
    }
}
