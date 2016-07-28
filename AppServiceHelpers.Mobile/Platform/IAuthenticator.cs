using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

namespace AppServiceHelpers
{
	public interface IAuthenticator
	{
		Task<bool> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider);
	}
}