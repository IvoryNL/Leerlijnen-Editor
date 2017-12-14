namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataCare.Utilities;

    public class Adressoort : IEquatable<Adressoort>
    {
        public Adressoort(Guid id, string naam)
        {
            Id = id;
            Naam = naam;
        }

        [Key]
        public Guid Id { get; private set; }

        public string Naam { get; private set; }

        #region Equality

        public static bool operator !=(Adressoort one, Adressoort other)
        {
            return !(one == other);
        }

        public static bool operator ==(Adressoort left, Adressoort right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Adressoort);
        }

        public bool Equals(Adressoort other)
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
    }
}