namespace DataCare.Model.Onderwijsinhoudelijk.Leerlijnen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;

    public sealed class Leerlijn : IEquatable<Leerlijn>, IHaveAuditTrail<Leerlijn>, IHaveBuilder<Leerlijn, Leerlijn.Builder>
    {
        public Leerlijn(
            Guid id,
            Vakgebied vakgebied,
            IEnumerable<Deellijn> deellijnen,
            AuditTrail<Leerlijn> auditTrail)
        {
            Contract.Requires(vakgebied != null, "vakgebied is null");

            Contract.Requires(
                deellijnen != null &&
                Contract.ForAll(deellijnen, deellijn => deellijn != null),
                "deellijnen is null");
            Contract.Requires(
                deellijnen == null ||
                Contract.ForAll(
                    deellijnen,
                    deellijn => deellijn == null ||
                                deellijn.Deelgebied != null),
                "deelgebied in deellijn is null");
            Contract.Requires(
                deellijnen == null ||
                Contract.ForAll(
                    deellijnen,
                    deellijn => deellijn == null ||
                                deellijn.Deelgebied == null ||
                                !deellijnen.Where(dl => dl != deellijn).Select(dl => dl.Deelgebied)
                                     .Contains(deellijn.Deelgebied)),
                "deelgebieden waarnaar deellijnen verwijzen moeten uniek zijn binnen de leerlijn");
            Contract.Requires(auditTrail != null);

            Id = id;
            Vakgebied = vakgebied;
            Deellijnen = deellijnen;
            AuditTrail = auditTrail;
        }

        [Key]
        public Guid Id { get; private set; }

        public Vakgebied Vakgebied { get; private set; }
        public IEnumerable<Deellijn> Deellijnen { get; private set; }
        public AuditTrail<Leerlijn> AuditTrail { get; private set; }

        #region Equality

        public static bool operator !=(Leerlijn one, Leerlijn other)
        {
            return !(one == other);
        }

        public static bool operator ==(Leerlijn left, Leerlijn right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Leerlijn);
        }

        public bool Equals(Leerlijn other)
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

        public class VakgebiedComparer : IEqualityComparer<Leerlijn>
        {
            public bool Equals(Leerlijn x, Leerlijn y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Vakgebied == y.Vakgebied;
            }

            public int GetHashCode(Leerlijn obj)
            {
                return obj.Vakgebied.GetHashCode();
            }
        }

        public class ValueComparer : IEqualityComparer<Leerlijn>
        {
            public bool Equals(Leerlijn x, Leerlijn y)
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
                    new Vakgebied.ValueComparer().Equals(x.Vakgebied, y.Vakgebied) &&
                    x.Deellijnen.SetEquals(y.Deellijnen, new Deellijn.ValueComparer());
            }

            public int GetHashCode(Leerlijn obj)
            {
                return typeof(Leerlijn).GetHashCode();
            }
        }

        public static Builder Create()
        {
            return new ActualBuilder(
                null,
                new Vakgebied(string.Empty, Enumerable.Empty<string>(), new AuditTrail<Vakgebied>()),
                Enumerable.Empty<Deellijn>());
        }

        public Builder ToBuilder()
        {
            return new ActualBuilder(this, Vakgebied, Deellijnen);
        }

        private class ActualBuilder : Builder
        {
            internal ActualBuilder(
                Leerlijn basis,
                Vakgebied vakgebied,
                IEnumerable<Deellijn> deellijnen)
                : base(basis, vakgebied, deellijnen)
            {
            }
        }

        public abstract class Builder : IAmBuilderFor<Leerlijn>
        {
            private readonly Leerlijn basis;
            private readonly Vakgebied vakgebied;
            private readonly IEnumerable<Deellijn> deellijnen;

            protected Builder(
                Leerlijn basis,
                Vakgebied vakgebied,
                IEnumerable<Deellijn> deellijnen)
            {
                this.basis = basis;
                this.vakgebied = vakgebied;
                this.deellijnen = deellijnen;
            }

            public Builder Deellijnen(IEnumerable<Deellijn> newDeellijnen)
            {
                return new ActualBuilder(basis, vakgebied, newDeellijnen);
            }

            public Leerlijn Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Leerlijn(
                    Guid.NewGuid(),
                    vakgebied,
                    deellijnen,
                    new AuditTrail<Leerlijn>(basis));
            }

            public bool IsValid()
            {
                return true;
            }
        }
    }
}