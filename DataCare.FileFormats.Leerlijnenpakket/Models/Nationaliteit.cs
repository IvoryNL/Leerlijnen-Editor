namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataCare.Utilities;
    using Microsoft.FSharp.Core;

    public class Nationaliteit : IEquatable<Nationaliteit>
    {
        /// <summary>
        /// nationaleteit op basis van de GBA nationaliteiten tabel
        /// </summary>
        /// <param name="code"></param>
        /// <param name="naam"></param>
        /// <param name="startdatum"></param>
        /// <param name="?"></param>
        /// <param name="einddatum"></param>
        public Nationaliteit(string code, string naam, FSharpOption<DateTime> startdatum, FSharpOption<DateTime> einddatum)
        {
            Code = code;
            Naam = naam;
            Startdatum = startdatum;
            Einddatum = einddatum;
        }

        [Key]
        public string Code { get; private set; }

        public string Naam { get; private set; }
        public FSharpOption<DateTime> Startdatum { get; private set; }
        public FSharpOption<DateTime> Einddatum { get; private set; }

        #region Equality

        public static bool operator !=(Nationaliteit one, Nationaliteit other)
        {
            return !(one == other);
        }

        public static bool operator ==(Nationaliteit left, Nationaliteit right)
        {
            return keyEquals(left, right);
        }

        public override bool Equals(object obj)
        {
            return Equality.Of(this, obj);
        }

        public bool Equals(Nationaliteit other)
        {
            return this == other;
        }

        private static Func<Nationaliteit, Nationaliteit, bool> keyEquals = Equality.KeyEquals<Nationaliteit>().Compile();

        private int? hashcode;

        public override int GetHashCode()
        {
            if (this.hashcode == null)
            {
                this.hashcode = Equality.GetHashCodeKey(this);
            }

            return (int)this.hashcode;
        }

        public class ValueComparer : IEqualityComparer<Nationaliteit>
        {
            public bool Equals(Nationaliteit x, Nationaliteit y)
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
                    x.Code == y.Code &&
                    x.Einddatum == y.Einddatum &&
                    x.Naam == y.Naam &&
                    x.Startdatum == y.Startdatum;
            }

            public int GetHashCode(Nationaliteit obj)
            {
                return typeof(Nationaliteit).GetHashCode();
            }
        }

        #endregion Equality
    }
}