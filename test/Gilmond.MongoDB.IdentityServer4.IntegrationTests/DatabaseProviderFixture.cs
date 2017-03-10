using System;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests
{
	public class DatabaseProviderFixture : IDisposable
	{
		// TODO: Find a way to intialize MongoDB
		// Given we don't have the luxury of EF migrations, initialization, nor seeds, 
		// contemplating using some docker integration, such as https://github.com/mariotoffia/FluentDocker
		public void Dispose()
		{
		}
	}
}
