namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.FSharp.Core;

    public class PersoonComparer : IEqualityComparer<Persoon>
    {
        public bool Equals(Persoon x, Persoon y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return
                FSharpOption<DateTime>.Equals(x.Geboortedatum, y.Geboortedatum) &&
                string.Equals(x.Naam, y.Naam, StringComparison.InvariantCultureIgnoreCase) &&
                FSharpOption<Geslacht>.Equals(x.Geslacht, y.Geslacht);
        }

        public int GetHashCode(Persoon obj)
        {
            return typeof(Persoon).GetHashCode();
        }
    }
}