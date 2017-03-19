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
		private string _collectionName;
		private Lazy<IMongoCollection<Client>> _collection;

		public ConfigurationCollectionResolver(IOptionsMonitor<MongoDatabaseConfigurationStoreOptions> optionsMonitor)
		{
			_connection = optionsMonitor.CurrentValue.Connection;
			_client = new Lazy<IMongoClient>(GetClient);
			_database = new Lazy<IMongoDatabase>(GetDatabase);
			_collectionName = optionsMonitor.CurrentValue.Collections.Client;
			if (string.IsNullOrWhiteSpace(_collectionName))
				throw new MongoDatabaseConfigurationException("No Client Collection Name has been configured.");
			_collection = new Lazy<IMongoCollection<Client>>(GetCollection);
			// TODO: Implement config change handling
			//optionsMonitor.OnChange(Update);
		}

		public IMongoCollection<Client> GetClientCollection() 
			=> _collection.Value;

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
		
		private IMongoCollection<Client> GetCollection() 
			=> _database.Value.GetCollection<Client>(_collectionName);
	}
}
