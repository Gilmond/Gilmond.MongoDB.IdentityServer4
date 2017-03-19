using Gilmond.MongoDB.IdentityServer4;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceExtensions
	{
		public static IIdentityServerBuilder AddConfigurationStore(this IIdentityServerBuilder builder, IConfiguration configuration)
		{
			builder.Services.AddOptions();
			builder.Services.AddTransient<IClientStore, ClientStore>();
			builder.Services.AddScoped<CollectionResolver, ConfigurationCollectionResolver>();
			builder.Services.AddScoped<ClientManager, ClientsCollectionManager>();

			builder.Services.Configure<MongoDatabaseConfigurationStoreOptions>(options =>
			{
				options.Collections.Client = "clients";
			});
			builder.Services.Configure<MongoDatabaseConfigurationStoreOptions>(configuration);

			Mapper.ConfigureModels();

			return builder;
		}
	}
}
