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
		private readonly Lazy<IMongoCollection<Client>> _collection;

		public ClientStore(CollectionResolver collectionResolver)
		{
			_collection = new Lazy<IMongoCollection<Client>>(collectionResolver.GetClientCollection);
		}

		public async Task<Client> FindClientByIdAsync(string clientId)
		{
			var clients = new List<Client>();
			using (var cursor = await _collection.Value.FindAsync(Builders<Client>.Filter.Eq(x => x.ClientId, clientId)).ConfigureAwait(false))
				while (await cursor.MoveNextAsync().ConfigureAwait(false))
					clients.AddRange(cursor.Current);
			return clients.SingleOrDefault();
		}
	}
}
