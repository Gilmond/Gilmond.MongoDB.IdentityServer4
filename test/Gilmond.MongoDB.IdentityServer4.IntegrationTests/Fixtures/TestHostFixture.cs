using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Fixtures
{
	public class TestHostFixture<Startup> where Startup : class
	{
		private TestServer CreateServer()
			=> new TestServer(new WebHostBuilder().UseStartup<Startup>());


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

		public HttpClient Client
			=> Server.CreateClient();
	}
}
