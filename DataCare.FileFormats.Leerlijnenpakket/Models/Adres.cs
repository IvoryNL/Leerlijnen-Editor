namespace DataCare.Model.Basisadministratie
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using DataCare.Utilities;
    using Microsoft.FSharp.Core;

    public static class AdresHelper
    {
        public static string GetStraatnaamHuisnummertoevoeging(string straatnaam, int? huisnummer, string huisnummertoevoeging, string huisnummeraanduiding)
        {
            bool someValue = false;
            bool huisnummerValue = false;
            var straatnaamHuisnummer = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(straatnaam))
            {
                straatnaamHuisnummer.Append(straatnaam.Trim());
                someValue = true;
            }

            if (!string.IsNullOrWhiteSpace(huisnummeraanduiding))
            {
                if (someValue)
                {
                    straatnaamHuisnummer.Append(" ");
                }

                straatnaamHuisnummer.Append("(" + huisnummeraanduiding + ")");
                someValue = true;
            }

            if (huisnummer.HasValue)
            {
                if (someValue)
                {
                    straatnaamHuisnummer.Append(" ");
                }

                straatnaamHuisnummer.Append(huisnummer);
                someValue = true;
                huisnummerValue = true;
            }

            if (!string.IsNullOrWhiteSpace(huisnummertoevoeging))
            {
                if (huisnummerValue)
                {
                    // Als er een huisnummer en een huisnummertoevoeging is dan zet een - anders een spatie
                    straatnaamHuisnummer.Append("-");
                }
                else if (someValue)
                {
                    straatnaamHuisnummer.Append(" ");
                }

                straatnaamHuisnummer.Append(huisnummertoevoeging);
                someValue = true;
            }

            return straatnaamHuisnummer.ToString();
        }
    }

    public sealed class Adres : IEquatable<Adres>, IHaveAuditTrail<Adres>, IHaveBuilder<Adres, Adres.Builder>
    {
        private static Func<Adres, Adres, bool> keyEquals = Equality.KeyEquals<Adres>().Compile();

        public Adres(
            Guid id,
            Adressoort adressoort,
            bool isGeheim,
            FSharpOption<string> straatnaam,
            FSharpOption<int> huisnummer,
            FSharpOption<string> huisnummertoevoeging,
            FSharpOption<string> huisnummeraanduiding,
            FSharpOption<string> postcode,
            FSharpOption<string> plaatsnaam,
            FSharpOption<string> locatieomschrijving,
            FSharpOption<string> adresregelBuitenland1,
            FSharpOption<string> adresregelBuitenland2,
            FSharpOption<string> adresregelBuitenland3,
            Land land,
            AuditTrail<Adres> auditTrail)
        {
            Id = id;
            Adressoort = adressoort;
            IsGeheim = isGeheim;
            Straatnaam = straatnaam;
            Huisnummer = huisnummer;
            Huisnummertoevoeging = huisnummertoevoeging;
            Huisnummeraanduiding = huisnummeraanduiding;
            Postcode = postcode;
            Plaatsnaam = plaatsnaam;
            Locatieomschrijving = locatieomschrijving;

            AdresregelBuitenland1 = adresregelBuitenland1;
            AdresregelBuitenland2 = adresregelBuitenland2;
            AdresregelBuitenland3 = adresregelBuitenland3;
            Land = land;
            AuditTrail = auditTrail;
        }

        public FSharpOption<string> AdresregelBuitenland1 { get; private set; }
        public FSharpOption<string> AdresregelBuitenland2 { get; private set; }
        public FSharpOption<string> AdresregelBuitenland3 { get; private set; }
        public Adressoort Adressoort { get; private set; }
        public AuditTrail<Adres> AuditTrail { get; private set; }
        public FSharpOption<int> Huisnummer { get; private set; }
        public FSharpOption<string> Huisnummeraanduiding { get; private set; }
        public FSharpOption<string> Huisnummertoevoeging { get; private set; }

        [Key]
        public Guid Id { get; private set; }

        public bool IsGeheim { get; private set; }
        public Land Land { get; private set; }

        public FSharpOption<string> Locatieomschrijving { get; private set; }
        public FSharpOption<string> Plaatsnaam { get; private set; }
        public FSharpOption<string> Postcode { get; private set; }

        public FSharpOption<string> Straatnaam { get; private set; }

        public static bool operator !=(Adres one, Adres other)
        {
            return !(one == other);
        }

        public static bool operator ==(Adres left, Adres right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Adres);
        }

        public bool Equals(Adres other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static Builder Create()
        {
            return new ActualBuilder(
                null,
                false,
                default(Adressoort),
                default(Land),
                FSharpOption<string>.None,
                FSharpOption<string>.None,
                FSharpOption<int>.None,
                FSharpOption<string>.None,
                FSharpOption<string>.None,
                FSharpOption<string>.None,
                FSharpOption<string>.None,
                FSharpOption<string>.None,
                FSharpOption<string>.None,
                FSharpOption<string>.None);
        }

        public Builder ToBuilder()
        {
            return new ActualBuilder(
                this,
                IsGeheim,
                Adressoort,
                Land,
                Postcode,
                Straatnaam,
                Huisnummer,
                Huisnummertoevoeging,
                Huisnummeraanduiding,
                Plaatsnaam,
                Locatieomschrijving,
                AdresregelBuitenland1,
                AdresregelBuitenland2,
                AdresregelBuitenland3);
        }

        public class AdresComparer : IEqualityComparer<Adres>
        {
            public bool Equals(Adres x, Adres y)
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
                    x.Land == y.Land &&
                    x.Plaatsnaam == y.Plaatsnaam &&
                    x.Straatnaam == y.Straatnaam &&
                    x.Huisnummer == y.Huisnummer &&
                    x.Postcode == y.Postcode;
            }

            public int GetHashCode(Adres obj)
            {
                return typeof(Adres).GetHashCode();
            }
        }

        public class ValueComparer : IEqualityComparer<Adres>
        {
            public bool Equals(Adres x, Adres y)
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
                    x.AdresregelBuitenland1 == y.AdresregelBuitenland1 &&
                    x.AdresregelBuitenland2 == y.AdresregelBuitenland2 &&
                    x.AdresregelBuitenland3 == y.AdresregelBuitenland3 &&
                    x.Adressoort == y.Adressoort &&
                    x.Huisnummer == y.Huisnummer &&
                    x.Huisnummeraanduiding == y.Huisnummeraanduiding &&
                    x.Huisnummertoevoeging == y.Huisnummertoevoeging &&
                    x.IsGeheim == y.IsGeheim &&
                    x.Land == y.Land &&
                    x.Locatieomschrijving == y.Locatieomschrijving &&
                    x.Plaatsnaam == y.Plaatsnaam &&
                    x.Postcode == y.Postcode &&
                    x.Straatnaam == y.Straatnaam;
            }

            public int GetHashCode(Adres obj)
            {
                return typeof(Adres).GetHashCode();
            }
        }

        public abstract class Builder : IAmBuilderFor<Adres>
        {
            private readonly FSharpOption<string> adresregelBuitenland1;
            private readonly FSharpOption<string> adresregelBuitenland2;
            private readonly FSharpOption<string> adresregelBuitenland3;
            private readonly Adressoort adressoort;
            private readonly Adres basis;
            private readonly FSharpOption<int> huisnummer;
            private readonly FSharpOption<string> huisnummeraanduiding;
            private readonly FSharpOption<string> huisnummertoevoeging;
            private readonly bool isGeheim;
            private readonly Land land;
            private readonly FSharpOption<string> locatieomschrijving;
            private readonly FSharpOption<string> plaatsnaam;
            private readonly FSharpOption<string> postcode;
            private readonly FSharpOption<string> straatnaam;

            protected Builder(
                Adres basis,
                bool isGeheim,
                Adressoort adressoort,
                Land land,
                FSharpOption<string> postcode,
                FSharpOption<string> straatnaam,
                FSharpOption<int> huisnummer,
                FSharpOption<string> huisnummertoevoeging,
                FSharpOption<string> huisnummeraanduiding,
                FSharpOption<string> plaatsnaam,
                FSharpOption<string> locatieomschrijving,
                FSharpOption<string> adresregelBuitenland1,
                FSharpOption<string> adresregelBuitenland2,
                FSharpOption<string> adresregelBuitenland3)
            {
                this.basis = basis;
                this.isGeheim = isGeheim;
                this.adressoort = adressoort;
                this.land = land;
                this.postcode = postcode;
                this.straatnaam = straatnaam;
                this.huisnummer = huisnummer;
                this.huisnummertoevoeging = huisnummertoevoeging;
                this.huisnummeraanduiding = huisnummeraanduiding;
                this.plaatsnaam = plaatsnaam;
                this.locatieomschrijving = locatieomschrijving;
                this.adresregelBuitenland1 = adresregelBuitenland1;
                this.adresregelBuitenland2 = adresregelBuitenland2;
                this.adresregelBuitenland3 = adresregelBuitenland3;
            }

            public Builder Adressoort(Adressoort newValue)
            {
                Contract.Requires(newValue != null);

                return new ActualBuilder(basis, isGeheim, newValue, land, postcode, straatnaam, huisnummer, huisnummertoevoeging, huisnummeraanduiding, plaatsnaam, locatieomschrijving, adresregelBuitenland1, adresregelBuitenland2, adresregelBuitenland3);
            }

            public Builder BinnenlandsAdres(
                FSharpOption<string> newStraatnaam,
                FSharpOption<int> newHuisnummer,
                FSharpOption<string> newHuisnummertoevoeging,
                FSharpOption<string> newHuisnummeraanduiding,
                FSharpOption<string> newPostcode,
                FSharpOption<string> newPlaatsnaam,
                FSharpOption<string> newLocatieomschrijving)
            {
                return new ActualBuilder(basis, isGeheim, adressoort, land, newPostcode, newStraatnaam, newHuisnummer, newHuisnummertoevoeging, newHuisnummeraanduiding, newPlaatsnaam, newLocatieomschrijving, adresregelBuitenland1, adresregelBuitenland2, adresregelBuitenland3);
            }

            public Adres Build()
            {
                if (!IsValid())
                {
                    throw new InvalidOperationException();
                }

                return new Adres(Guid.NewGuid(), adressoort, isGeheim, straatnaam, huisnummer, huisnummertoevoeging, huisnummeraanduiding, postcode, plaatsnaam, locatieomschrijving, adresregelBuitenland1, adresregelBuitenland2, adresregelBuitenland3, land, new AuditTrail<Adres>(basis));
            }

            public Builder BuitenlandsAdres(FSharpOption<string> newAdresregel1, FSharpOption<string> newAdresregel2, FSharpOption<string> newAdresregel3, Land newLand)
            {
                return new ActualBuilder(basis, isGeheim, adressoort, newLand, postcode, straatnaam, huisnummer, huisnummertoevoeging, huisnummeraanduiding, plaatsnaam, locatieomschrijving, newAdresregel1, newAdresregel2, newAdresregel3);
            }

            public Builder IsGeheim(bool newValue)
            {
                return new ActualBuilder(basis, newValue, adressoort, land, postcode, straatnaam, huisnummer, huisnummertoevoeging, huisnummeraanduiding, plaatsnaam, locatieomschrijving, adresregelBuitenland1, adresregelBuitenland2, adresregelBuitenland3);
            }

            public bool IsValid()
            {
                return true;
            }
        }

        private class ActualBuilder : Builder
        {
            internal ActualBuilder(
                Adres basis,
                bool isGeheim,
                Adressoort adressoort,
                Land land,
                FSharpOption<string> postcode,
                FSharpOption<string> straatnaam,
                FSharpOption<int> huisnummer,
                FSharpOption<string> huisnummerToevoeging,
                FSharpOption<string> huisnummerAanduiding,
                FSharpOption<string> plaatsnaam,
                FSharpOption<string> locatieOmschrijving,
                FSharpOption<string> adresregelBuitenland1,
                FSharpOption<string> adresregelBuitenland2,
                FSharpOption<string> adresregelBuitenland3)
                : base(basis, isGeheim, adressoort, land, postcode, straatnaam, huisnummer, huisnummerToevoeging, huisnummerAanduiding, plaatsnaam, locatieOmschrijving, adresregelBuitenland1, adresregelBuitenland2, adresregelBuitenland3)
            {
            }
        }
    }
}