using System;
using AppServiceHelpers.Utils;

namespace AppServiceHelpers
{
	public class CurrentPlatform : IPlatform
	{
		IAuthenticator IPlatform.Authenticator
		{
			get
			{
				return Authenticator.Instance;
			}
		}

		public static void Init()
		{
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
			SQLitePCL.CurrentPlatform.Init();
		}
	}
}