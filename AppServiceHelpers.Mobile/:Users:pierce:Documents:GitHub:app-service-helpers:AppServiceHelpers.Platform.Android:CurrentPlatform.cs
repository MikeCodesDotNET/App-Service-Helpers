using System;

using Android.Content;

using Microsoft.WindowsAzure.MobileServices;

namespace AppServiceHelpers.Platform.Android
{
	public class CurrentPlatform
	{
		public static void Init(Context context)
		{
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
		}
	}
}