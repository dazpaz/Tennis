using Duende.IdentityServer.Models;
using System.Collections.Generic;

namespace IdentityServer
{
	public static class Config
	{
		public static IEnumerable<ApiScope> ApiScopes =>
			new List<ApiScope>
			{
				new ApiScope("tournamentManagement", "Tournament Management API")
			};

		public static IEnumerable<Client> Clients =>
			new List<Client>
			{
				new Client
				{
					ClientId = "client",

					// no interactive user, use the clientid/secret for authentication
					AllowedGrantTypes = GrantTypes.ClientCredentials,

					// secret for authentication
					ClientSecrets =
					{
						new Secret("client-secret".Sha256())
					},

					// scopes that client has access to
					AllowedScopes = { "tournamentManagement" }
				}
			};
	}
}