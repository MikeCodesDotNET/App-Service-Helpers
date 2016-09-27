using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

using AppServiceHelpers.Authentication;

namespace AppServiceHelpers
{
    internal class Authenticator : IAuthenticator
    {
        private static readonly IAuthenticator instance = new Authenticator();
        internal static IAuthenticator Instance
        {
            get
            {
                return instance;
            }
        }

        public bool UserPreviouslyAuthenticated
        {
            get
            {
                if (AccountStore.GetAccount() == null)
                    return false;
                else
                    return true;
            }
        }

        public MobileServiceAuthenticationProvider FindIdentityProvider()
        {
            return AccountStore.GetIdentityProvider();
        }

        public Dictionary<string, string> LoadCachedUserCredentials()
        {
            return AccountStore.GetAccount();
        }

        public async Task<bool> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            var success = false;

            try
            {
				var dictionary = new Dictionary<string, string>();
				switch (provider)
				{
					// Does not support refresh token concept with server-flow authentication.
					case MobileServiceAuthenticationProvider.Facebook:
					case MobileServiceAuthenticationProvider.Twitter:
                    // Supports refresh token concept, but all configuration is server-side.
                    case MobileServiceAuthenticationProvider.MicrosoftAccount:
                    // Supports refresh token concept.
                    case MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory:
                        dictionary.Add("response_type", "code id_token");
                        break;
                    case MobileServiceAuthenticationProvider.Google:
						dictionary.Add("access_type", "offline");
						break;
				}

                var user = await client.LoginAsync(provider, dictionary);

                if (user != null)
                {
                    var authenticationToken = client.CurrentUser.MobileServiceAuthenticationToken;
                    var userId = client.CurrentUser.UserId;

                    var keys = new Dictionary<string, string>
                    {
                        { "userId", userId },
                        { "authenticationToken", authenticationToken },
						{ "identityProvider", provider.ToString() }
                    };

                    AccountStore.SaveAccount(keys);

                    success = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging in: {ex.Message}");
            }

            return success;
        }
    }
}
