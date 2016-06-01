using System.Threading.Tasks;
using Azure.Mobile;
using Azure.Mobile.Abstractions;
using Azure.Mobile.Utils;
using FormsSample.DataStores;
using FormsSample.Models;
using Xamarin.Forms;

namespace FormsSample
{
    public partial class App : Application
    {
        IEasyMobileServiceClient client;
        public App()
        {
            InitializeComponent();

            client = new EasyMobileServiceClient("http://xamarin-todo-sample.azurewebsites.net");
            client.RegisterTable<ToDo, ToDoStore>(); 

            MainPage = new NavigationPage(new Pages.ToDoListPage(client));
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

