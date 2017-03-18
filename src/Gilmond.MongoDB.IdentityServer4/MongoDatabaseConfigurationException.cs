using System;

namespace Gilmond.MongoDB.IdentityServer4
{
	public class MongoDatabaseConfigurationException : Exception
	{
		internal MongoDatabaseConfigurationException(string message)
			: base(message) { }
	}
}
