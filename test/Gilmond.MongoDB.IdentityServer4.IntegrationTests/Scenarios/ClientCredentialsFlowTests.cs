using System.Threading.Tasks;
using FluentAssertions;
using Gilmond.MongoDB.IdentityServer4.IntegrationTests.Fixtures;
using Gilmond.MongoDB.IdentityServer4.IntegrationTests.Helpers;
using Gilmond.MongoDB.IdentityServer4.IntegrationTests.Startup;
using IdentityModel.Client;
using Xunit;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Scenarios
{
	public class ClientCredentialsFlowTests
	{
		public class GivenConfiguredTestHost : IClassFixture<TestHostFixture<ClientCredentialsFlow>>
		{
			private readonly TestHostFixture<ClientCredentialsFlow> _fixture;

			private GivenConfiguredTestHost(TestHostFixture<ClientCredentialsFlow> fixture)
			{
				_fixture = fixture;
			}

			public class WhenRequestClientCredentialsAsync : GivenConfiguredTestHost
			{
				public WhenRequestClientCredentialsAsync(TestHostFixture<ClientCredentialsFlow> fixture)
					: base(fixture) { }

				[Fact]
				public async Task ThenTestHostIsDiscoverable()
				{
					var disco = await _fixture.Client.GetDiscoveryResponseAsync(_fixture.Server.BaseAddress).ConfigureAwait(false);
					disco.IsError.Should().BeFalse(because: $"discovery response should not contain the error: {disco.Error}");
				}

				[Fact]
				public async Task ThenTokenResponseReceived()
				{
					var disco = await _fixture.Client.GetDiscoveryResponseAsync(_fixture.Server.BaseAddress).ConfigureAwait(false);
					var client = new TokenClient(disco.TokenEndpoint, ClientCredentialsFlow.ClientId, ClientCredentialsFlow.ClientSecret);
					var tokenReponse = await client.RequestClientCredentialsAsync(ClientCredentialsFlow.Resource).ConfigureAwait(false);
					tokenReponse.Should().NotBeNull(because: "host should return token response");
					tokenReponse.IsError.Should().BeFalse(because: $"token response should not contain the error: {tokenReponse.Error}");
				}
			}
		}
	}
}
