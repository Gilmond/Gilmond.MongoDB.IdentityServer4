using IdentityServer4.Models;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	public interface CollectionResolver
	{
		IMongoCollection<Client> GetClientCollection();
		IMongoCollection<IdentityResource> GetIdentityResourceCollection();
		IMongoCollection<ApiResource> GetApiResourceCollection();
	}
}
