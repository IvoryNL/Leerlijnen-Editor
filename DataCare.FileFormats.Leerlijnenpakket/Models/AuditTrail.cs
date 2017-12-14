namespace DataCare.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using DataCare.Model.Basisadministratie;
    using DataCare.Utilities;
    using FSharpx;
    using Microsoft.FSharp.Core;

    public sealed class AuditTrail<T> : IEquatable<AuditTrail<T>> where T : IHaveAuditTrail<T>
    {
        public AuditTrail(
            Medewerker createdBy,
            DateTime createdOn,
            FSharpOption<T> previousVersion)
        {
            if (createdBy == null)
            {
                System.Diagnostics.Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} WARNING: Request for AuditTrail with createdBy null for {1}, using logged in user", DateTimeExtensions.Now, typeof(T)));
                createdBy = Medewerker.IngelogdeGebruiker;
            }

            CreatedBy = createdBy;
            CreatedOn = createdOn;
            this.previousVersion = new WeakReference(previousVersion);
            this.previousVersionResolver = () => previousVersion;
            this.semaphore = Tuple.Create(previousVersion);
            this.identifier = previousVersion == null ? string.Empty : PreviousVersion.GetHashCode().ToString(CultureInfo.InvariantCulture);
        }

        public AuditTrail(
            Medewerker createdBy,
            DateTime createdOn,
            Func<FSharpOption<T>> previousVersionResolver,
            string identifier)
        {
            if (createdBy == null)
            {
                System.Diagnostics.Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} WARNING: Request for resolving AuditTrail with createdBy null for {1}, using logged in user", DateTimeExtensions.Now, typeof(T)));
                createdBy = Medewerker.IngelogdeGebruiker;
            }

            CreatedBy = createdBy;
            CreatedOn = createdOn;
            this.previousVersion = null;
            this.previousVersionResolver = previousVersionResolver;
            this.semaphore = new Object();
            this.identifier = identifier;
        }

        public AuditTrail(T previousVersion)
            : this(Medewerker.IngelogdeGebruiker, DateTimeExtensions.Now, previousVersion.ToFSharpOption())
        {
        }

        public AuditTrail()
            : this(Medewerker.IngelogdeGebruiker, DateTimeExtensions.Now, FSharpOption<T>.None)
        {
        }

        internal AuditTrail(Medewerker m)
            : this(m, DateTimeExtensions.Now, FSharpOption<T>.None)
        {
        }

        [Key]
        public Medewerker CreatedBy { get; private set; }

        [Key]
        public DateTime CreatedOn { get; private set; }

        [Key]
        public FSharpOption<T> PreviousVersion
        {
            get
            {
                FSharpOption<T> previous;

                lock (semaphore)
                {
                    if (previousVersion == null)
                    {
                        previous = previousVersionResolver();
                        previousVersion = new WeakReference(previous);
                    }
                    else
                    {
                        previous = previousVersion.Target as FSharpOption<T>;
                        if (previous == null)
                        {
                            previous = previousVersionResolver();
                            previousVersion = new WeakReference(previous);
                        }
                    }
                }

                return previous;
            }
        }

        public bool IsPreviousVersionCreated
        {
            get
            {
                if (previousVersion == null)
                {
                    return false;
                }

                return previousVersion.IsAlive;
            }
        }

        public string RefString
        {
            get
            {
                return identifier;
            }
        }

        private WeakReference previousVersion;
        private readonly string identifier;
        private Func<FSharpOption<T>> previousVersionResolver;
        private object semaphore;

        #region Equality

        public static bool operator !=(AuditTrail<T> one, AuditTrail<T> other)
        {
            return !(one == other);
        }

        public static bool operator ==(AuditTrail<T> left, AuditTrail<T> right)
        {
            return ReferenceEquals(left, right) || (!ReferenceEquals(left, null) && !ReferenceEquals(right, null) &&
                left.CreatedBy.Equals(right.CreatedBy) && left.CreatedOn.Equals(right.CreatedOn) && left.identifier.Equals(right.identifier));
        }

        public override bool Equals(object obj)
        {
            return Equality.Of(this, obj);
        }

        public bool Equals(AuditTrail<T> other)
        {
            return this == other;
        }

        private int? hashcode;

        public override int GetHashCode()
        {
            this.hashcode = this.hashcode ?? new object[] { CreatedBy, CreatedOn, this.identifier }.CombineHashCodes();
            return (int)this.hashcode;
        }

        #endregion Equality

        /// <summary>
        /// Checks if the AuditTrail contains a particular object
        /// WARNING: Potentially very expensive, as the audittrail can be arbitrarily long and not known locally
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool Contains(T element)
        {
            return GetFullTrail().Any(el => el.Equals(element));
        }

        /// <summary>
        /// Returns all objects in the audit trail
        /// WARNING: Potentially very expensive, as the audit trail can be arbitrarily long and not known locally
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetFullTrail()
        {
            var current = this;

            while (current.PreviousVersion.HasValue()
                && !ReferenceEquals(current.PreviousVersion.Value, null)
                && !current.Equals(current.PreviousVersion.Value))
            {
                yield return current.PreviousVersion.Value;
                current = current.PreviousVersion.Value.AuditTrail;
            }
        }
    }
}