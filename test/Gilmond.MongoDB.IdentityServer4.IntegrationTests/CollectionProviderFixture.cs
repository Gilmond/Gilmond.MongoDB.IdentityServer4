using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests
{
	public class CollectionProviderFixture : IDisposable
	{
		private readonly List<Client> _clients = new List<Client>();
		private readonly Lazy<IServiceProvider> _provider 
			= new Lazy<IServiceProvider>(BuildServiceProvider);

		public void AddClient(Client client) => _clients.Add(client);

		private static IServiceProvider BuildServiceProvider()
		{
			var configuration = new ConfigurationBuilder()
				.AddUserSecrets<CollectionProviderFixture>()
				.Build();
				
			var services = new ServiceCollection();
			services.AddIdentityServer().AddConfigurationStore(configuration);
			return services.BuildServiceProvider();
		}

		public IClientStore GetClientStore()
			=> _provider.Value.GetRequiredService<IClientStore>();

		public void Dispose()
		{
			if (_provider.IsValueCreated && _provider.Value is IDisposable disposable)
				disposable.Dispose();
		}
	}
}
