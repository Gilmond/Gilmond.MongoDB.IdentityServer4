using System;
using System.Reflection;
using IdentityModel.Client;

namespace Gilmond.MongoDB.IdentityServer4.IntegrationTests.Helpers
{
	public static class ReflectionExtensions
	{
		private static readonly TypeInfo DiscoveryPolicyTypeInfo
			= typeof(DiscoveryPolicy).GetTypeInfo();
		private static readonly FieldInfo DiscoveryPolicyAuthorityFieldInfo
			= DiscoveryPolicyTypeInfo.GetField("Authority", BindingFlags.Instance | BindingFlags.NonPublic);

		public static void SetAuthority(this DiscoveryPolicy policy, Uri authority)
			=> policy.SetAuthority(authority.ToString());

		public static void SetAuthority(this DiscoveryPolicy policy, string authority)
			=> DiscoveryPolicyAuthorityFieldInfo.SetValue(policy, authority);
	}
}
