namespace DataCare.Model.Onderwijsinhoudelijk.Leerlijnen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;
    using Microsoft.FSharp.Core;

    public class Leerlijnenpakket : IHaveAuditTrail<Leerlijnenpakket>, IEquatable<Leerlijnenpakket>
    {
        public Leerlijnenpakket(
           Guid id,
            Guid nummer,
            string naam,
            DateTime invuldatum,
            bool definitief,
            IEnumerable<Leerlijn> leerlijnen,
            AuditTrail<Leerlijnenpakket> auditTrail)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(naam), "Naam leerlijnenpakket is null of leeg");
            Contract.Requires(leerlijnen != null, "Leerlijnen is null");

            // vakgebieden waarnaar de leerlijnen verwijzen moeten uniek zijn binnen het leerlijnenpakket
            Contract.Requires(
                leerlijnen == null ||
                Contract.ForAll(
                    leerlijnen,
                    leerlijn => !leerlijnen.Where(ll => ll != leerlijn).Select(ll => ll.Vakgebied)
                                     .Contains(leerlijn.Vakgebied)));
            Contract.Requires(auditTrail != null);

            Id = id;
            Nummer = nummer;
            Naam = naam;
            Invuldatum = invuldatum;
            Definitief = definitief;
            Leerlijnen = leerlijnen;
            AuditTrail = auditTrail;
        }

        [Key]
        public Guid Id { get; private set; }

        public Guid Nummer { get; private set; }

        public string Naam { get; private set; }

        public DateTime Invuldatum { get; private set; }

        public bool Definitief { get; private set; }

        public IEnumerable<Leerlijn> Leerlijnen { get; private set; }

        public virtual AuditTrail<Leerlijnenpakket> AuditTrail { get; private set; }

        #region Equality

        public static bool operator !=(Leerlijnenpakket one, Leerlijnenpakket other)
        {
            return !(one == other);
        }

        public static bool operator ==(Leerlijnenpakket left, Leerlijnenpakket right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Leerlijnenpakket);
        }

        public bool IsVersionOf(Leerlijnenpakket other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Nummer == other.Nummer;
        }

        public bool Equals(Leerlijnenpakket other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion Equality

        public Builder ToBuilder()
        {
            return new ActualBuilder(this, Nummer, Naam, Invuldatum, Definitief, Leerlijnen);
        }

        private sealed class ActualBuilder : Builder
        {
            public ActualBuilder(
                Leerlijnenpakket basis,
                Guid nummer,
                string naam,
                DateTime invuldatum,
                bool definitief,
                IEnumerable<Leerlijn> leerlijnen)

                : base(
               basis,
               nummer,
               naam,
               invuldatum,
               definitief,
               leerlijnen)
            {
            }
        }

        public abstract class Builder : IAmBuilderFor<Leerlijnenpakket>
        {
            private readonly Leerlijnenpakket basis;
            private readonly Guid nummer;
            private readonly string naam;
            private readonly DateTime invuldatum;
            private readonly IEnumerable<Leerlijn> leerlijnen;
            private readonly bool definitief;

            protected Builder(
                Leerlijnenpakket basis,
                Guid nummer,
                string naam,
                DateTime invuldatum,
                bool definitief,
                IEnumerable<Leerlijn> leerlijnen)
            {
                this.basis = basis;
                this.nummer = nummer;
                this.naam = naam;
                this.invuldatum = invuldatum;
                this.definitief = definitief;
                this.leerlijnen = leerlijnen;
            }

            public Leerlijnenpakket Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Leerlijnenpakket(
                    Guid.NewGuid(),
                    nummer,
                    naam,
                    invuldatum,
                    definitief,
                    leerlijnen,
                    new AuditTrail<Leerlijnenpakket>(basis));
            }

            private bool IsValid()
            {
                // Er moet een leerlijnenpakketDefinitie aanwezig zijn!
                return nummer != Guid.Empty
                    && !string.IsNullOrWhiteSpace(naam);
            }

            public Builder Naam(string newNaam)
            {
                return new ActualBuilder(basis, nummer, newNaam, invuldatum, definitief, leerlijnen);
            }

            public Builder Invuldatum(DateTime newInvuldatum)
            {
                return new ActualBuilder(basis, nummer, naam, newInvuldatum, definitief, leerlijnen);
            }

            public Builder Definitief(bool newDefinitief)
            {
                return new ActualBuilder(basis, nummer, naam, invuldatum, newDefinitief, leerlijnen);
            }

            public Builder Leerlijnen(IEnumerable<Leerlijn> newLeerlijnen)
            {
                return new ActualBuilder(basis, nummer, naam, invuldatum, definitief, newLeerlijnen.Memoize());
            }

            public Builder Add(Leerlijn newLeerlijn)
            {
                return new ActualBuilder(basis, nummer, naam, invuldatum, definitief, leerlijnen.Concat(new[] { newLeerlijn }).Memoize());
            }

            public Builder Remove(Leerlijn oldLeerlijn)
            {
                return new ActualBuilder(basis, nummer, naam, invuldatum, definitief, leerlijnen.Where(l => l != oldLeerlijn).Memoize());
            }

            public Builder Replace(Leerlijn oldLeerlijn, Leerlijn newLeerlijn)
            {
                return new ActualBuilder(basis, nummer, naam, invuldatum, definitief, leerlijnen.Where(l => l != oldLeerlijn).Concat(new[] { newLeerlijn }).Memoize());
            }
        }

        public class MergeComparer : EqualityComparer<Leerlijnenpakket>
        {
            public override bool Equals(Leerlijnenpakket x, Leerlijnenpakket y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                // Conceptuele leerlijnenpakketversies van zelfde definitie zijn gelijk aan elkaar.
                return x.Nummer == y.Nummer
                    &&
                    ((!x.Definitief && !y.Definitief)
                        || ((x.Definitief && y.Definitief) && (x.Invuldatum == y.Invuldatum) && (x.Naam == y.Naam)));
            }

            public override int GetHashCode(Leerlijnenpakket obj)
            {
                if (obj == null)
                {
                    return 0;
                }

                return obj.Nummer.GetHashCode();
            }
        }
    }

    public static class LeerlijnenpakketHelpers
    {
        /// <summary>
        /// Neem van ieder leerlijnenpakket, per definitie, de nieuwste versie groter of gelijk aan peildatum en definitief of niet definitief.
        /// </summary>
        /// <param name="leerlijnenpakketten"></param>
        /// <param name="peildatum"></param>
        /// <returns></returns>

        public static Leerlijnenpakket GetLeerlijnenpakketOpDatum(
            this Leerlijnenpakket pakket,
            IEnumerable<Leerlijnenpakket> leerlijnenpakketten,
            bool definitief,
            DateTime peildatum)
        {
            var result = leerlijnenpakketten.Where(v => v.Nummer == pakket.Nummer && v.Definitief == definitief && v.Invuldatum <= peildatum);
            if (result.Any())
            {
                return result.MaxBy(l => l.Invuldatum).First();
            }
            return null;
        }

        public static Leerlijnenpakket GetLeerlijnenpakketOpDatum(
       this Leerlijnenpakket pakket,
       IEnumerable<Leerlijnenpakket> leerlijnenpakketten,
       DateTime peildatum)
        {
            var result = leerlijnenpakketten.Where(v => v.Nummer == pakket.Nummer && v.Invuldatum <= peildatum);
            if (result.Any())
            {
                return result.MaxBy(l => l.Invuldatum).First();
            }
            return null;
        }

        public static IEnumerable<Deellijn> GetDeellijnen(this Leerlijnenpakket leerlijnenpakket)
        {
            return leerlijnenpakket.Leerlijnen.SelectMany(l => l.Deellijnen).Memoize();
        }
    }
}