using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AppServiceHelpers;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;

using UIKit;

namespace AppServiceHelpers.Platform.iOS
{
	public class Authenticator : IAuthenticator
	{
		public async Task<bool> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider)
		{
			var success = false;

			try
			{
				var user = await client.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, provider);

				if (user != null)
				{
					var authenticationToken = client.CurrentUser.MobileServiceAuthenticationToken;
					var userId = client.CurrentUser.UserId;

					var keys = new Dictionary<string, string>
					{
						{ "userId", authenticationToken },
						{ "authenticationToken", userId }
					};

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