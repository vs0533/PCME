using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api
{
    public class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
        public static IEnumerable<ApiResource> GetApiReSource()
        {
            return new List<ApiResource> {
                new ApiResource("api1","CMEAPI")
            };

        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client{
                    ClientId = "extjs",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("secret".Sha256()) },
                    AllowedScopes = { "api1"}
                }
            };
        }
    }
}
