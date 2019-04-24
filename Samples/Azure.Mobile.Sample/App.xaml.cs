using System;
using AppServiceHelpers;
using AppServiceHelpers.Abstractions;
using Azure.Mobile.Sample.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Azure.Mobile.Sample
{
    public partial class App : Application
    {
        IEasyMobileServiceClient client;

        public App()
        {
            InitializeComponent();

            client = new EasyMobileServiceClient();
            client.Initialize("http://xamarin-todo-sample.azurewebsites.net");
            client.RegisterTable<ToDo>();
            client.FinalizeSchema();

            MainPage = new NavigationPage(new Pages.ToDosPage(client) { Title = "ToDo" });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
