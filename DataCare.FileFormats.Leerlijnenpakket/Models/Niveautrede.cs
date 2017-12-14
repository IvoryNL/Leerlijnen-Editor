namespace DataCare.Model.Onderwijsinhoudelijk.Leerlijnen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;

    public sealed class Niveautrede : IEquatable<Niveautrede>, IHaveAuditTrail<Niveautrede>, IHaveBuilder<Niveautrede, Niveautrede.Builder>
    {
        public Niveautrede(
            Guid id,
            int niveau,
            IEnumerable<Doel> doelen,
            AuditTrail<Niveautrede> auditTrail)
        {
            Contract.Requires(niveau >= 1 && niveau <= 16, "Niveau ligt niet tussen 1 en 16");
            Contract.Requires(doelen != null, "Doelen is null");
            Contract.Requires(doelen == null || Contract.ForAll(doelen, doel => doel != null), "doel in Doelen is null");
            Contract.Requires(auditTrail != null);

            Id = id;
            Niveau = niveau;
            AuditTrail = auditTrail;
            Doelen = doelen;
        }

        [Key]
        public Guid Id { get; private set; }

        public int Niveau { get; private set; }

        public IEnumerable<Doel> Doelen { get; private set; }

        public AuditTrail<Niveautrede> AuditTrail { get; private set; }

        #region Equality

        private static Func<Niveautrede, Niveautrede, bool> keyEquals = Equality.KeyEquals<Niveautrede>().Compile();

        public static bool operator !=(Niveautrede one, Niveautrede other)
        {
            return !(one == other);
        }

        public static bool operator ==(Niveautrede left, Niveautrede right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Niveautrede);
        }

        public bool Equals(Niveautrede other)
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
            return new ActualBuilder(this, Niveau, Doelen);
        }

        private class ActualBuilder : Builder
        {
            internal ActualBuilder(
                Niveautrede basis,
                int niveau,
                IEnumerable<Doel> doelen)
                : base(basis, niveau, doelen)
            {
            }
        }

        public abstract class Builder : IAmBuilderFor<Niveautrede>
        {
            private readonly Niveautrede basis;
            private readonly int niveau;
            private readonly IEnumerable<Doel> doelen;

            protected Builder(
                Niveautrede basis,
                int niveau,
                IEnumerable<Doel> doelen)
            {
                this.basis = basis;
                this.niveau = niveau;
                this.doelen = doelen;
            }

            public Builder Doelen(IEnumerable<Doel> newDoelen)
            {
                return new ActualBuilder(basis, niveau, newDoelen);
            }

            public Builder Niveau(int niveau)
            {
                return new ActualBuilder(basis, niveau, doelen);
            }

            public Niveautrede Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Niveautrede(Guid.NewGuid(), niveau, doelen, new AuditTrail<Niveautrede>(basis));
            }

            public bool IsValid()
            {
                return true;
            }
        }
    }
}