using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ResourcesCollectionManager : ResourceManager
	{
		private readonly Lazy<IMongoCollection<ApiResource>> _apiResources;
		private readonly Lazy<IMongoCollection<IdentityResource>> _identityResources;

		public ResourcesCollectionManager(CollectionResolver collectionResolver)
		{
			_apiResources = new Lazy<IMongoCollection<ApiResource>>(collectionResolver.GetApiResourceCollection);
			_identityResources = new Lazy<IMongoCollection<IdentityResource>>(collectionResolver.GetIdentityResourceCollection);
			// TODO: Implement config change handling
			//optionsMonitor.OnChange(Update);
		}

		public Task AddResourceAsync(ApiResource apiResource)
			=> _apiResources.Value.InsertOneAsync(apiResource);

		public Task AddResourceAsync(IdentityResource identityResource)
			=> _identityResources.Value.InsertOneAsync(identityResource);
	}
}
