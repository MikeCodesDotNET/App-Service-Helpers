using System;
using System.Threading.Tasks;
using AppServiceHelpers;
using FormsSample.ViewModels;

using Xamarin.Forms;

namespace FormsSample
{
	public class LoginViewModel : BaseViewModel
	{
	    private readonly IEasyMobileServiceClient client;

		public LoginViewModel()
		{
            client = EasyMobileServiceClient.Current;
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

                var navigationPage = App.Current.MainPage as NavigationPage;
			    if (navigationPage != null)
			    {
			        var todos = new Pages.ToDoListPage();
			        await navigationPage.PushAsync(todos);
			    }
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