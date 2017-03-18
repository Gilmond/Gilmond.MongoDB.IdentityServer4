using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ClientStore : IClientStore
	{
		private readonly Lazy<Task<IMongoCollection<Client>>> _collection;

		public ClientStore(CollectionResolver collectionResolver)
		{
			_collection = new Lazy<Task<IMongoCollection<Client>>>(collectionResolver.GetClientCollectionAsync);
		}

		public async Task<Client> FindClientByIdAsync(string clientId)
		{
			var collection = await _collection.Value;
			var clients = new List<Client>();
			using (var cursor = await collection.FindAsync(Builders<Client>.Filter.Eq(x => x.ClientId, clientId)))
				while (await cursor.MoveNextAsync())
					clients.AddRange(cursor.Current);
			return clients.SingleOrDefault();
		}
	}
}
