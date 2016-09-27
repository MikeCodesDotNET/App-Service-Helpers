using AppServiceHelpers.Authentication;

namespace AppServiceHelpers.Platform
{
	public interface IPlatform
	{
		IAuthenticator Authenticator { get; }
	}
}