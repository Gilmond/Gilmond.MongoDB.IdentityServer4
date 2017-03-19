namespace Gilmond.MongoDB.IdentityServer4
{
	public class MongoDatabaseConnectionOptions
	{
		public MongoDatabaseServerOptions Server { get; set; } = new MongoDatabaseServerOptions();
		public string AuthenticationDatabaseName { get; set; }
		public string DatabaseName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
