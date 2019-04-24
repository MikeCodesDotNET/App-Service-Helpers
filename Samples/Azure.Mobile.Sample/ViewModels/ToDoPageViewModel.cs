using System;
using System.Windows.Input;
using AppServiceHelpers.Abstractions;
using Azure.Mobile.Sample.Models;
using Xamarin.Forms;

namespace Azure.Mobile.Sample.ViewModels
{
    public class ToDoPageViewModel
    {
       public ToDo ToDo { get; set; }

       public ToDoPageViewModel(IEasyMobileServiceClient client, ToDo selectedItem)
       {
            ToDo = selectedItem;
            this.client = client;
       }

        private ICommand _saveItemCommand;
        public ICommand SaveItemCommand
        {
            get
            {
                _saveItemCommand = _saveItemCommand ?? new Command(async () =>
                {
                    if (ToDo.Id == null)
                    {
                        await client.Table<ToDo>().AddAsync(ToDo);
                    }
                    else
                    {
                        await client.Table<ToDo>().UpdateAsync(ToDo);
                    }
                    var navigation = Application.Current.MainPage as NavigationPage;
                    await navigation.PopAsync();
                });
                return _saveItemCommand; ;
            }
        }

        private ICommand _deleteItemCommand;
        public ICommand DeleteItemCommand
        {
            get
            {
                _deleteItemCommand = _deleteItemCommand ?? new Command(async () =>
                {
                    await client.Table<ToDo>().DeleteAsync(ToDo);
                    var navigation = Application.Current.MainPage as NavigationPage;
                    await navigation.PopAsync();
                });
                return _deleteItemCommand; ;
            }
        }


        IEasyMobileServiceClient client;
    }

}
