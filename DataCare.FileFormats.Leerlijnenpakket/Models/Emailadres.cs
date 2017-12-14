namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;

    public class Emailadres : IEquatable<Emailadres>, IHaveAuditTrail<Emailadres>, IHaveBuilder<Emailadres, Emailadres.Builder>
    {
        public Emailadres(Guid id, bool isGeheim, EmailadresSoort emailadressoort, string emailadres, AuditTrail<Emailadres> auditTrail)
        {
            Id = id;
            IsGeheim = isGeheim;
            EmailadresSoort = emailadressoort;
            Adres = emailadres;
            AuditTrail = auditTrail;
        }

        [Key]
        public Guid Id { get; private set; }

        public bool IsGeheim { get; private set; }
        public EmailadresSoort EmailadresSoort { get; private set; }
        public string Adres { get; private set; }
        public AuditTrail<Emailadres> AuditTrail { get; private set; }

        private static Func<Emailadres, Emailadres, bool> keyEquals = Equality.KeyEquals<Emailadres>().Compile();

        public static bool operator !=(Emailadres one, Emailadres other)
        {
            return !(one == other);
        }

        public static bool operator ==(Emailadres left, Emailadres right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Emailadres);
        }

        public bool Equals(Emailadres other)
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
            return new ActualBuilder(null, false, default(EmailadresSoort), string.Empty);
        }

        public Builder ToBuilder()
        {
            return new ActualBuilder(this, IsGeheim, EmailadresSoort, Adres);
        }

        private class ActualBuilder : Builder
        {
            internal ActualBuilder(
                Emailadres basis,
                bool isGeheim,
                EmailadresSoort emailadresSoort,
                string adres)
                : base(basis, isGeheim, emailadresSoort, adres)
            {
            }
        }

        public abstract class Builder : IAmBuilderFor<Emailadres>
        {
            private readonly Emailadres basis;
            private readonly bool isGeheim;
            private readonly EmailadresSoort emailadresSoort;
            private readonly string adres;

            protected Builder(
                Emailadres basis,
                bool isGeheim,
                EmailadresSoort emailadresSoort,
                string adres)
            {
                this.basis = basis;
                this.isGeheim = isGeheim;
                this.emailadresSoort = emailadresSoort;
                this.adres = adres;
            }

            public bool IsValid()
            {
                return emailadresSoort != null && !string.IsNullOrWhiteSpace(adres);
            }

            public Builder Emailadres(string newAdres)
            {
                return new ActualBuilder(basis, isGeheim, emailadresSoort, newAdres);
            }

            public Builder IsGeheim(bool newValue)
            {
                return new ActualBuilder(basis, newValue, emailadresSoort, adres);
            }

            public Builder EmailadresSoort(EmailadresSoort newValue)
            {
                Contract.Requires(newValue != null);
                return new ActualBuilder(basis, isGeheim, newValue, adres);
            }

            public Emailadres Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Emailadres(Guid.NewGuid(), isGeheim, emailadresSoort, adres, new AuditTrail<Emailadres>(basis));
            }
        }

        public class EmailadresComparer : IEqualityComparer<Emailadres>
        {
            public bool Equals(Emailadres x, Emailadres y)
            {
                return x.Adres == y.Adres;
            }

            public int GetHashCode(Emailadres obj)
            {
                return typeof(Emailadres).GetHashCode();
            }
        }
    }
}