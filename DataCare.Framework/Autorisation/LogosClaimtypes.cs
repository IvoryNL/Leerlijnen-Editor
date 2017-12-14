namespace DataCare.Framework.Autorisation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using DataCare.IdentityModel.Claims;
    using Framework;

    public static class LogosClaimtypes
    {
        public static string CreateResourceClaim(string module, string claim)
        {
            return Claimtypes.CreateResourceClaim(Claimtypes.ResourceNamespace, module, claim);
        }

        public static string CreateResourceClaimForType<T>(string module)
        {
            return Claimtypes.CreateResourceClaim<T>(Claimtypes.ResourceNamespace, module, string.Empty);
        }

        public static string CreateResourceClaimForId<T>(string module, Guid value)
        {
            return CreateResourceClaimForType<T>(module, string.Format("id={0}", value));
        }

        public static UriTemplate CreateUriTemplateForId()
        {
            return new UriTemplate("?id={id}");
        }

        public static string CreateResourceClaimForType<T>(string module, Dictionary<string, string> properties)
        {
            return Claimtypes.CreateResourceClaim<T>(Claimtypes.ResourceNamespace, module, Claimtypes.CreateUriPropertiesSegment(properties));
        }

        public static string CreateResourceClaimForType<T>(string module, string specificatie)
        {
            return Claimtypes.CreateResourceClaim<T>(Claimtypes.ResourceNamespace, module, specificatie);
        }

        public static string CreateResourceCodeClaimForType<T>(string module, string specificatie)
        {
            return Claimtypes.CreateResourceClaim<T>(Claimtypes.ResourceCodeNamespace, module, specificatie);
        }

        public static string CreateResourceCodeClaimForResource(string module, string specificatie)
        {
            return Claimtypes.CreateResourceClaim(Claimtypes.ResourceCodeNamespace, module, specificatie);
        }
    }
}