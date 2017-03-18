namespace Gilmond.MongoDB.IdentityServer4
{
	public class MongoDatabaseConfigurationStoreOptions
	{
		public MongoDatabaseCollectionOptions Collections { get; set; } = new MongoDatabaseCollectionOptions();
		public MongoDatabaseConnectionOptions Connection { get; set; } = new MongoDatabaseConnectionOptions();
	}
}
