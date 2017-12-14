namespace DataCare.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ISurrogateKey
    {
        Guid Nummer { get; }
    }

    public class SurrogaatKeyComparer<T> : IEqualityComparer<T> where T : ISurrogateKey
    {
        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }
            else
            {
                return x.Nummer == y.Nummer;
            }
        }

        public int GetHashCode(T obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            return obj.Nummer.GetHashCode();
        }
    }
}