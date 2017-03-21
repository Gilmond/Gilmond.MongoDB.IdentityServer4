using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Fixtures
{
	public class TestHostFixture<Startup> where Startup : class
	{
		private TestServer _server;
		public TestServer Server
		{
			get
			{
				if (_server == null)
					return _server = CreateServer();
				return _server;
			}
		}
		private TestServer CreateServer()
			=> new TestServer(new WebHostBuilder().UseStartup<Startup>());

		public HttpClient Client
			=> Server.CreateClient();
	}
	public class TestHostFixture<IdentityServerStartup, ApiServerStartup> 
		where IdentityServerStartup : class
		where ApiServerStartup : class
	{
		private TestServer _identityServer;
		public TestServer IdentityServer
		{
			get
			{
				if (_identityServer == null)
					return _identityServer = CreateIdentityServer();
				return _identityServer;
			}
		}
		private TestServer CreateIdentityServer()
			=> new TestServer(new WebHostBuilder().UseStartup<IdentityServerStartup>());

		public HttpClient CreateIdentityServerClient()
			=> IdentityServer.CreateClient();

		private TestServer _apiServer;

		public TestServer ApiServer
		{
			get
			{
				if (_apiServer == null)
					return _apiServer = CreateApiServer();
				return _apiServer;
			}
		}

		private TestServer CreateApiServer()
			=> new TestServer(new WebHostBuilder().UseStartup<ApiServerStartup>());

		public HttpClient CreateApiServerClient()
			=> _apiServer.CreateClient();
	}
}
