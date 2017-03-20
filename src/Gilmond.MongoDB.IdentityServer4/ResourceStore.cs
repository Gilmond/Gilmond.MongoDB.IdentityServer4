using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ResourceStore : IResourceStore
	{
		private readonly Lazy<IMongoCollection<ApiResource>> _apiResources;
		private readonly Lazy<IMongoCollection<IdentityResource>> _identityResources;

		public ResourceStore(CollectionResolver collectionResolver)
		{
			_apiResources = new Lazy<IMongoCollection<ApiResource>>(collectionResolver.GetApiResourceCollection);
			_identityResources = new Lazy<IMongoCollection<IdentityResource>>(collectionResolver.GetIdentityResourceCollection);
		}

		public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
		{
			var identities = new List<IdentityResource>();
			using (var cursor = await _identityResources.Value.FindAsync(Builders<IdentityResource>.Filter.In(x => x.Name, scopeNames)).ConfigureAwait(false))
				while (await cursor.MoveNextAsync().ConfigureAwait(false))
					identities.AddRange(cursor.Current);
			return identities;
		}

		public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
		{
			var apis = new List<ApiResource>();
			using (var cursor = await _apiResources.Value.FindAsync(Builders<ApiResource>.Filter.In(x => x.Name, scopeNames)).ConfigureAwait(false))
				while (await cursor.MoveNextAsync().ConfigureAwait(false))
					apis.AddRange(cursor.Current);
			return apis;
		}

		public Task<ApiResource> FindApiResourceAsync(string name)
		{
			throw new System.NotImplementedException();
		}

		public async Task<Resources> GetAllResources()
		{
			var identities = new List<IdentityResource>();
			using (var cursor = await _identityResources.Value.FindAsync(FilterDefinition<IdentityResource>.Empty).ConfigureAwait(false))
				while (await cursor.MoveNextAsync().ConfigureAwait(false))
					identities.AddRange(cursor.Current);
			var apis = new List<ApiResource>();
			using (var cursor = await _apiResources.Value.FindAsync(FilterDefinition<ApiResource>.Empty).ConfigureAwait(false))
				while (await cursor.MoveNextAsync().ConfigureAwait(false))
					apis.AddRange(cursor.Current);
			return new Resources(identities, apis);
		}
	}
}
