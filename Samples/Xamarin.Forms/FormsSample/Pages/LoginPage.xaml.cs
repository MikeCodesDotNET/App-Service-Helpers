using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FormsSample
{
	public partial class LoginPage : ContentPage
	{
		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			var client = AppServiceHelpers.EasyMobileServiceClient.Current;
			await client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Facebook);
		}

		public LoginPage()
		{
			InitializeComponent();

			BindingContext = new LoginViewModel();
		}


	}
}

