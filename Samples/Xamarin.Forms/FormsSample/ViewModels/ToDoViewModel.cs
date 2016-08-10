using System;
using AppServiceHelpers.Abstractions;

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

        public bool Complete
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



    }
}

