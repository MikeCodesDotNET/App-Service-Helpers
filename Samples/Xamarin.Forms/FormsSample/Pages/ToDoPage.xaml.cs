using System;
using System.Collections.Generic;
using AppServiceHelpers.Abstractions;
using Xamarin.Forms;

namespace FormsSample.Pages
{
    public partial class ToDoPage : ContentPage
    {
        public ToDoPage(IEasyMobileServiceClient client, Models.ToDo todo)
        {
            InitializeComponent();

            var viewModel = new ViewModels.ToDoViewModel(client, todo);
            BindingContext = viewModel;
        }
    }
}

