using Android.Content;

using AppServiceHelpers.Authentication;
using AppServiceHelpers.Platform;

namespace AppServiceHelpers
{
	public class CurrentPlatform : IPlatform
	{
		public static Context Context { get; set; }

		IAuthenticator IPlatform.Authenticator
		{
			get
			{
				return Authenticator.Instance;
			}
		}

		public static void Init(Context context)
		{
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			Context = context;
		}
	}
}