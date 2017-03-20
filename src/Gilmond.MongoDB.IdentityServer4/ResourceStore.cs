using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using MongoDB.Driver;

namespace Gilmond.MongoDB.IdentityServer4
{
	internal sealed class ResourceStore : IResourceStore
	{
		private readonly CollectionResolver _collections;

		public ResourceStore(CollectionResolver collections)
		{
			_collections = collections;
		}

		public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
		{
			throw new System.NotImplementedException();
		}

		public Task<ApiResource> FindApiResourceAsync(string name)
		{
			throw new System.NotImplementedException();
		}

		public async Task<Resources> GetAllResources()
		{
			var identities = new List<IdentityResource>();
			using (var cursor = await _collections.GetIdentityResourceCollection().FindAsync(FilterDefinition<IdentityResource>.Empty).ConfigureAwait(false))
				while (await cursor.MoveNextAsync().ConfigureAwait(false))
					identities.AddRange(cursor.Current);
			var apis = new List<ApiResource>();
			using (var cursor = await _collections.GetApiResourceCollection().FindAsync(FilterDefinition<ApiResource>.Empty).ConfigureAwait(false))
				while (await cursor.MoveNextAsync().ConfigureAwait(false))
					apis.AddRange(cursor.Current);
			return new Resources(identities, apis);
		}
	}
}
