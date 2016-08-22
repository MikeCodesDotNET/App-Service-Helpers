using System;
using System.Collections.Generic;
using AppServiceHelpers;
using AppServiceHelpers.Abstractions;
using FormsSample.ViewModels;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = (ToDosViewModel) BindingContext;
            vm.RefreshCommand.Execute(null);
        }
    }
}

