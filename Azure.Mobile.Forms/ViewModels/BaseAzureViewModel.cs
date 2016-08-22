using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using AppServiceHelpers.Abstractions;
using AppServiceHelpers.Models;

using Xamarin.Forms;

namespace AppServiceHelpers.Forms
{
    public class BaseAzureViewModel<T> : INotifyPropertyChanged where T : EntityData
    {
        IEasyMobileServiceClient client;
        ITableDataStore<T> table;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Azure.Mobile.Forms.BaseAzureViewModel`1"/> class.
		/// </summary>
		/// <param name="client">EasyMobileServiceClient for performing table operations.</param>
        public BaseAzureViewModel(IEasyMobileServiceClient client)
        {
            this.client = client;
            table = client.Table<T>();
        }

        /// <summary>
        /// Returns an ObservableCollection of all the items in the table.
        /// </summary>
        ObservableCollection<T> items = new ObservableCollection<T>();
        public ObservableCollection<T> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("items");
            }
        }

		/// <summary>
		/// Adds an item to the table.
		/// </summary>
		/// <param name="item">The item to add to the table.</param>
		public async Task AddItemAsync(T item)
        {
            await table.AddAsync(item);
        }

		/// <summary>
		/// Deletes an item from the table.
		/// </summary>
		/// <param name="item">The item to delete from the table.</param>
		public async Task DeleteItemAsync(T item)
        {
            await table.DeleteAsync(item);
        }

		/// <summary>
		/// Updates an item in the table.
		/// </summary>
		/// <param name="item">The item to update in the table.</param>
		public async Task UpdateItemAsync(T item)
        {
            await table.UpdateAsync(item);
        }

        /// <summary>
        /// Refresh the table, and sychronize data with Azure.
        /// </summary>
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
                var _items = await table.GetItemsAsync();
                Items.Clear();
                foreach (var item in _items)
                {
                    Items.Add(item);
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        string title = string.Empty;
        /// <summary>
        /// The title of the page.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        string subTitle = string.Empty;
        /// <summary>
        /// The subtitle of the page.
        /// </summary>
        public string Subtitle
        {
            get { return subTitle; }
            set { SetProperty(ref subTitle, value); }
        }

        string icon = null;
        /// <summary>
        /// The icon of the page.
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value); }
        }

        bool isBusy;
        /// <summary>
        /// The current state of the view.
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        bool canLoadMore = true;
        /// <summary>
        /// Can we load more items?
        /// </summary>
        public bool CanLoadMore
        {
            get { return canLoadMore; }
            set { SetProperty(ref canLoadMore, value); }
        }

        protected void SetProperty<TStore>(
            ref TStore backingStore, TStore value,
            [CallerMemberName]string propertyName = null,
            Action onChanged = null)
        {


            if (EqualityComparer<TStore>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

		/// <summary>
		/// An implementation of INotifyPropertyChanged.
		/// </summary>
		/// <param name="propertyName">The property name to fire the PropertyChanged event on.</param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}