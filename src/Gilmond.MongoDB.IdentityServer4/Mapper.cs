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
		}

		private static void ConfigureClientModel(BsonClassMap<Client> map)
		{
			map.AutoMap();
			map.MapIdProperty(x => x.ClientId);
		}
	}
}
