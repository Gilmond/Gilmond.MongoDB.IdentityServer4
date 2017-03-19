using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Gilmond.MongoDB.IdentityServer4
{
	public interface ClientManager
	{
		Task AddClientAsync(Client client);
	}
}
