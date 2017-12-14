namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Deze klasse wordt gebruikt om de applicatie te vertellen of hij bezig is met laden van lijsten.
    /// Dit is om te voorkomen dat 'null' niet als laden wordt gezien.
    /// </summary>
    public class ILoading : IEnumerable<object>
    {
        private List<object> list;

        public ILoading()
        {
            this.list = new List<object>();
            list.Add("Gegevens laden...");
        }

        public IEnumerator<object> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}