using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;
using AppServiceHelpers.Authentication;

namespace AppServiceHelpers
{
	public class Authenticator : IAuthenticator
	{
		private static readonly IAuthenticator instance = new Authenticator();
		internal static IAuthenticator Instance
		{
			get
			{
				return instance;
			}
		}

		public async Task<bool> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider)
		{
			var success = false;

			try
			{
				if (CurrentPlatform.Context == null)
				{
					throw new Exception("You must call AppServiceHelpers.CurrentPlatform.Init(Context context) to authenticate users.");
				}

				var dictionary = new Dictionary<string, string>();
				switch (provider)
				{
					// Does not support refresh token concept with server-flow authentication.
					case MobileServiceAuthenticationProvider.Facebook:
					case MobileServiceAuthenticationProvider.Twitter:
					// Supports refresh token concept, but all configuration is server-side.
					case MobileServiceAuthenticationProvider.MicrosoftAccount:
						break;
					case MobileServiceAuthenticationProvider.Google:
						dictionary.Add("access_type", "offline");
						break;
					case MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory:
						dictionary.Add("response_type", "code id_token");
						break;
				}

				var user = await client.LoginAsync(CurrentPlatform.Context, provider, dictionary);

				if (user != null)
				{
					var authenticationToken = client.CurrentUser.MobileServiceAuthenticationToken;
					var userId = client.CurrentUser.UserId;

					var keys = new Dictionary<string, string>
					{
						{ "userId", userId },
						{ "authenticationToken", authenticationToken }
					};

					AccountStore.Create(CurrentPlatform.Context).Save(new Account(userId, keys), provider.ToString());

					success = true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error logging in: {ex.Message}");
			}

			return success;
		}
	}
}