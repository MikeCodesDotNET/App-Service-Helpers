using System;
using Azure.Mobile;

using FormsSample.Models;
using FormsSample.DataStores;
using Xamarin.Forms;
using System.Threading.Tasks;
using Azure.Mobile.Abstractions;
using System.Collections.ObjectModel;
using Azure.Mobile.Forms;

namespace FormsSample.ViewModels
{
    public class ToDoViewModel : BaseFormsViewModel<ToDo>
    {
        public ToDoViewModel()
        {
            Title = "ToDo";
        }

    }
}

