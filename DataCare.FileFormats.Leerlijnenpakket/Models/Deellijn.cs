namespace DataCare.Model.Onderwijsinhoudelijk.Leerlijnen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;

    public sealed class Deellijn : IEquatable<Deellijn>, IHaveAuditTrail<Deellijn>, IHaveBuilder<Deellijn, Deellijn.Builder>
    {
        public Deellijn(
            Guid id,
            string deelgebied,
            IEnumerable<Niveautrede> niveautreden,
            IEnumerable<Doel> hoofddoelen,
            AuditTrail<Deellijn> auditTrail)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(deelgebied), "deelgebied is ongeldig");
            Contract.Requires(niveautreden != null, "niveautreden is null");
            Contract.Requires(niveautreden == null || Contract.ForAll(niveautreden, niveautrede => niveautrede != null), "niveautreden is null");
            Contract.Requires(hoofddoelen != null, "hoofddoelen is null");
            Contract.Requires(
                hoofddoelen == null ||
                Contract.ForAll(hoofddoelen, hoofddoel => niveautreden.SelectMany(nt => nt.Doelen).Contains(hoofddoel)),
                "Tenminste 1 hoofddoel komt niet voor in een niveautrede binnen de deellijn");
            Contract.Requires(auditTrail != null);

            Id = id;
            Deelgebied = deelgebied;
            Niveautreden = niveautreden;
            Hoofddoelen = hoofddoelen;
            AuditTrail = auditTrail;
        }

        [Key]
        public Guid Id { get; private set; }

        public string Deelgebied { get; private set; }

        public IEnumerable<Niveautrede> Niveautreden { get; private set; }
        public IEnumerable<Doel> Hoofddoelen { get; private set; }
        public AuditTrail<Deellijn> AuditTrail { get; private set; }

        #region Equality

        public static bool operator !=(Deellijn one, Deellijn other)
        {
            return !(one == other);
        }

        public static bool operator ==(Deellijn left, Deellijn right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Deellijn);
        }

        public bool Equals(Deellijn other)
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

        public IEnumerable<Doel> GetHoofddoelenVanNiveau(int niveau)
        {
            // Haal alle doelen uit de niveautredes van een bepaald niveau en daarvan een doorsnede met de hoofddoelen.
            var doelenVanNiveautredes = GetAlleDoelenVanNiveau(niveau);
            return Hoofddoelen.Join(
                doelenVanNiveautredes,
                hd => hd.Naam,
                d => d.Naam,
                (hd, d) => d);
        }

        public IEnumerable<Doel> GetAlleDoelen()
        {
            return Niveautreden.SelectMany(n => n.Doelen);
        }

        private IEnumerable<DoelMetContext> niveaudoelen;

        public IEnumerable<DoelMetContext> AlleNiveauDoelen
        {
            get
            {
                return niveaudoelen ??
                    (niveaudoelen = Niveautreden.SelectMany(n => n.Doelen.Select(d => new DoelMetContext(d, n.Niveau, this.Hoofddoelen.Contains(d)))).ToList());
            }
        }

        public IEnumerable<Doel> GetAlleDoelenVanNiveau(int niveau)
        {
            // Haal alle doelen uit de niveautredes van een bepaald niveau en daarvan een doorsnede met de hoofddoelen.
            return Niveautreden.Where(n => n.Niveau == niveau).SelectMany(d => d.Doelen);
        }

        private int? maxNiveau;
        public int MaxNiveau
        {
            get
            {
                if (!maxNiveau.HasValue)
                {
                    if (Niveautreden.Any())
                    {
                        maxNiveau = Niveautreden.Select(n => n.Niveau).Max();
                    }
                    else
                    {
                        maxNiveau = 0;
                    }
                }

                return maxNiveau.Value;
            }
        }

        private int? minNiveau;
        public int MinNiveau
        {
            get
            {

                if (!minNiveau.HasValue)
                {
                    if (Niveautreden.Any())
                    {
                        minNiveau = Niveautreden.Select(n => n.Niveau).Min();
                    }
                    else
                    {
                        minNiveau = 0;
                    }
                }

                return minNiveau.Value;
            }
        }

        public class ValueComparer : IEqualityComparer<Deellijn>
        {
            public bool Equals(Deellijn x, Deellijn y)
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
                    x.Deelgebied == y.Deelgebied &&
                    x.Niveautreden == y.Niveautreden &&
                    x.Hoofddoelen == y.Hoofddoelen;
            }

            public int GetHashCode(Deellijn obj)
            {
                return typeof(Deellijn).GetHashCode();
            }
        }

        public static IEqualityComparer<Deellijn> NaamComparer = new NaamComparerImpl();

        private class NaamComparerImpl : IEqualityComparer<Deellijn>
        {
            public bool Equals(Deellijn x, Deellijn y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Deelgebied == y.Deelgebied;
            }

            public int GetHashCode(Deellijn obj)
            {
                return typeof(Deellijn).GetHashCode();
            }
        }

        public Builder ToBuilder()
        {
            return new ActualBuilder(this, Deelgebied, Niveautreden, Hoofddoelen);
        }

        private class ActualBuilder : Builder
        {
            internal ActualBuilder(
                Deellijn basis,
                string deelgebied,
                IEnumerable<Niveautrede> niveautreden,
                IEnumerable<Doel> hoofddoelen)
                : base(basis, deelgebied, niveautreden, hoofddoelen)
            {
            }
        }

        public abstract class Builder : IAmBuilderFor<Deellijn>
        {
            private readonly Deellijn basis;
            private readonly string deelgebied;
            private readonly IEnumerable<Niveautrede> niveautreden;
            private readonly IEnumerable<Doel> hoofddoelen;

            protected Builder(
                Deellijn basis,
                string deelgebied,
                IEnumerable<Niveautrede> niveautreden,
                IEnumerable<Doel> hoofddoelen)
            {
                this.basis = basis;
                this.deelgebied = deelgebied;
                this.niveautreden = niveautreden;
                this.hoofddoelen = hoofddoelen;
            }

            public Builder Niveautreden(IEnumerable<Niveautrede> newNiveautreden)
            {
                return new ActualBuilder(basis, deelgebied, newNiveautreden, hoofddoelen);
            }

            public Builder Hoofddoelen(IEnumerable<Doel> newHoofddoelen)
            {
                return new ActualBuilder(basis, deelgebied, niveautreden, newHoofddoelen);
            }

            public Deellijn Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Deellijn(Guid.NewGuid(), deelgebied, niveautreden, hoofddoelen, new AuditTrail<Deellijn>(basis));
            }

            public bool IsValid()
            {
                return !string.IsNullOrWhiteSpace(deelgebied);
            }
        }
    }
}