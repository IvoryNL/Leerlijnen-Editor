namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataCare.Utilities;

    public class EmailadresSoort : IEquatable<EmailadresSoort>
    {
        public EmailadresSoort(Guid id, string naam)
        {
            Id = id;
            Naam = naam;
        }

        [Key]
        public Guid Id { get; private set; }

        public string Naam { get; private set; }

        #region Equality

        public static bool operator !=(EmailadresSoort one, EmailadresSoort other)
        {
            return !(one == other);
        }

        public static bool operator ==(EmailadresSoort left, EmailadresSoort right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EmailadresSoort);
        }

        public bool Equals(EmailadresSoort other)
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