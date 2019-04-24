using System;
using System.Collections.Generic;
using AppServiceHelpers.Abstractions;
using Azure.Mobile.Sample.Models;
using Xamarin.Forms;

namespace Azure.Mobile.Sample.Pages
{
    public partial class ToDoPage : ContentPage
    {
        public ToDoPage(IEasyMobileServiceClient client, ToDo selectedItem)
        {
            InitializeComponent();
            BindingContext = new ViewModels.ToDoPageViewModel(client, selectedItem);
        }
    }
}
