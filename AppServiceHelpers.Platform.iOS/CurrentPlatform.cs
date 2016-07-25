using AppServiceHelpers.Utils;
using AppServiceHelpers.Platform.iOS;

namespace AppServiceHelpers
{
	public class CurrentPlatform
	{
		public static void Init()
		{
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
			SQLitePCL.CurrentPlatform.Init();

			ServiceLocator.Instance.Add<IAuthenticator, Authenticator>();
		}
	}
}