using System;
using Azure.Mobile;

using FormsSample.Models;
using FormsSample.DataStores;

namespace FormsSample.ViewModels
{
    public class ToDoViewModel 
    {
        public EasyMobileServiceClient ServiceClient;

        public ToDoViewModel()
        {
            ServiceClient = new EasyMobileServiceClient("");
            Init();
        }

        public async void Init()
        {
           await ServiceClient.RegisterTable<ToDo, ToDoStore>();
        }
    }
}

