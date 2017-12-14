using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Claims;
using System.Linq;
using System.Text;
using DataCare.IdentityModel.Claims;

namespace DataCare.Framework.Autorisation
{
    public static class LogosClaimsPrincipalExtensions
    {
        private static string GetClaimResourceAsString(ClaimsPrincipal principal, string claimtype)
        {
            if (principal == null)
            {
                return string.Empty;
            }

            var claim = principal.ClaimSets
                .FindClaims(claimtype, Rights.PossessProperty)
                .FirstOrDefault();

            if (claim != null)
            {
                return claim.Resource as string;
            }

            return string.Empty;
        }

        public static Guid GetInitialLocatieId(this ClaimsPrincipal principal)
        {
            var claim = string.Format(CultureInfo.CurrentCulture, "{0}{1}?{2}", Claimtypes.ResourceCodeNamespace2014, "AdministratieveLocatie", "HuidigeLocatie");
            var result = GetClaimResourceAsString(principal, claim);

            Guid nummer;
            if (Guid.TryParse(result, out nummer))
            {
                return nummer;
            }

            return Guid.Empty;
        }

        public static Guid GetLocatienummer(this ClaimsPrincipal principal)
        {
            var claim = string.Format(CultureInfo.CurrentCulture, "{0}{1}?{2}", Claimtypes.ResourceCodeNamespace2014, "AdministratieveLocatie", "LocatieNummer");
            var result = GetClaimResourceAsString(principal, claim);

            Guid nummer;
            if (Guid.TryParse(result, out nummer))
            {
                return nummer;
            }

            return Guid.Empty;
        }

        public static Guid GetKLantId(this ClaimsPrincipal principal)
        {
            var claim = string.Format(CultureInfo.CurrentCulture, "{0}{1}?{2}", Claimtypes.ResourceCodeNamespace2014, "Klant", "KlantId");
            var result = GetClaimResourceAsString(principal, claim);

            Guid id;
            if (Guid.TryParse(result, out id))
            {
                return id;
            }

            return Guid.Empty;
        }

        public static string GetGebruikersnaam(this ClaimsPrincipal principal)
        {
            var claim = Claimtypes.CreateResourceClaim(Claimtypes.ResourceCodeNamespace2014 + "toegangsbeheer/", string.Empty, "Medewerker", "HuidigeGebruikersnaam");
            var username = GetClaimResourceAsString(principal, claim);

            if (!string.IsNullOrWhiteSpace(username))
            {
                return username;
            }

            return string.Empty;
        }

        public static string GetMederwerkernaam(this ClaimsPrincipal principal)
        {
            var claim = Claimtypes.CreateResourceClaim(Claimtypes.ResourceCodeNamespace2014 + "toegangsbeheer/", string.Empty, "Medewerker", "HuidigeMedewerker");
            var username = GetClaimResourceAsString(principal, claim);

            if (!string.IsNullOrWhiteSpace(username))
            {
                return username;
            }

            return string.Empty;
        }

        public static Guid GetIngelogdeGebruikersNummer(this ClaimsPrincipal principal)
        {
            var claim = Claimtypes.CreateResourceClaim(Claimtypes.ResourceCodeNamespace2014 + "toegangsbeheer/", string.Empty, "Medewerker", "HuidigeMedewerkerId");
            var medewerkersnummer = GetClaimResourceAsString(principal, claim);

            Guid nummer;
            if (Guid.TryParse(medewerkersnummer, out nummer))
            {
                return nummer;
            }

            return Guid.Empty;
        }

        public static string GetIngelogdeGebruikerBeschrijving(this ClaimsPrincipal principal)
        {
            var medewerkerGebruikersNaam = GetGebruikersnaam(principal);
            var medewerkerNaam = GetMederwerkernaam(principal);
            var medewerkerNummer = GetIngelogdeGebruikersNummer(principal);

            var locatieId = GetInitialLocatieId(principal);

            return string.Format("{0} in {1} ({2} als {3})", medewerkerNaam, locatieId, medewerkerGebruikersNaam, medewerkerNummer);
        }
    }
}