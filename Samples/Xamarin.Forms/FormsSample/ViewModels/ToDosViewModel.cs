using System;
using AppServiceHelpers;

using FormsSample.Models;
using FormsSample.DataStores;
using Xamarin.Forms;
using System.Threading.Tasks;
using AppServiceHelpers.Abstractions;
using System.Collections.ObjectModel;
using AppServiceHelpers.Forms;

namespace FormsSample.ViewModels
{
    public class ToDosViewModel : BaseAzureViewModel<ToDo>
    {
        IEasyMobileServiceClient client;
        public ToDosViewModel(IEasyMobileServiceClient client) : base (client)
        {
            this.client = client;
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
    }
}

