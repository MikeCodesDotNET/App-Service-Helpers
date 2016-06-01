using System;
using System.Collections.Generic;
using Azure.Mobile;
using Xamarin.Forms;

namespace FormsSample.Pages
{
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage(EasyMobileServiceClient client)
        {
            InitializeComponent();
            BindingContext = new ViewModels.ToDoViewModel(client);
        }
    }
}

