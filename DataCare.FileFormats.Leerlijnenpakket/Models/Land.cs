namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataCare.Utilities;
    using Microsoft.FSharp.Core;

    public class Land : IEquatable<Land>
    {
        public Land(string code, string omschrijving, FSharpOption<DateTime> startdatum, FSharpOption<DateTime> einddatum)
        {
            Code = code;
            Omschrijving = omschrijving;
            Startdatum = startdatum;
            Einddatum = einddatum;
        }

        [Key]
        public string Code { get; private set; }

        public string Omschrijving { get; private set; }
        public FSharpOption<DateTime> Startdatum { get; private set; }
        public FSharpOption<DateTime> Einddatum { get; private set; }

        #region Equality

        private static Func<Land, Land, bool> keyEquals = Equality.KeyEquals<Land>().Compile();

        public static bool operator !=(Land one, Land other)
        {
            return !(one == other);
        }

        public static bool operator ==(Land left, Land right)
        {
            return keyEquals(left, right);
        }

        public override bool Equals(object obj)
        {
            return Equality.Of(this, obj);
        }

        public bool Equals(Land other)
        {
            return this == other;
        }

        private int? hashcode;

        public override int GetHashCode()
        {
            if (this.hashcode == null)
            {
                this.hashcode = Equality.GetHashCodeKey(this);
            }

            return (int)this.hashcode;
        }

        #endregion Equality
    }
}