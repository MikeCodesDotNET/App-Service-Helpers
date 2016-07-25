using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;

namespace AppServiceHelpers.Platform.Android
{
	public class Authenticator : IAuthenticator
	{
		public async Task<bool> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider)
		{
			var success = false;

			try
			{
				if (CurrentPlatform.Context == null)
				{
					throw new Exception("You must call AppServiceHelpers.Platform.Android.CurrentPlatform.Init(Context context) to authenticate users.");
				}

				var user = await client.LoginAsync(CurrentPlatform.Context, provider);

				if (user != null)
				{
					var authenticationToken = client.CurrentUser.MobileServiceAuthenticationToken;
					var userId = client.CurrentUser.UserId;

					var keys = new Dictionary<string, string>
					{
						{ "userId", userId },
						{ "authenticationToken", authenticationToken }
					};

					await AccountStore.Create().SaveAsync(new Account(userId, keys), provider.ToString());

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