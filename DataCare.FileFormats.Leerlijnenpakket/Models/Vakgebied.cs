namespace DataCare.Model.Onderwijsinhoudelijk.Leerlijnen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;

    public sealed class Vakgebied : IEquatable<Vakgebied>, IHaveAuditTrail<Vakgebied>
    {
        public Vakgebied(
            string naam,
            IEnumerable<string> deelgebieden,
            AuditTrail<Vakgebied> auditTrail)
        {
            Contract.Requires(deelgebieden != null);
            Contract.Requires(auditTrail != null);

            Naam = naam;
            this.Deelgebieden = deelgebieden.Distinct();
            AuditTrail = auditTrail;
        }

        [Key]
        public string Naam { get; private set; }

        public IEnumerable<string> Deelgebieden { get; private set; }
        public AuditTrail<Vakgebied> AuditTrail { get; private set; }

        #region Equality

        private static Func<Vakgebied, Vakgebied, bool> keyEquals = Equality.KeyEquals<Vakgebied>().Compile();

        public static bool operator !=(Vakgebied one, Vakgebied other)
        {
            return !(one == other);
        }

        public static bool operator ==(Vakgebied left, Vakgebied right)
        {
            return keyEquals(left, right);
        }

        public override bool Equals(object obj)
        {
            return Equality.Of(this, obj);
        }

        public bool Equals(Vakgebied other)
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

        public class ValueComparer : IEqualityComparer<Vakgebied>
        {
            public bool Equals(Vakgebied x, Vakgebied y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Naam == y.Naam && x.Deelgebieden == y.Deelgebieden;
            }

            public int GetHashCode(Vakgebied obj)
            {
                return typeof(Vakgebied).GetHashCode();
            }
        }
    }
}