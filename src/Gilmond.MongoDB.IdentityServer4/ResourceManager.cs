using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Gilmond.MongoDB.IdentityServer4
{
	public interface ResourceManager
	{
		Task AddResourceAsync(ApiResource apiResource);
		Task AddResourceAsync(IdentityResource identityResource);
	}
}
