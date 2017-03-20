using IdentityServer4.Models;
using MongoDB.Bson.Serialization;

namespace Gilmond.MongoDB.IdentityServer4
{
	// TODO: Make extensible
	internal static class Mapper
	{
		public static void ConfigureModels()
		{
			BsonClassMap.RegisterClassMap<Client>(ConfigureClientModel);
			BsonClassMap.RegisterClassMap<ApiResource>(ConfigureApiResourceModel);
			BsonClassMap.RegisterClassMap<IdentityResource>(ConfigureIdentityResourceModel);
		}

		private static void ConfigureClientModel(BsonClassMap<Client> map)
		{
			map.AutoMap();
			map.MapIdProperty(x => x.ClientId);
		}

		private static void ConfigureApiResourceModel(BsonClassMap<ApiResource> map)
		{
			map.AutoMap();
			map.MapIdProperty(x => x.Name);
		}

		private static void ConfigureIdentityResourceModel(BsonClassMap<IdentityResource> map)
		{
			map.AutoMap();
			map.MapIdProperty(x => x.Name);
		}
	}
}
