using AppServiceHelpers;
using AppServiceHelpers.Abstractions;
using FormsSample.Models;
using Xamarin.Forms;

namespace FormsSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            EasyMobileServiceClient.Current.Initialize("http://xamarin-todo-sample.azurewebsites.net");
            EasyMobileServiceClient.Current.RegisterTable<ToDo>();
            EasyMobileServiceClient.Current.FinalizeSchema();

            MainPage = new NavigationPage(new Pages.ToDoListPage());
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

