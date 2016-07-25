using Android.Content;

using AppServiceHelpers.Utils;
using AppServiceHelpers.Platform.Android;

namespace AppServiceHelpers
{
	public class CurrentPlatform
	{
		public static Context Context { get; set; }

		public static void Init(Context context)
		{
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			ServiceLocator.Instance.Add<IAuthenticator, Authenticator>();
		}
	}
}