using System;
using System.Threading.Tasks;
using AppServiceHelpers.Abstractions;
using AppServiceHelpers.Helpers;
using Azure.Mobile.Sample.Models;
using Xamarin.Forms;

namespace Azure.Mobile.Sample.ViewModels
{
    public class ToDosPageViewModel
    {
        public ObservableRangeCollection<ToDo> ToDos { get; set; }

        public ToDosPageViewModel(IEasyMobileServiceClient client)
        {
            this.client = client;
            this.toDoTable = client.Table<ToDo>();

            ToDos = new ObservableRangeCollection<ToDo>();
        }


        public Command RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
        }

        private Command addNewItemCommand;
        public Command AddNewItemCommand
        {
            get
            {
                addNewItemCommand = addNewItemCommand ?? new Command(() =>
                {
                    var navigation = Application.Current.MainPage as NavigationPage;
                    navigation.PushAsync(new Pages.ToDoPage(client, new ToDo()));
                });
                return addNewItemCommand;
            }
        }

        private async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var _items = await toDoTable.GetItemsAsync();
                ToDos.Clear();
                foreach (var item in _items)
                {
                    ToDos.Add(item);
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        ToDo selectedToDoItem;
        public ToDo SelectedToDoItem
        {
            get { return selectedToDoItem; }
            set
            {
                selectedToDoItem = value;
                if (selectedToDoItem != null)
                {
                    var navigation = Application.Current.MainPage as NavigationPage;
                    navigation.PushAsync(new Pages.ToDoPage(client, selectedToDoItem));
                    SelectedToDoItem = null;
                }
            }
        }



        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }

        private bool isBusy; 
        private IEasyMobileServiceClient client;
        private ITableDataStore<ToDo> toDoTable;
        private Command refreshCommand;

    }
}
