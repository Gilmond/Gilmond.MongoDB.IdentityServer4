using System;
using System.Collections.Generic;
using Gilmond.MongoDB.IdentityServer4.IntegrationTests.Fixtures;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Startup
{
	public class ClientCredentialsFlow
	{
		public static string ClientId { get; } = Guid.NewGuid().ToString("N");
		public static string ClientSecret { get; } = Guid.NewGuid().ToString("N");
		public static string Resource { get; } = Guid.NewGuid().ToString("N");

		private readonly string _clientCollectionName = Guid.NewGuid().ToString("N");

		public void ConfigureServices(IServiceCollection services)
		{
			var configuration = new ConfigurationBuilder()
				.AddUserSecrets<CollectionProviderFixture>()
				.AddInMemoryCollection(GetOverridenConfiguration())
				.Build();

			services.AddIdentityServer()
				.AddTemporarySigningCredential()
				.AddConfigurationStore(configuration)
				.AddOperationalStore(configuration);
		}

		private IEnumerable<KeyValuePair<string, string>> GetOverridenConfiguration()
		{
			yield return new KeyValuePair<string, string>("Collections:Client", _clientCollectionName);
		}

		public void Configure(IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
				SeedData(scope.ServiceProvider);

			app.UseIdentityServer();
		}

		private void SeedData(IServiceProvider provider)
		{
			var clients = provider.GetRequiredService<ClientManager>();
			clients.AddClientAsync(new Client
			{
				ClientId = ClientId,
				AllowedGrantTypes = GrantTypes.ClientCredentials,
				ClientSecrets = { new Secret(ClientSecret.Sha512()) },
				AllowedScopes = { Resource }
			}).Wait();
		}
	}
}
