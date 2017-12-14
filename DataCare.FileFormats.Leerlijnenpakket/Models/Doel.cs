namespace DataCare.Model.Onderwijsinhoudelijk.Leerlijnen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;

    public sealed class Doel : IEquatable<Doel>, IHaveAuditTrail<Doel>, IHaveBuilder<Doel, Doel.Builder>
    {
        public Doel(
            string naam,
            AuditTrail<Doel> auditTrail)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(naam), "naam is invalid");
            Naam = naam;
            AuditTrail = auditTrail;
        }

        [Key]
        public string Naam { get; private set; }

        public AuditTrail<Doel> AuditTrail { get; private set; }

        private static Func<Doel, Doel, bool> keyEquals = Equality.KeyEquals<Doel>().Compile();

        public static bool operator !=(Doel one, Doel other)
        {
            return !(one == other);
        }

        public static bool operator ==(Doel left, Doel right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Doel);
        }

        public bool Equals(Doel other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Naam == other.Naam;
        }

        private int? hashcode;

        public override int GetHashCode()
        {
            if (this.hashcode == null)
            {
                this.hashcode =
                    this.Naam != null
                    ? this.Naam.GetHashCode()
                    : 0;
            }

            return (int)this.hashcode;
        }

        public Builder ToBuilder()
        {
            return new ActualBuilder(this, Naam);
        }

        private class ActualBuilder : Builder
        {
            internal ActualBuilder(
                Doel basis,
                string naam)
                : base(basis, naam)
            {
            }
        }

        public abstract class Builder : IAmBuilderFor<Doel>
        {
            private readonly Doel basis;
            private readonly string naam;

            protected Builder(
                Doel basis,
                string naam)
            {
                this.basis = basis;
                this.naam = naam;
            }

            public Builder Naam(string newNaam)
            {
                return new ActualBuilder(basis, newNaam);
            }

            public Doel Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Doel(naam, new AuditTrail<Doel>(basis));
            }

            public bool IsValid()
            {
                return !string.IsNullOrWhiteSpace(naam);
            }
        }
    }

    public class DoelMetContext
    {
        public DoelMetContext(Doel doel, int niveau, bool isHoofddoel)
        {
            this.Doel = doel;
            this.Niveau = niveau;
            this.IsHoofddoel = isHoofddoel;
        }

        public Doel Doel { get; private set; }
        public bool IsHoofddoel { get; private set; }
        public int Niveau { get; private set; }
    }

    public static class DoelHelper
    {
        /// <summary>
        /// Wordt een doel gezet in Behaald dan mag deze niet ook in NietBehaald staan.
        /// Wordt een doel gezet in NietBehaald dan mag deze niet ook in Behaald staan.
        /// </summary>
        /// <param name="given"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static IEnumerable<Doel> CheckMutualExclusive(this IEnumerable<Doel> given, IEnumerable<Doel> toCheck)
        {
            Contract.Requires(given != null && toCheck != null);

            return toCheck.Except(given).Memoize();
        }
    }
}