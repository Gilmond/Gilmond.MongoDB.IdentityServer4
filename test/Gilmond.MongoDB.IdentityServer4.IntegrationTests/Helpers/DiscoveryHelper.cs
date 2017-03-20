using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Helpers
{
	public static class DiscoveryHelper
	{
		public static async Task<DiscoveryResponse> GetDiscoveryResponseAsync(this HttpClient client, Uri authority)
		{
			var discoveryAddress = $"{authority}{OidcConstants.Discovery.DiscoveryEndpoint}";
			var response = await client.GetAsync(discoveryAddress).ConfigureAwait(false);
			var policy = new DiscoveryPolicy();
			policy.SetAuthority(authority);
			return new DiscoveryResponse(await response.Content.ReadAsStringAsync().ConfigureAwait(false), policy);
		}
	}
}
