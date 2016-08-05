using System;

namespace AppServiceHelpers
{
	public interface IPlatform
	{
		IAuthenticator Authenticator { get; }
	}
}