using System;
using System.Threading.Tasks;

using FormsSample.ViewModels;

using AppServiceHelpers.Abstractions;
using AppServiceHelpers.Helpers;
using Xamarin.Forms;

namespace FormsSample
{
	public class LoginViewModel : BaseViewModel
	{
		IEasyMobileServiceClient client;

		public LoginViewModel(IEasyMobileServiceClient client)
		{
			this.client = client;
		}

		Command loginCommand;
		public Command LoginCommand
		{
			get { return loginCommand ?? (loginCommand = new Command(async () => await ExecuteLoginCommandAsync())); }
		}

		async Task ExecuteLoginCommandAsync()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				await client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Facebook);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}


