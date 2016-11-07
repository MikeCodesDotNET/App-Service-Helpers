using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FormsSample
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();

			BindingContext = new LoginViewModel();
		}
	}
}