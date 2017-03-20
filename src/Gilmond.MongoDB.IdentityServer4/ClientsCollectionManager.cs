using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ClientsCollectionManager : ClientManager
	{
		private readonly Lazy<IMongoCollection<Client>> _clients;

		public ClientsCollectionManager(CollectionResolver collectionResolver)
		{
			_clients = new Lazy<IMongoCollection<Client>>(collectionResolver.GetClientCollection);
			// TODO: Implement config change handling
			//optionsMonitor.OnChange(Update);
		}

		public Task AddClientAsync(Client client) 
			=> _clients.Value.InsertOneAsync(client);
	}
}
