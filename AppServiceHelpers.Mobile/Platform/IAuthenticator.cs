using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

namespace AppServiceHelpers
{
	public interface IAuthenticator
	{
		Task<bool> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider);
		MobileServiceAuthenticationProvider FindIdentityProvider();
		Dictionary<string, string> LoadCachedUserCredentials();
		bool UserPreviouslyAuthenticated { get; }
	}
}