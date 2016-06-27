using System;
using System.Collections.Generic;
using Azure.Mobile;
using Azure.Mobile.Abstractions;
using Xamarin.Forms;

namespace FormsSample.Pages
{
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage(IEasyMobileServiceClient client)
        {
            InitializeComponent();
            BindingContext = new ViewModels.ToDosViewModel(client);
        }
    }
}

