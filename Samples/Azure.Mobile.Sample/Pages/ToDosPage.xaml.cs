using System;
using System.Collections.Generic;
using AppServiceHelpers.Abstractions;
using Xamarin.Forms;

namespace Azure.Mobile.Sample.Pages
{
    public partial class ToDosPage : ContentPage
    {
        public ToDosPage(IEasyMobileServiceClient client)
        {
            InitializeComponent();
            BindingContext = new ViewModels.ToDosPageViewModel(client);
        }
    }
}
