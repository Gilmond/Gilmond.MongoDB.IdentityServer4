using System.Threading.Tasks;
using FluentAssertions;
using Gilmond.MongoDB.IdentityServer4.IntegrationTests.Fixtures;
using Gilmond.MongoDB.IdentityServer4.IntegrationTests.Helpers;
using Gilmond.MongoDB.IdentityServer4.IntegrationTests.Startup;
using IdentityModel.Client;
using IdentityServer4.Models;
using Xunit;

using IdentityServer = Gilmond.MongoDB.IdentityServer4.IntegrationTests.Startup.ClientCredentialsFlowIdentityServer;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Scenarios
{
	public class ClientCredentialsFlowTests
	{
		public class GivenConfiguredTestHost : IClassFixture<TestHostFixture<IdentityServer, ClientCredentialsFlowApi>>
		{
			public class WhenRequestClientCredentialsAsync : GivenConfiguredTestHost
			{
				private readonly TestHostFixture<IdentityServer, SampleApi> _fixture;

				public WhenRequestClientCredentialsAsync(TestHostFixture<IdentityServer, SampleApi> fixture) 
					=> _fixture = fixture;

				[Fact]
				public async Task ThenTestHostIsDiscoverable()
				{
					var disco = await _fixture.Client.GetDiscoveryResponseAsync(_fixture.IdentityServer.BaseAddress).ConfigureAwait(false);
					disco.IsError.Should().BeFalse(because: $"discovery response should not contain the error: {disco.Error}");
				}

				[Fact]
				public async Task ThenTokenResponseReceived()
				{
					var disco = await _fixture.IdentityServerClient.GetDiscoveryResponseAsync(_fixture.IdentityServer.BaseAddress).ConfigureAwait(false);
					var tokenClient = new TestServerTokenClient(disco.TokenEndpoint, IdentityServer.ClientId, IdentityServer.ClientSecret, _fixture.IdentityServerClient);
					var tokenReponse = await tokenClient.RequestClientCredentialsAsync(IdentityServer.Resource).ConfigureAwait(false);
					tokenReponse.Should().NotBeNull(because: "host should return token response");
					tokenReponse.IsError.Should().BeFalse(because: $"token response should not contain the error: {tokenReponse.Error}");
				}

				[Fact]
				public async Task ThenApiCanBeCalled()
				{
					var disco = await _fixture.IdentityServerClient.GetDiscoveryResponseAsync(_fixture.IdentityServer.BaseAddress).ConfigureAwait(false);
					var tokenClient = new TestServerTokenClient(disco.TokenEndpoint, IdentityServer.ClientId, IdentityServer.ClientSecret, _fixture.IdentityServerClient);
					var tokenReponse = await tokenClient.RequestClientCredentialsAsync(IdentityServer.Resource).ConfigureAwait(false);
					var client = _fixture.ApiServerClient;
					client.SetBearerToken(tokenReponse.AccessToken);

					var apiResponse = await client.GetStringAsync("identity");
				}
			}
		}
	}
}
