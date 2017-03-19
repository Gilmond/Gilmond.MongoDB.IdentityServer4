using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ClientsCollectionManager : ClientManager
	{
		private readonly Lazy<Task<IMongoCollection<Client>>> _collection;

		public ClientsCollectionManager(CollectionResolver collectionResolver)
		{
			_collection = new Lazy<Task<IMongoCollection<Client>>>(collectionResolver.GetClientCollectionAsync);
		}

		public async Task AddClientAsync(Client client)
		{
			var collection = await _collection.Value;
			await collection.InsertOneAsync(client);
		}
	}
}
