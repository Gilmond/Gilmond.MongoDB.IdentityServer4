using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests
{
	public class CollectionProviderFixture : IDisposable
	{
		private readonly string _clientCollectionName = Guid.NewGuid().ToString("N");
		private readonly List<Client> _clients = new List<Client>();
		private readonly Lazy<IServiceProvider> _provider;

		private IClientStore _clientStore;

		public CollectionProviderFixture()
		{
			_provider = new Lazy<IServiceProvider>(BuildServiceProvider);
		}

		public void AddClient(Client client) => _clients.Add(client);

		private IServiceProvider BuildServiceProvider()
		{
			var configuration = new ConfigurationBuilder()
				.AddUserSecrets<CollectionProviderFixture>()
				.AddInMemoryCollection(GetOverridenConfiguration())
				.Build();
				
			var services = new ServiceCollection();
			services.AddIdentityServer().AddConfigurationStore(configuration);
			return services.BuildServiceProvider();
		}

		private IEnumerable<KeyValuePair<string, string>> GetOverridenConfiguration()
		{
			yield return new KeyValuePair<string, string>("Collections:Client", _clientCollectionName);
		}

		public IClientStore GetClientStore()
		{
			if (_clientStore != null)
				return _clientStore;
			if (_clients.Any())
				AddClients();
			return _clientStore = _provider.Value.GetRequiredService<IClientStore>();
		}

		private void AddClients()
		{
			var manager = _provider.Value.GetRequiredService<ClientManager>();
			foreach (var client in _clients)
				manager.AddClientAsync(client).Wait();
		}

		public void Dispose()
		{
			if (!_provider.IsValueCreated) return;

			var config = _provider.Value.GetRequiredService<IOptions<MongoDatabaseConfigurationStoreOptions>>().Value;
			var client = new MongoClient(new MongoClientSettings
			{
				Server = new MongoServerAddress(config.Connection.Server.Host, config.Connection.Server.Port),
				Credentials = new[]
				{
					MongoCredential.CreateCredential(
						config.Connection.AuthenticationDatabaseName, 
						config.Connection.Username,
						config.Connection.Password)
				}
			});
			var database = client.GetDatabase(config.Connection.DatabaseName);
			database.DropCollection(config.Collections.Client);

			if (_provider.Value is IDisposable disposable)
				disposable.Dispose();
		}
	}
}
