using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Xunit;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Stores
{
	public class ClientStoreTests
	{
		public class GivenClientExists : IClassFixture<CollectionProviderFixture>
		{
			private Client Client { get; }

			private GivenClientExists(CollectionProviderFixture fixture)
			{
				Client = new Client
				{
					ClientId = "test_client",
					ClientName = "Test Client"
				};
				fixture.AddClient(Client);
			}

			public class WhenFindClientByIdAsync : GivenClientExists
			{
				private readonly IClientStore _clientStore;

				public WhenFindClientByIdAsync(CollectionProviderFixture fixture) : base(fixture)
				{
					_clientStore = fixture.GetClientStore();
				}

				[Fact]
				public async Task ThenClientIsReturned()
				{
					var client = await _clientStore.FindClientByIdAsync(Client.ClientId);
					client.Should().NotBeNull(because: "client exists");
				}


				[Fact]
				public async Task ThenClientIdIsPopulated()
				{
					var client = await _clientStore.FindClientByIdAsync(Client.ClientId);
					client.ClientId.Should().Be(Client.ClientId, because: "client name was set");
				}

				[Fact]
				public async Task ThenClientNameIsPopulated()
				{
					var client = await _clientStore.FindClientByIdAsync(Client.ClientId);
					client.ClientName.Should().Be(Client.ClientName, because: "client name was set");
				}
			}
		}
	}
}
