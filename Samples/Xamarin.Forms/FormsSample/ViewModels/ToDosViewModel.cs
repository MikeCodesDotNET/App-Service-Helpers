using System;
using AppServiceHelpers;

using FormsSample.Models;
using FormsSample.DataStores;
using Xamarin.Forms;
using System.Threading.Tasks;
using AppServiceHelpers.Abstractions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppServiceHelpers.Forms;

namespace FormsSample.ViewModels
{
    public class ToDosViewModel : BaseAzureViewModel<ToDo>
    {
        IEasyMobileServiceClient client;
        public ToDosViewModel(IEasyMobileServiceClient client) : base (client)
        {
            this.client = client;

            Title = "To Do List";
        }

        Models.ToDo selectedToDoItem;
        public Models.ToDo SelectedToDoItem
        {
            get { return selectedToDoItem; }
            set
            {
                selectedToDoItem = value;
                OnPropertyChanged("SelectedItem");

                if (selectedToDoItem != null)
                {
                    var navigation = Application.Current.MainPage as NavigationPage;
                    navigation.PushAsync(new Pages.ToDoPage(client, selectedToDoItem));
                    SelectedToDoItem = null;
                }
            }
        }

        private ICommand _addNewItemCommand;
        public ICommand AddNewItemCommand
        {
            get
            {
                _addNewItemCommand = _addNewItemCommand ?? new Command(() =>
                {
                    var navigation = Application.Current.MainPage as NavigationPage;
                    navigation.PushAsync(new Pages.ToDoPage(client, new ToDo()));
                });
                return _addNewItemCommand;
            }
        }

    }
}

