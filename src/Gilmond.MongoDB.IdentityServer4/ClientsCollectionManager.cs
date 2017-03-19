using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ClientsCollectionManager : ClientManager
	{
		private readonly Lazy<IMongoCollection<Client>> _collection;

		public ClientsCollectionManager(CollectionResolver collectionResolver)
		{
			_collection = new Lazy<IMongoCollection<Client>>(collectionResolver.GetClientCollection);
		}

		public Task AddClientAsync(Client client) 
			=> _collection.Value.InsertOneAsync(client);
	}
}
