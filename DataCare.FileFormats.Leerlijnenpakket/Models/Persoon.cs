namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using DataCare.Utilities;
    using FSharpx;
    using Microsoft.FSharp.Core;
    using NodaTime;

    public abstract class Persoon : IEquatable<Persoon>, ISurrogateKey
    {
        [Key]
        public Guid Id { get; protected set; }

        public Guid Nummer { get; protected set; }

        public FSharpOption<DateTime> Geboortedatum { get; protected set; }

        public FSharpOption<Geslacht> Geslacht { get; protected set; }

        public FSharpOption<string> Meisjesnaam { get; protected set; }

        public string Achternaam { get; protected set; }

        public FSharpOption<string> Tussenvoegsel { get; protected set; }

        public FSharpOption<string> Initialen { get; protected set; }

        public FSharpOption<string> Voornamen { get; protected set; }

        public FSharpOption<string> Roepnaam { get; protected set; }

        public FSharpOption<Nationaliteit> Nationaliteit1 { get; protected set; }
        public FSharpOption<Nationaliteit> Nationaliteit2 { get; protected set; }

        public FSharpOption<string> Burgelijkestaat { get; protected set; }

        public IEnumerable<Adres> Adressen { get; protected set; }

        public IEnumerable<Telefoonnummer> Telefoonnummers { get; protected set; }

        public IEnumerable<Emailadres> Emailadressen { get; protected set; }

        #region Equality

        public static bool operator !=(Persoon one, Persoon other)
        {
            return !(one == other);
        }

        public static bool operator ==(Persoon left, Persoon right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Persoon);
        }

        public bool Equals(Persoon other)
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

        public int CompareTo(Persoon value)
        {
            return new PersoonComparer().Compare(this, value);
        }

        public class PersoonComparer : IComparer<Persoon>
        {
            public int Compare(Persoon x, Persoon y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (ReferenceEquals(x, null))
                {
                    return -1;
                }

                if (ReferenceEquals(y, null))
                {
                    return 1;
                }

                int result = x.Achternaam.CompareTo(y.Achternaam);
                if (result != 0)
                {
                    return result;
                }

                result = Comparer<FSharpOption<string>>.Default.Compare(x.Tussenvoegsel, y.Tussenvoegsel);
                if (result != 0)
                {
                    return result;
                }

                result = Comparer<FSharpOption<string>>.Default.Compare(x.Voornamen, y.Voornamen);

                return result;
            }
        }

        public class ValueComparer : IEqualityComparer<Persoon>
        {
            public bool Equals(Persoon x, Persoon y)
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
                    x.Achternaam == y.Achternaam &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Tussenvoegsel, y.Tussenvoegsel) &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Roepnaam, y.Roepnaam) &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Voornamen, y.Voornamen) &&
                    EqualityComparer<FSharpOption<Nationaliteit>>.Default.Equals(x.Nationaliteit1, y.Nationaliteit1) &&
                    EqualityComparer<FSharpOption<Nationaliteit>>.Default.Equals(x.Nationaliteit2, y.Nationaliteit2) &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Meisjesnaam, y.Meisjesnaam) &&
                    x.Leeftijd == y.Leeftijd &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Initialen, y.Initialen) &&
                    EqualityComparer<FSharpOption<Geslacht>>.Default.Equals(x.Geslacht, y.Geslacht) &&
                    EqualityComparer<FSharpOption<DateTime>>.Default.Equals(x.Geboortedatum, y.Geboortedatum) &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Burgelijkestaat, y.Burgelijkestaat) &&
                    x.Adressen.SetEquals(y.Adressen, new Adres.ValueComparer());
            }

            public int GetHashCode(Persoon obj)
            {
                return typeof(Persoon).GetHashCode();
            }
        }

        public class NaamGeboortedatumGeslachtComparer : IEqualityComparer<Persoon>
        {
            public bool Equals(Persoon x, Persoon y)
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
                    x.Achternaam == y.Achternaam &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Tussenvoegsel, y.Tussenvoegsel) &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Roepnaam, y.Roepnaam) &&
                    EqualityComparer<FSharpOption<string>>.Default.Equals(x.Voornamen, y.Voornamen) &&
                    EqualityComparer<FSharpOption<Geslacht>>.Default.Equals(x.Geslacht, y.Geslacht) &&
                    EqualityComparer<FSharpOption<DateTime>>.Default.Equals(x.Geboortedatum, y.Geboortedatum);
            }

            public int GetHashCode(Persoon obj)
            {
                return typeof(Persoon).GetHashCode();
            }
        }

        #endregion Equality

        public string Naam
        {
            get
            {
                if (Tussenvoegsel.HasValue() && !string.IsNullOrWhiteSpace(Tussenvoegsel.Value))
                {
                    return string.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", Roepnaam.HasValue() ? Roepnaam.Value : string.Empty, Tussenvoegsel.Value, Achternaam).Trim();
                }

                return string.Format(CultureInfo.CurrentCulture, "{0} {1}", Roepnaam.HasValue() ? Roepnaam.Value : string.Empty, Achternaam).Trim();
            }
        }

        public int Leeftijd
        {
            // TODO: deze berekening is fout
            get { return DateTime.Today.Year - (!Geboortedatum.HasValue() ? DateTime.Today : Geboortedatum.Value).Year; }
        }

        public int LeeftijdInJaren
        {
            get { return CalculateLeeftijd().Item1; }
        }

        public int LeeftijdInMaanden
        {
            get { return CalculateLeeftijd().Item2; }
        }

        private Tuple<int, int> CalculateLeeftijd()
        {
            if (Geboortedatum.HasSomeValue())
            {
                var birthday = new LocalDate(Geboortedatum.Value.Year, Geboortedatum.Value.Month, Geboortedatum.Value.Day);
                var today = new LocalDate(DateTimeExtensions.Now.Year, DateTimeExtensions.Now.Month, DateTimeExtensions.Now.Day);

                Period period = Period.Between(birthday, today, PeriodUnits.Years | PeriodUnits.Months);
                return Tuple.Create<int, int>((int)period.Years, (int)period.Months);
            }

            return Tuple.Create<int, int>(0, 0);
        }

        public abstract class Builder<TBuilt, TBuilder> : IAmBuilderFor<TBuilt>, IAmBuilder<TBuilder>
            where TBuilt : Persoon
            where TBuilder : Builder<TBuilt, TBuilder>, IAmBuilder<TBuilder>
        {
            protected Builder()
            {
            }

            protected Builder(
                TBuilt basis,
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
            {
                this.Basis = basis;
                this.GeboortedatumField = geboortedatum;
                this.GeslachtField = geslacht;
                this.MeisjesnaamField = meisjesnaam;
                this.AchternaamField = achternaam;
                this.TussenvoegselField = tussenvoegsel;
                this.InitialenField = initialen;
                this.VoornamenField = voornamen;
                this.RoepnaamField = roepnaam;
                this.Nationaliteit1Field = nationaliteit1;
                this.Nationaliteit2Field = nationaliteit2;
                this.BurgelijkestaatField = burgelijkeStaat;
                this.AdressenField = adressen;
                this.TelefoonnummersField = telefoonnummers;
                this.EmailadressenField = emailadressen;
            }

            protected TBuilt Basis { get; set; }
            protected FSharpOption<DateTime> GeboortedatumField { get; set; }
            protected FSharpOption<Geslacht> GeslachtField { get; set; }
            protected FSharpOption<string> MeisjesnaamField { get; set; }
            protected string AchternaamField { get; set; }
            protected FSharpOption<string> TussenvoegselField { get; set; }
            protected FSharpOption<string> InitialenField { get; set; }
            protected FSharpOption<string> VoornamenField { get; set; }
            protected FSharpOption<string> RoepnaamField { get; set; }
            protected FSharpOption<Nationaliteit> Nationaliteit1Field { get; set; }
            protected FSharpOption<Nationaliteit> Nationaliteit2Field { get; set; }
            protected FSharpOption<string> BurgelijkestaatField { get; set; }
            protected IEnumerable<Adres> AdressenField { get; set; }
            protected IEnumerable<Telefoonnummer> TelefoonnummersField { get; set; }
            protected IEnumerable<Emailadres> EmailadressenField { get; set; }

            public TBuilder Geboortedatum(FSharpOption<DateTime> nieuweGeboorteDatum)
            {
                var result = Clone();
                result.GeboortedatumField = nieuweGeboorteDatum;
                return result;
            }

            public TBuilder Geslacht(FSharpOption<Geslacht> nieuwGeslacht)
            {
                var result = Clone();
                result.GeslachtField = nieuwGeslacht;
                return result;
            }

            public TBuilder Burgelijkestaat(FSharpOption<string> nieuweBurgelijkestaat)
            {
                var result = Clone();
                result.BurgelijkestaatField = nieuweBurgelijkestaat;
                return result;
            }

            public TBuilder Nationaliteit1(FSharpOption<Nationaliteit> nationaliteit)
            {
                var result = Clone();
                result.Nationaliteit1Field = nationaliteit;
                return result;
            }

            public TBuilder Nationaliteit2(FSharpOption<Nationaliteit> nationaliteit)
            {
                var result = Clone();
                result.Nationaliteit2Field = nationaliteit;
                return result;
            }

            public TBuilder Naam(
                 string achternaam,
                 FSharpOption<string> tussenvoegsel,
                 FSharpOption<string> initialen,
                 FSharpOption<string> voornamen,
                 FSharpOption<string> roepnaam,
                 FSharpOption<string> meisjesnaam)
            {
                var result = Clone();
                result.MeisjesnaamField = meisjesnaam;
                result.AchternaamField = achternaam;
                result.TussenvoegselField = tussenvoegsel;
                result.InitialenField = initialen;
                result.VoornamenField = voornamen;
                result.RoepnaamField = roepnaam;
                return result;
            }

            public TBuilder Adressen(IEnumerable<Adres> nieuweAdressen)
            {
                var result = Clone();
                result.AdressenField = nieuweAdressen;
                return result;
            }

            public TBuilder Telefoonnummers(IEnumerable<Telefoonnummer> nieuweTelefoonnummers)
            {
                var result = Clone();
                result.TelefoonnummersField = nieuweTelefoonnummers;
                return result;
            }

            public TBuilder Emailadressen(IEnumerable<Emailadres> nieuweEmailadressen)
            {
                var result = Clone();
                result.EmailadressenField = nieuweEmailadressen;
                return result;
            }

            public TBuilder Add(Adres adres)
            {
                var newValues = this.AdressenField
                        .Concat(new[] { adres })
                        .Memoize();

                return this.Adressen(newValues);
            }

            public TBuilder Add(Emailadres email)
            {
                var newValues = this.EmailadressenField
                        .Concat(new[] { email })
                        .Memoize();

                return this.Emailadressen(newValues);
            }

            public TBuilder Add(Telefoonnummer nummer)
            {
                var newValues = this.TelefoonnummersField
                        .Concat(new[] { nummer })
                        .Memoize();

                return this.Telefoonnummers(newValues);
            }

            public TBuilder Replace(Telefoonnummer oldNummer, Telefoonnummer newNummer)
            {
                var newValues = this.TelefoonnummersField
                    .Where(t => t != oldNummer)
                    .Concat(new[] { newNummer })
                    .Memoize();

                return Telefoonnummers(newValues);
            }

            public TBuilder Replace(Adres oldAdres, Adres newAdres)
            {
                var newValues = this.AdressenField
                    .Where(a => a != oldAdres)
                    .Concat(new[] { newAdres })
                    .Memoize();

                return Adressen(newValues);
            }

            public TBuilder Replace(Emailadres oldEmail, Emailadres newEmail)
            {
                var newValues = this.EmailadressenField
                    .Where(t => t != oldEmail)
                    .Concat(new[] { newEmail })
                    .Memoize();

                return Emailadressen(newValues);
            }

            public TBuilder Remove(Telefoonnummer oldNummer)
            {
                var newValues = this.TelefoonnummersField
                    .Where(t => t != oldNummer)
                    .Memoize();

                return Telefoonnummers(newValues);
            }

            public TBuilder Remove(Adres oldAdres)
            {
                var newValues = this.AdressenField
                    .Where(t => t != oldAdres)
                    .Memoize();

                return Adressen(newValues);
            }

            public TBuilder Remove(Emailadres oldEmail)
            {
                var newValues = this.EmailadressenField
                    .Where(t => t != oldEmail)
                    .Memoize();

                return Emailadressen(newValues);
            }

            public virtual bool IsValid()
            {
                return !string.IsNullOrWhiteSpace(AchternaamField);
            }

            protected abstract TBuilder CreateClone();

            public virtual TBuilder Clone()
            {
                var actual = CreateClone();

                actual.Basis = Basis;
                actual.GeboortedatumField = GeboortedatumField;
                actual.GeslachtField = GeslachtField;
                actual.MeisjesnaamField = MeisjesnaamField;
                actual.AchternaamField = AchternaamField;
                actual.TussenvoegselField = TussenvoegselField;
                actual.InitialenField = InitialenField;
                actual.VoornamenField = VoornamenField;
                actual.RoepnaamField = RoepnaamField;
                actual.Nationaliteit1Field = Nationaliteit1Field;
                actual.Nationaliteit2Field = Nationaliteit2Field;
                actual.BurgelijkestaatField = BurgelijkestaatField;
                actual.AdressenField = AdressenField;
                actual.TelefoonnummersField = TelefoonnummersField;
                actual.EmailadressenField = EmailadressenField;

                return actual;
            }

            public abstract TBuilt Build();
        }
    }
}