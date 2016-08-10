using System;
using System.Windows.Input;
using AppServiceHelpers.Abstractions;
using Xamarin.Forms;

namespace FormsSample.ViewModels
{
    public class ToDoViewModel : AppServiceHelpers.Forms.BaseAzureViewModel<Models.ToDo>
    {
        IEasyMobileServiceClient client;
        Models.ToDo todo;

        public ToDoViewModel(IEasyMobileServiceClient client, Models.ToDo todo) : base (client)
        {
            this.client = client;
            this.todo = todo;

            Title = todo.Id == null ? "Add To Do" : "Edit To Do";
        }

        public string Text
        {
            get
            {
                return todo.Text;   
            }
            set
            {
                todo.Text = value;
            }
        }

        public bool Completed
        {
            get
            {
                return todo.Completed;
            }
            set
            {
                todo.Completed = value;
            }
        }

        private ICommand _saveItemCommand;
        public ICommand SaveItemCommand
        {
            get
            {
                _saveItemCommand = _saveItemCommand ?? new Command(async () =>
                {
                    if (todo.Id == null)
                    {
                        await AddItemAsync(todo);
                    }
                    else
                    {
                        await UpdateItemAsync(todo);
                    }
                    var navigation = Application.Current.MainPage as NavigationPage;
                    navigation.PopAsync();
                });
                return _saveItemCommand; ;
            }
        }
    }
}

