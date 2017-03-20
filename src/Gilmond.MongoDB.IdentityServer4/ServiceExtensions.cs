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
			builder.Services.AddScoped<CollectionResolver, ConfigurationCollectionResolver>();
			builder.Services.AddTransient<IClientStore, ClientStore>();
			builder.Services.AddScoped<ClientManager, ClientsCollectionManager>();
			builder.Services.AddTransient<IResourceStore, ResourceStore>();
			builder.Services.AddScoped<ResourceManager, ResourcesCollectionManager>();

			builder.Services.Configure<MongoDatabaseConfigurationStoreOptions>(options =>
			{
				options.Collections.Client = "clients";
				options.Collections.IdentityResource = "identities";
				options.Collections.ApiResource = "apis";
			});
			builder.Services.Configure<MongoDatabaseConfigurationStoreOptions>(configuration);

			Mapper.ConfigureModels();

			return builder;
		}

		public static IIdentityServerBuilder AddOperationalStore(this IIdentityServerBuilder builder, IConfiguration configuration)
		{
			return builder;
		}
	}
}
