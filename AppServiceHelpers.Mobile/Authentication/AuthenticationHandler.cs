using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

namespace AppServiceHelpers
{
	public class AuthenticationHandler : DelegatingHandler
	{
		IMobileServiceClient client;

		public AuthenticationHandler(IMobileServiceClient client)
		{
			this.client = client;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			// Clone the request, in the event we need to re-issue it.
			var clonedRequest = await CloneRequestAsync(request);
			var response = await base.SendAsync(clonedRequest, cancellationToken);

			// Authentication token for the user needs to be refreshed.
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				try
				{
					// TODO: Delete cached token.


					// Attempt to use the /.auth/refresh endpoint to generate a new authentication token.
					await client.RefreshUserAsync();

					clonedRequest = await CloneRequestAsync(request);
					clonedRequest.Headers.Remove("X-ZUMO-AUTH");
					clonedRequest.Headers.Add("X-XUMO-AUTH", client.CurrentUser.MobileServiceAuthenticationToken);

					response = await base.SendAsync(clonedRequest, cancellationToken);
					if (response.StatusCode != HttpStatusCode.Unauthorized)
					{
						// TODO: Response worked - store the token.
					}
					else
					{
						// TODO: Prompt the user for web auth login.
					}
				}
				catch (MobileServiceInvalidOperationException ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
			}

			return response;
		}

		async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
		{
			var result = new HttpRequestMessage(request.Method, request.RequestUri);
			foreach (var header in request.Headers)
			{
				result.Headers.Add(header.Key, header.Value);
			}

			if (request.Content != null && request.Content.Headers.ContentType != null)
			{
				var requestBody = await request.Content.ReadAsStringAsync();
				var mediaType = request.Content.Headers.ContentType.MediaType;
				result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
				foreach (var header in request.Content.Headers)
				{
					if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
					{
						result.Content.Headers.Add(header.Key, header.Value);
					}
				}
			}

			return result;
		}
	}
}

/*

        public static async Task DoLoginAsync(Settings.AuthOption authOption)
        {
            if (authOption == Settings.AuthOption.GuestAccess) {
                Settings.Current.CurrentUserId = Settings.Current.DefaultUserId;
                return; // can't authenticate
            }

            var mobileClient = DependencyService.Get<IPlatform>();

            var user =
                authOption == Settings.AuthOption.Facebook ?
                    await LoginFacebookAsync(mobileClient) :
                    await mobileClient.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

            App.Instance.AuthenticatedUser = user;
            System.Diagnostics.Debug.WriteLine("Authenticated with user: " + user.UserId);

            Settings.Current.CurrentUserId =
                await App.Instance.MobileService.InvokeApiAsync<string>(
                "ManageUser",
                System.Net.Http.HttpMethod.Get,
                null);

            Debug.WriteLine($"Set current userID to: {Settings.Current.CurrentUserId}");

            AuthStore.CacheAuthToken(user);
        }

        private static Task<MobileServiceUser> LoginFacebookAsync(IPlatform mobileClient)
        {
            // use server flow if the service URL has been customized
            return Settings.IsDefaultServiceUrl() ?
                mobileClient.LoginFacebookAsync() :
                mobileClient.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
        }
    }
}*/

