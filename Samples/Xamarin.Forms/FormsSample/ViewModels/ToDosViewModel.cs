using System;
using System.Threading.Tasks;
using FormsSample.Models;
using Xamarin.Forms;

using AppServiceHelpers.Data.Collections;

namespace FormsSample.ViewModels
{
	public class ToDosViewModel : BaseViewModel
    {
        public ToDosViewModel()
        {
            var client = AppServiceHelpers.EasyMobileServiceClient.Current;

			Todos = new ConnectedObservableCollection<ToDo>(client.Table<ToDo>());
			ExecuteRefreshCommand();
        }

		ConnectedObservableCollection<ToDo> todos;
		public ConnectedObservableCollection<ToDo> Todos
		{
			get { return todos; }
			set { todos = value; OnPropertyChanged("Todos"); }
		}

		Command refreshCommand;
		public Command RefreshCommand
		{
			get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
		}

		async Task ExecuteRefreshCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				await Todos.Refresh();
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		Command addNewItemCommand;
		public Command AddNewItemCommand
		{
			get { return addNewItemCommand ?? (addNewItemCommand = new Command(async () => await ExecuteAddNewItemCommandAsync())); }
		}

		async Task ExecuteAddNewItemCommandAsync()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				Todos.Add(new ToDo
				{
					Text = DateTime.Now.ToString(),
					Completed = false
				});
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}
    }
}