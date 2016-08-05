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
		public IMobileServiceClient Client { get; set; }

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
					// Attempt to use the /.auth/refresh endpoint to generate a new authentication token.
					await Client.RefreshUserAsync();

					// Re-clone the original request, update the headers, and re-issue the cloned request.
					clonedRequest = await CloneRequestAsync(request);
					clonedRequest.Headers.Remove("X-ZUMO-AUTH");
					clonedRequest.Headers.Add("X-XUMO-AUTH", Client.CurrentUser.MobileServiceAuthenticationToken);
					response = await base.SendAsync(clonedRequest, cancellationToken);

					// Refresh token didn't work or does not exist, prompt user to re-authenticate.
					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						// Was the user previously authenticated, at any time?
						var authenticator = Platform.Instance.Authenticator;
						if (authenticator.UserPreviouslyAuthenticated)
						{
							var provider = authenticator.FindIdentityProvider();
							await authenticator.LoginAsync(Client, provider);
						}
						else
							throw new UnauthorizedAccessException("User is not authenticated.");
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