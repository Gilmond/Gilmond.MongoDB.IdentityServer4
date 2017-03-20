using System;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ConfigurationCollectionResolver : CollectionResolver
	{
		private MongoDatabaseConnectionOptions _connection;
		private Lazy<IMongoClient> _client;
		private Lazy<IMongoDatabase> _database;
		private string _clientCollectionName;
		private string _identityResourceCollectionName;
		private string _apiResourceCollectionName;
		private Lazy<IMongoCollection<Client>> _clientCollection;
		private Lazy<IMongoCollection<IdentityResource>> _identityResourceCollection;
		private Lazy<IMongoCollection<ApiResource>> _apiResourceCollection;

		public ConfigurationCollectionResolver(IOptionsMonitor<MongoDatabaseConfigurationStoreOptions> optionsMonitor)
		{
			_connection = optionsMonitor.CurrentValue.Connection;
			_client = new Lazy<IMongoClient>(GetClient);
			_database = new Lazy<IMongoDatabase>(GetDatabase);

			_clientCollectionName = optionsMonitor.CurrentValue.Collections.Client;
			_identityResourceCollectionName = optionsMonitor.CurrentValue.Collections.IdentityResource;
			_apiResourceCollectionName = optionsMonitor.CurrentValue.Collections.ApiResource;

			if (string.IsNullOrWhiteSpace(_clientCollectionName))
				throw new MongoDatabaseConfigurationException("No Client Collection Name has been configured.");
			if (string.IsNullOrWhiteSpace(_identityResourceCollectionName))
				throw new MongoDatabaseConfigurationException("No Identity Resource Collection Name has been configured.");
			if (string.IsNullOrWhiteSpace(_apiResourceCollectionName))
				throw new MongoDatabaseConfigurationException("No Api Resource Collection Name has been configured.");

			_clientCollection = new Lazy<IMongoCollection<Client>>(GetClients);
			_identityResourceCollection = new Lazy<IMongoCollection<IdentityResource>>(GetIdentityResources);
			_apiResourceCollection = new Lazy<IMongoCollection<ApiResource>>(GetApiResources);

			// TODO: Implement config change handling
			//optionsMonitor.OnChange(Update);
		}

		public IMongoCollection<Client> GetClientCollection() 
			=> _clientCollection.Value;

		private IMongoCollection<Client> GetClients()
			=> _database.Value.GetCollection<Client>(_clientCollectionName);

		public IMongoCollection<IdentityResource> GetIdentityResourceCollection()
			=> _identityResourceCollection.Value;

		private IMongoCollection<IdentityResource> GetIdentityResources()
			=> _database.Value.GetCollection<IdentityResource>(_identityResourceCollectionName);

		private IMongoCollection<ApiResource> GetApiResources()
			=> _database.Value.GetCollection<ApiResource>(_apiResourceCollectionName);

		public IMongoCollection<ApiResource> GetApiResourceCollection()
			=> _apiResourceCollection.Value;

		private IMongoClient GetClient()
		{
			var settings = new MongoClientSettings
			{
				Server = new MongoServerAddress(_connection.Server.Host, _connection.Server.Port),
				Credentials = new [] { MongoCredential.CreateCredential(_connection.AuthenticationDatabaseName, _connection.Username, _connection.Password) }
			};
			return new MongoClient(settings);
		}

		private IMongoDatabase GetDatabase() 
			=> _client.Value.GetDatabase(_connection.DatabaseName);
	}
}
