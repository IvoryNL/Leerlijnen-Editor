namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCare.Utilities;
    using Microsoft.FSharp.Core;

    public sealed class Medewerker : Persoon, IHaveAuditTrail<Medewerker>, IHaveBuilder<Medewerker, Medewerker.ActualBuilder>
    {
        public Medewerker(
            Guid id,
            Guid nummer,
            FSharpOption<DateTime> geboortedatum,
            FSharpOption<Geslacht> geslacht,
            FSharpOption<string> meisjesnaam,
            string achternaam,
            FSharpOption<string> tussenvoegsel,
            FSharpOption<string> initialen,
            FSharpOption<string> voornamen,
            FSharpOption<string> roepnaam,
            FSharpOption<Nationaliteit> nationaliteit1,
            FSharpOption<Nationaliteit> nationaliteit2,
            FSharpOption<string> burgelijkestaat,
            IEnumerable<Adres> adressen,
            IEnumerable<Telefoonnummer> telefoonnummers,
            IEnumerable<Emailadres> emailadressen,
            AuditTrail<Medewerker> auditTrail)
            : this(
                id,
                nummer,
                geboortedatum,
                geslacht,
                meisjesnaam,
                achternaam,
                tussenvoegsel,
                initialen,
                voornamen,
                roepnaam,
                nationaliteit1,
                nationaliteit2,
                burgelijkestaat,
                adressen,
                telefoonnummers,
                emailadressen,
                new Lazy<AuditTrail<Medewerker>>(() => auditTrail))
        {
        }

        private Medewerker(
            Guid id,
            Guid nummer,
            FSharpOption<DateTime> geboortedatum,
            FSharpOption<Geslacht> geslacht,
            FSharpOption<string> meisjesnaam,
            string achternaam,
            FSharpOption<string> tussenvoegsel,
            FSharpOption<string> initialen,
            FSharpOption<string> voornamen,
            FSharpOption<string> roepnaam,
            FSharpOption<Nationaliteit> nationaliteit1,
            FSharpOption<Nationaliteit> nationaliteit2,
            FSharpOption<string> burgelijkestaat,
            IEnumerable<Adres> adressen,
            IEnumerable<Telefoonnummer> telefoonnummers,
            IEnumerable<Emailadres> emailadressen,
            Lazy<AuditTrail<Medewerker>> auditTrail)
        {
            Id = id;
            Nummer = nummer;
            Geslacht = geslacht;
            Meisjesnaam = meisjesnaam;
            Achternaam = achternaam;
            Tussenvoegsel = tussenvoegsel;
            Initialen = initialen;
            Voornamen = voornamen;
            Roepnaam = roepnaam;
            Geboortedatum = geboortedatum;
            Nationaliteit1 = nationaliteit1;
            Nationaliteit2 = nationaliteit2;
            Burgelijkestaat = burgelijkestaat;
            Adressen = adressen;
            Telefoonnummers = telefoonnummers;
            Emailadressen = emailadressen;
            this.auditTrail = auditTrail;
        }

        private Lazy<AuditTrail<Medewerker>> auditTrail;

        public static Medewerker CreateWithDelayedAuditTrail(
            Guid id,
            Guid nummer,
            FSharpOption<DateTime> geboortedatum,
            FSharpOption<Geslacht> geslacht,
            FSharpOption<string> meisjesnaam,
            string achternaam,
            FSharpOption<string> tussenvoegsel,
            FSharpOption<string> initialen,
            FSharpOption<string> voornamen,
            FSharpOption<string> roepnaam,
            FSharpOption<Nationaliteit> nationaliteit1,
            FSharpOption<Nationaliteit> nationaliteit2,
            FSharpOption<string> burgelijkestaat,
            IEnumerable<Adres> adressen,
            IEnumerable<Telefoonnummer> telefoonnummers,
            IEnumerable<Emailadres> emailadressen,
            Func<Medewerker, AuditTrail<Medewerker>> auditTrailCreator)
        {
            Contract.Requires(auditTrailCreator != null);

            var medewerker = new Medewerker(
                id,
                nummer,
                geboortedatum,
                geslacht,
                meisjesnaam,
                achternaam,
                tussenvoegsel,
                initialen,
                voornamen,
                roepnaam,
                nationaliteit1,
                nationaliteit2,
                burgelijkestaat,
                adressen,
                telefoonnummers,
                emailadressen,
                (Lazy<AuditTrail<Medewerker>>)null);
            var auditTrail = new Lazy<AuditTrail<Medewerker>>(() => auditTrailCreator(medewerker));
            medewerker.auditTrail = auditTrail;
            return medewerker;
        }

        public static Medewerker IngelogdeGebruiker
        {
            get { return setuser ?? Defaultuser; }
            set { if (setuser == null) { setuser = value; } }
        }

        private static IEqualityComparer<Medewerker> surrogaatKeyComparer = new DataCare.Model.SurrogaatKeyComparer<Medewerker>();
        public static IEqualityComparer<Medewerker> SurrogaatKeyComparer { get { return surrogaatKeyComparer; } }

        public AuditTrail<Medewerker> AuditTrail
        {
            get { return this.auditTrail.Value; }
        }

        private static Medewerker setuser;

        private static readonly Medewerker Defaultuser =
            CreateWithDelayedAuditTrail(
                    Guid.Parse("5e5598af-aa57-4f70-abb0-05a0f199a797"),
                    Guid.Parse("4dda65fb-29d1-4fd8-9f1e-8b07c4a0be46"),
                    new FSharpOption<DateTime>(DateTimeExtensions.Now.Date),
                    null,
                    null,
                    "User",
                    null,
                    null,
                    null,
                    new FSharpOption<string>("User"),
                    null,
                    null,
                    null,
                    Enumerable.Empty<Adres>(),
                    Enumerable.Empty<Telefoonnummer>(),
                    Enumerable.Empty<Emailadres>(),
                    m => new AuditTrail<Medewerker>(m));

        public ActualBuilder ToBuilder()
        {
            return new HiddenBuilder(
                this,
                Geboortedatum,
                Geslacht,
                Meisjesnaam,
                Achternaam,
                Tussenvoegsel,
                Initialen,
                Voornamen,
                Roepnaam,
                Nationaliteit1,
                Nationaliteit2,
                Burgelijkestaat,
                Adressen,
                Telefoonnummers,
                Emailadressen);
        }

        private sealed class HiddenBuilder : ActualBuilder
        {
            private HiddenBuilder()
            {
            }

            public HiddenBuilder(
                Medewerker basis,
                FSharpOption<DateTime> geboortedatum,
                FSharpOption<Geslacht> geslacht,
                FSharpOption<string> meisjesnaam,
                string achternaam,
                FSharpOption<string> tussenvoegsel,
                FSharpOption<string> initialen,
                FSharpOption<string> voornamen,
                FSharpOption<string> roepnaam,
                FSharpOption<Nationaliteit> nationaliteit1,
                FSharpOption<Nationaliteit> nationaliteit2,
                FSharpOption<string> burgelijkeStaat,
                IEnumerable<Adres> adressen,
                IEnumerable<Telefoonnummer> telefoonnummers,
                IEnumerable<Emailadres> emailadressen)
                : base(
                basis,
                geboortedatum,
                geslacht,
                meisjesnaam,
                achternaam,
                tussenvoegsel,
                initialen,
                voornamen,
                roepnaam,
                nationaliteit1,
                nationaliteit2,
                burgelijkeStaat,
                adressen,
                telefoonnummers,
                emailadressen)
            {
            }

            protected override ActualBuilder CreateClone()
            {
                return new HiddenBuilder();
            }
        }

        public abstract class ActualBuilder : Builder<ActualBuilder>
        {
            protected ActualBuilder()
            {
            }

            protected ActualBuilder(
                Medewerker basis,
                FSharpOption<DateTime> geboortedatum,
                FSharpOption<Geslacht> geslacht,
                FSharpOption<string> meisjesnaam,
                string achternaam,
                FSharpOption<string> tussenvoegsel,
                FSharpOption<string> initialen,
                FSharpOption<string> voornamen,
                FSharpOption<string> roepnaam,
                FSharpOption<Nationaliteit> nationaliteit1,
                FSharpOption<Nationaliteit> nationaliteit2,
                FSharpOption<string> burgelijkeStaat,
                IEnumerable<Adres> adressen,
                IEnumerable<Telefoonnummer> telefoonnummers,
                IEnumerable<Emailadres> emailadressen)
                : base(
                basis,
                geboortedatum,
                geslacht,
                meisjesnaam,
                achternaam,
                tussenvoegsel,
                initialen,
                voornamen,
                roepnaam,
                nationaliteit1,
                nationaliteit2,
                burgelijkeStaat,
                adressen,
                telefoonnummers,
                emailadressen)
            {
            }
        }

        public abstract class Builder<TBuilder> : Persoon.Builder<Medewerker, TBuilder>
            where TBuilder : Builder<TBuilder>, IAmBuilder<TBuilder>
        {
            protected Builder()
            {
            }

            protected Builder(
                Medewerker basis,
                FSharpOption<DateTime> geboortedatum,
                FSharpOption<Geslacht> geslacht,
                FSharpOption<string> meisjesnaam,
                string achternaam,
                FSharpOption<string> tussenvoegsel,
                FSharpOption<string> initialen,
                FSharpOption<string> voornamen,
                FSharpOption<string> roepnaam,
                FSharpOption<Nationaliteit> nationaliteit1,
                FSharpOption<Nationaliteit> nationaliteit2,
                FSharpOption<string> burgerlijkeStaat,
                IEnumerable<Adres> adressen,
                IEnumerable<Telefoonnummer> telefoonnummers,
                IEnumerable<Emailadres> emailadressen) : base(
                    basis,
                    geboortedatum,
                    geslacht,
                    meisjesnaam,
                    achternaam,
                    tussenvoegsel,
                    initialen,
                    voornamen,
                    roepnaam,
                    nationaliteit1,
                    nationaliteit2,
                    burgerlijkeStaat,
                    adressen,
                    telefoonnummers,
                    emailadressen)
            {
            }

            public override Medewerker Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Medewerker(
                    Guid.NewGuid(),
                    Basis.Nummer,
                    GeboortedatumField,
                    GeslachtField,
                    MeisjesnaamField,
                    AchternaamField,
                    TussenvoegselField,
                    InitialenField,
                    VoornamenField,
                    RoepnaamField,
                    Nationaliteit1Field,
                    Nationaliteit2Field,
                    BurgelijkestaatField,
                    AdressenField,
                    TelefoonnummersField,
                    EmailadressenField,
                    new AuditTrail<Medewerker>(Basis));
            }
        }

        public class FullNameComparer : IEqualityComparer<Medewerker>
        {
            public bool Equals(Medewerker x, Medewerker y)
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
                    EqualityComparer<string>.Default.Equals(x.Naam, y.Naam);
            }

            public int GetHashCode(Medewerker obj)
            {
                if (ReferenceEquals(obj, null))
                {
                    return 0;
                }

                return obj.Naam.GetHashCode();
            }
        }
    }
}