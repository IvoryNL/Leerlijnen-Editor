namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;

    public class Telefoonnummer : IEquatable<Telefoonnummer>, IHaveAuditTrail<Telefoonnummer>, IHaveBuilder<Telefoonnummer, Telefoonnummer.Builder>
    {
        public Telefoonnummer(Guid id, string nummer, bool isGeheim, TelefoonnummerSoort telefoonnummerSoort, AuditTrail<Telefoonnummer> auditTrail)
        {
            Id = id;
            Nummer = nummer;
            IsGeheim = isGeheim;
            TelefoonnummerSoort = telefoonnummerSoort;
            AuditTrail = auditTrail;
        }

        [Key]
        public Guid Id { get; private set; }

        public string Nummer { get; private set; }
        public bool IsGeheim { get; private set; }
        public TelefoonnummerSoort TelefoonnummerSoort { get; private set; }
        public AuditTrail<Telefoonnummer> AuditTrail { get; private set; }

        #region Equality

        public static bool operator !=(Telefoonnummer one, Telefoonnummer other)
        {
            return !(one == other);
        }

        public static bool operator ==(Telefoonnummer left, Telefoonnummer right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Telefoonnummer);
        }

        public bool Equals(Telefoonnummer other)
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

        public static Builder Create()
        {
            return new ActualBuilder(
                null,
                false,
                default(TelefoonnummerSoort),
                string.Empty);
        }

        public Builder ToBuilder()
        {
            return new ActualBuilder(
                this,
                IsGeheim,
                this.TelefoonnummerSoort,
                this.Nummer);
        }

        #endregion Equality

        public class NummerComparer : IEqualityComparer<Telefoonnummer>
        {
            public bool Equals(Telefoonnummer x, Telefoonnummer y)
            {
                return x.Nummer == y.Nummer;
            }

            public int GetHashCode(Telefoonnummer obj)
            {
                return typeof(Telefoonnummer).GetHashCode();
            }
        }

        public abstract class Builder : IAmBuilderFor<Telefoonnummer>
        {
            private readonly string nummer;
            private readonly bool isGeheim;
            private readonly TelefoonnummerSoort telefoonnummersoort;
            private readonly Telefoonnummer basis;

            public static Builder Create()
            {
                return new ActualBuilder(
                    null,
                    false,
                    default(TelefoonnummerSoort),
                    string.Empty);
            }

            protected Builder(
                Telefoonnummer basis,
                bool isGeheim,
                TelefoonnummerSoort telefoonnummersoort,
                string nummer)
            {
                this.basis = basis;
                this.isGeheim = isGeheim;
                this.telefoonnummersoort = telefoonnummersoort;
                this.nummer = nummer;
            }

            public Builder IsGeheim(bool newValue)
            {
                return new ActualBuilder(basis, newValue, telefoonnummersoort, nummer);
            }

            public Builder TelefoonnummerSoort(TelefoonnummerSoort newValue)
            {
                Contract.Requires(newValue != null);

                return new ActualBuilder(basis, isGeheim, newValue, nummer);
            }

            public Builder Nummer(string newValue)
            {
                Contract.Requires(newValue != null);

                return new ActualBuilder(basis, isGeheim, telefoonnummersoort, newValue);
            }

            public bool IsValid()
            {
                return true;
            }

            public Telefoonnummer Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Telefoonnummer(
                    Guid.NewGuid(),
                    nummer,
                    isGeheim,
                    telefoonnummersoort,
                    new AuditTrail<Telefoonnummer>(basis));
            }
        }

        private class ActualBuilder : Builder
        {
            internal ActualBuilder(
                Telefoonnummer basis,
                bool isGeheim,
                TelefoonnummerSoort telefoonnummersoort,
                string nummer)
                : base(basis, isGeheim, telefoonnummersoort, nummer)
            {
            }
        }
    }
}