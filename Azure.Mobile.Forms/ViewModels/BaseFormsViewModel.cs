using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

using Azure.Mobile.Abstractions;
using Azure.Mobile.Models;

using Xamarin.Forms;

namespace Azure.Mobile.Forms
{
    public class BaseFormsViewModel<T> where T : EntityData
    {
        IEasyMobileServiceClient client;
        ITableDataStore<T> table;

        public BaseFormsViewModel(IEasyMobileServiceClient client)
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

        public async Task AddItemAsnyc(T item)
        {
           await table.Add(item); 
        }

        public async Task DeleteItemAsnyc(T item)
        {
            await table.Delete(item);
        }

        public async Task UpdateItemAsnyc(T item)
        {
            await table.Update(item);
        }

        /// <summary>
        /// Refresh the table (pushes and pulls data from Azure)
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
                var _items = await table.GetItems();
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
        public const string TitlePropertyName = "Title";

        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value, TitlePropertyName); }
        }

        string subTitle = string.Empty;
        /// <summary>
        /// Gets or sets the "Subtitle" property
        /// </summary>
        public const string SubtitlePropertyName = "Subtitle";
        public string Subtitle
        {
            get { return subTitle; }
            set { SetProperty(ref subTitle, value, SubtitlePropertyName); }
        }

        string icon = null;
        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public const string IconPropertyName = "Icon";
        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value, IconPropertyName); }
        }

        bool isBusy;
        /// <summary>
        /// Gets or sets if the view is busy.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value, IsBusyPropertyName); }
        }

        bool canLoadMore = true;
        /// <summary>
        /// Gets or sets if we can load more.
        /// </summary>
        public const string CanLoadMorePropertyName = "CanLoadMore";
        public bool CanLoadMore
        {
            get { return canLoadMore; }
            set { SetProperty(ref canLoadMore, value, CanLoadMorePropertyName); }
        }

        protected void SetProperty<T>(
            ref T backingStore, T value,
            string propertyName,
            Action onChanged = null)
        {


            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

