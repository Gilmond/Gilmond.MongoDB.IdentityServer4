using System.Net.Http;
using System.Net.Http.Headers;
using IdentityModel.Client;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Helpers
{
	public class TestServerTokenClient : TokenClient
	{
		public TestServerTokenClient(string address)
			: base(address) { }

		public TestServerTokenClient(string address, HttpMessageHandler innerHttpMessageHandler)
			: base(address, innerHttpMessageHandler) { }

		public TestServerTokenClient(string address, string clientId, AuthenticationStyle style = AuthenticationStyle.PostValues)
			: base(address, clientId, style) { }

		public TestServerTokenClient(string address, string clientId, string clientSecret, AuthenticationStyle style = AuthenticationStyle.BasicAuthentication)
			: this(address, clientId, clientSecret, new HttpClient(new HttpClientHandler()), style) { }

		public TestServerTokenClient(string address, string clientId, string clientSecret, HttpClient client, AuthenticationStyle style = AuthenticationStyle.BasicAuthentication)
			: base(address, clientId, clientSecret, style)
		{
			Client = client;
			Client.DefaultRequestHeaders.Accept.Clear();
			Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public TestServerTokenClient(string address, string clientId, HttpMessageHandler innerHttpMessageHandler)
			: base(address, clientId, innerHttpMessageHandler) { }

		public TestServerTokenClient(string address, string clientId, string clientSecret, HttpMessageHandler innerHttpMessageHandler, AuthenticationStyle style = AuthenticationStyle.BasicAuthentication)
			: base(address, clientId, clientSecret, innerHttpMessageHandler, style) { }
	}
}
