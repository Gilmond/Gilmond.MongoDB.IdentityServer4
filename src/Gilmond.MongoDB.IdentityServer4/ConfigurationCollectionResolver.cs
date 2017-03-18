using System;
using System.Threading.Tasks;
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
		private Lazy<Task<IMongoCollection<Client>>> _collection;

		public ConfigurationCollectionResolver(IOptionsMonitor<MongoDatabaseConfigurationStoreOptions> optionsMonitor)
		{
			_connection = optionsMonitor.CurrentValue.Connection;
			_client = new Lazy<IMongoClient>(GetClient);
			_database = new Lazy<IMongoDatabase>(GetDatabase);
			_collectionName = optionsMonitor.CurrentValue.Collections.Client;
			if (string.IsNullOrWhiteSpace(_collectionName))
				throw new MongoDatabaseConfigurationException("No Client Collection Name has been configured.");
			_collection = new Lazy<Task<IMongoCollection<Client>>>(GetCollectionAsync);
			// TODO: Implement config change handling
			//optionsMonitor.OnChange(Update);
		}

		public Task<IMongoCollection<Client>> GetClientCollectionAsync() 
			=> _collection.Value;

		private IMongoClient GetClient()
		{
			var builder = new MongoUrlBuilder
			{
				Server = new MongoServerAddress(_connection.Server.Host, _connection.Server.Port),
				DatabaseName = _connection.DatabaseName,
				Username = _connection.Username,
				Password = _connection.Password
			};
			return new MongoClient(builder.ToMongoUrl());
		}

		private IMongoDatabase GetDatabase() 
			=> _client.Value.GetDatabase(_connection.DatabaseName);

		private const string UniqueClientIdIndexName = "unique-client-id";
		private async Task<IMongoCollection<Client>> GetCollectionAsync()
		{
			var collection = _database.Value.GetCollection<Client>(_collectionName);
			using (var cursor = await collection.Indexes.ListAsync())
				while (cursor.MoveNext())
					foreach (var index in cursor.Current)
						if (index.GetElement("name").Value.AsString == UniqueClientIdIndexName)
							return collection;
			var builder = new IndexKeysDefinitionBuilder<Client>();
			await collection.Indexes.CreateOneAsync(builder.Hashed(x => x.ClientId), new CreateIndexOptions { Unique = true });
			return collection;
		}
	}
}
